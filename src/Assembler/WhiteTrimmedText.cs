// <copyright file="WhiteTrimmedText.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Assembler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public sealed class WhiteTrimmedText : IReadOnlyList<char>
    {
        public WhiteTrimmedText(string text)
            : this(new PreprocessedText(text))
        {
        }

        public WhiteTrimmedText(PreprocessedText preprocessedText)
        {
            var builder = new Builder(preprocessedText);
            Text = builder.ToString();
        }

        public PreprocessedText PreprocessedText
        {
            get;
        }

        public int Length
        {
            get
            {
                return Text.Length;
            }
        }

        int IReadOnlyCollection<char>.Count
        {
            get
            {
                return Length;
            }
        }

        private string Text
        {
            get;
        }

        public char this[int index]
        {
            get
            {
                return Text[index];
            }
        }

        public override string ToString()
        {
            return Text;
        }

        public CharEnumerator GetEnumerator()
        {
            return Text.GetEnumerator();
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private unsafe sealed class Builder
        {
            private int i;
            private char lastAppended;

            public Builder(
                PreprocessedText preprocessedText)
            {
                PreprocessedText = preprocessedText ??
                    throw new ArgumentNullException(
                        nameof(preprocessedText));

                lastAppended = '\n';
                StringBuilder = new StringBuilder(Length);
                for (i = 0; i < Length;)
                {
                    // Trims repeating white space to a single space.
                    if (TrimWhiteSpace())
                    {
                        continue;
                    }

                    // Trims line comments to a single space.
                    if (TrimLineComment())
                    {
                        continue;
                    }

                    // Trims block comments to a single space or empty line.
                    if (TrimBlockComment())
                    {
                        continue;
                    }

                    // Ensure that strings are properly closed in either single or double quotes.
                    if (AppendQuoteText())
                    {
                        continue;
                    }

                    // Newlines are special cases that check for whitespace-only lines.
                    if (AppendNewLine())
                    {
                        i++;
                        lastAppended = '\n';
                        continue;
                    }

                    // default case.
                    lastAppended = Current;
                    StringBuilder.Append(lastAppended);
                    i++;
                }
            }

            private PreprocessedText PreprocessedText
            {
                get;
            }

            private char Current
            {
                get
                {
                    return PreprocessedText[i];
                }
            }

            private char Next
            {
                get
                {
                    return PreprocessedText[i + 1];
                }
            }

            private bool CurrentIsLineCommentOpenTag
            {
                get
                {
                    return Current == ';';
                }
            }

            private bool CurrentIsNewLine
            {
                get
                {
                    return Current == '\n';
                }
            }

            private bool CurrentIsWhiteSpace
            {
                get
                {
                    return Current == ' ' || Current == '\t';
                }
            }

            private bool CurrentIsBlockCommentOpenTag
            {
                get
                {
                    return Current == '/' && Next == '*';
                }
            }

            private bool CurrentIsBlockCommentCloseTag
            {
                get
                {
                    return Current == '*' && Next == '/';
                }
            }

            private bool CurrentIsQuoteTag
            {
                get
                {
                    return Current == '\'' || Current == '"';
                }
            }

            private int Length
            {
                get
                {
                    return PreprocessedText.Length;
                }
            }

            private StringBuilder StringBuilder
            {
                get;
            }

            public override string ToString()
            {
                return StringBuilder.ToString();
            }

            private bool TrimWhiteSpace()
            {
                // Check for the start of whitespace
                if (CurrentIsWhiteSpace)
                {
                    // Skip all repeating whitespace characters
                    do
                    {
                        i++;
                    }
                    while (CurrentIsWhiteSpace);

                    AppendWhite();
                    return true;
                }

                return false;
            }

            private bool TrimLineComment()
            {
                // Check for the start of a line comment.
                if (CurrentIsLineCommentOpenTag)
                {
                    // Skip all characters until we reach a newline.
                    do
                    {
                        i++;
                    }
                    while (!CurrentIsNewLine);

                    AppendWhite();
                    return true;
                }

                return false;
            }

            private bool TrimBlockComment()
            {
                // Check for the start of a block comment: /*
                if (!CurrentIsBlockCommentOpenTag)
                {
                    return false;
                }

                // Append a new space if we have not done so already.
                AppendWhite();

                for (i += 2; i < Length; i++)
                {
                    // Check for end of block comment: */
                    if (CurrentIsBlockCommentCloseTag)
                    {
                        // Update index to end of block comment.
                        i += 2;
                        return true;
                    }

                    // We do this to know which line the code is on when processing.
                    AppendNewLine();
                }

                // Block comment not closed.
                Console.Error.WriteLine("Block comment not closed.");
                return true;
            }

            private bool AppendQuoteText()
            {
                // Check if this a quote char: ' or "
                if (CurrentIsQuoteTag)
                {
                    // Save quote char for match.
                    lastAppended = Current;
                    StringBuilder.Append(Current);

                    // Wait for newline char.
                    for (i++; CurrentIsNewLine; i++)
                    {
                        // Append new char.
                        StringBuilder.Append(Current);

                        // Quit once we get the closing char.
                        if (Current == lastAppended)
                        {
                            return true;
                        }
                    }

                    // Close string if not closed.
                    StringBuilder.Append(lastAppended);

                    Console.Error.WriteLine("String not closed.");
                    return true;
                }

                return false;
            }

            private bool AppendNewLine()
            {
                // Check for newline (no '\r' is guaranteed).
                if (!CurrentIsNewLine)
                {
                    return false;
                }

                RemoveTrailingWhite();
                StringBuilder.Append('\n');
                return true;

                void RemoveTrailingWhite()
                {
                    var len = StringBuilder.Length;
                    if (len > 0 && StringBuilder[len - 1] == ' ')
                    {
                        // Remove trailing white space.
                        StringBuilder.Remove(len - 1, 1);
                    }
                }
            }

            // Append a single space so long as our last char wasn't a space or newline.
            private void AppendWhite()
            {
                // Don't append extra spaces.
                if (lastAppended == ' ' || lastAppended == '\n')
                {
                    return;
                }

                // Append a new space.
                StringBuilder.Append(' ');
                lastAppended = ' ';
            }
        }
    }
}
