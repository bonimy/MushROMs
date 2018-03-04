// <copyright file="PreprocessedText.cs" company="Public Domain">
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
    using System.Collections.ObjectModel;
    using System.Text;
    using static Helper.SR;

    public sealed class PreprocessedText : IReadOnlyList<char>
    {
        private static readonly IReadOnlyDictionary<char, char>
            TrigraphDictionary = new ReadOnlyDictionary<char, char>(
                new Dictionary<char, char>()
                {
                    { '=', '#' },
                    { ')', ']' },
                    { '!', '|' },
                    { '(', '[' },
                    { '\'', '^' },
                    { '>', '}' },
                    { '/', '\\' },
                    { '<', '{' },
                    { '-', '~' }
                });

        public PreprocessedText(string text)
            : this(text, false, false)
        {
        }

        public PreprocessedText(
            string text,
            bool resolveTrigraphs,
            bool asciiCompliant)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            TrigraphsResolved = resolveTrigraphs;
            AsciiCompliant = asciiCompliant;
            unsafe
            {
                fixed (char* str = text)
                {
                    var builder = new Builder(
                        str,
                        text.Length,
                        resolveTrigraphs,
                        asciiCompliant);

                    Text = builder.ToString();
                }
            }
        }

        public bool TrigraphsResolved
        {
            get;
        }

        public bool AsciiCompliant
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

        public static bool IsTrigraphChar(char c)
        {
            return TrigraphDictionary.ContainsKey(c);
        }

        public static bool TryGetTrigraph(char c, out char trigraph)
        {
            return TrigraphDictionary.TryGetValue(c, out trigraph);
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

            public Builder(
                char* str,
                int length,
                bool resolveTrigraphs,
                bool isAsciiCompliant)
            {
                Text = str;
                Length = length;
                TrigraphsResolved = resolveTrigraphs;
                AsciiCompliant = isAsciiCompliant;
                StringBuilder = new StringBuilder(length);

                for (i = 0; i < length;)
                {
                    // Add \n for \r or \r\n.
                    if (AppendAltNewLine())
                    {
                        continue;
                    }

                    // Replace trigraph sequences.
                    if (AppendTrigraphSequence())
                    {
                        continue;
                    }

                    // Replace non-ascii chars with their escape sequences.
                    if (AppendAsciiEscapeSequence())
                    {
                        continue;
                    }

                    // Append the next character.
                    StringBuilder.Append(Text[i++]);
                }

                // Make sure string ends with empty line.
                AppendLastNewLine();
            }

            private char* Text
            {
                get;
            }

            private int Length
            {
                get;
            }

            private bool TrigraphsResolved
            {
                get;
            }

            private bool AsciiCompliant
            {
                get;
            }

            private StringBuilder StringBuilder
            {
                get;
            }

            public override string ToString()
            {
                return StringBuilder.ToString();
            }

            private bool AppendAltNewLine()
            {
                // Ignore cases without alt-newline character.
                if (Text[i] != '\r')
                {
                    return false;
                }

                // Append the desired newline character.
                StringBuilder.Append('\n');

                // Ignore \n char of \r\n sequence. Update position.
                if (Text[++i] == '\n')
                {
                    i++;
                }

                return true;
            }

            private bool AppendTrigraphSequence()
            {
                // Ignore if we aren't resolving trigraph sequences.
                if (!TrigraphsResolved)
                {
                    return false;
                }

                // Ignore if we don't start a trigraph sequence
                if (Text[i] != '?' || Text[i + 1] != '?')
                {
                    return false;
                }

                // Update position to trigraph char index.
                i += 2;

                // Is this a trigraph char?
                if (TryGetTrigraph(Text[i], out var trigraph))
                {
                    // Append the char and update position.
                    StringBuilder.Append(trigraph);
                    i++;
                    return true;
                }

                // Append text that wasn't a trigraph sequence.
                StringBuilder.Append("??");
                return false;
            }

            private bool AppendAsciiEscapeSequence()
            {
                // Ignore if we don't need to worry about ascii compliance.
                if (!AsciiCompliant)
                {
                    return false;
                }

                // Get the number code of the current char.
                var code = (int)Text[i];

                // Ignore valid ascii chars.
                if (code <= 0x7F)
                {
                    return false;
                }

                // Append escape sequence \uXX or \uXXXX.
                StringBuilder.Append("\\u");
                var format = code < 0x100 ?
                    "{0:X2}" :
                    "{0:X4}";

                var result = GetString(format, code);
                StringBuilder.Append(result);
                return true;
            }

            private void AppendLastNewLine()
            {
                // Don't add a new line to empty strings.
                if (StringBuilder.Length == 0)
                {
                    return;
                }

                // Don't add a new line if one already exists.
                if (StringBuilder[StringBuilder.Length - 1] == '\n')
                {
                    return;
                }

                StringBuilder.Append('\n');
            }
        }
    }
}
