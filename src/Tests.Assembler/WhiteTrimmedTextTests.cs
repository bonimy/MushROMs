using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler;

namespace Tests.Assembler
{
    [TestClass]
    public class WhiteTrimmedTextTests
    {
        [TestMethod]
        public void TrimNullText()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new WhiteTrimmedText(
                    null as PreprocessedText);
            });
        }

        [TestMethod]
        public void TrimEmptyText()
        {
            AssertTrimResult(
                String.Empty,
                String.Empty);
        }

        [TestMethod]
        public void TrimSingleLineComment()
        {
            AssertTrimResult(
                ";Comment",
                "\n");
        }

        [TestMethod]
        public void TrimTrivialBlockComment()
        {
            AssertTrimResult(
                "/**/",
                "\n");
        }

        [TestMethod]
        public void TrimWhitePaddedLineComment()
        {
            AssertTrimResult(
                "\n\t    ;Comment",
                "\n\n");
        }

        [TestMethod]
        public void TrimMultipleBlocksCommentsOnSameEmptyLine()
        {
            AssertTrimResult(
                "/*block*/  /* comment */",
                "\n");
        }

        [TestMethod]
        public void TrimConjoinedBlockComments()
        {
            AssertTrimResult(
                "/**//**//* more blocks*/",
                "\n");
        }

        [TestMethod]
        public void TrimMultiLineBlockComment()
        {
            AssertTrimResult(
                "\t/* block comment\nstuff\n*/",
                "\n\n\n");
        }

        [TestMethod]
        public void TestBlockCommentToLineComment()
        {
            AssertTrimResult(
                "/*Block Comment*/;Line Comment",
                "\n");
        }

        [TestMethod]
        public void TestBlockCommentToLineCommentWithWhiteSpace()
        {
            AssertTrimResult(
                " /*Block Comment*/\t;Line Comment",
                "\n");
        }

        [TestMethod]
        public void TestBlockCommentWithBlockCommentChars()
        {
            AssertTrimResult(
                "/*/* /*/",
                "\n");
        }

        [TestMethod]
        public void TestBlockCommentWithLineCommentChars()
        {
            AssertTrimResult(
                "/*;*/",
                "\n");
        }

        [TestMethod]
        public void TestLineCommentWithLineCommentChars()
        {
            AssertTrimResult(
                ";;;;;;",
                "\n");
        }

        [TestMethod]
        public void TestLineCommentWithOpenBlockCommentChars()
        {
            AssertTrimResult(
                ";some/*text",
                "\n");
        }

        [TestMethod]
        public void TestLineCommentWithCloseBlockCommentChars()
        {
            AssertTrimResult(
                ";more*/text",
                "\n");
        }

        [TestMethod]
        public void TestTextWithLineComment()
        {
            AssertTrimResult(
                "Hello ;World!",
                "Hello\n");
        }

        [TestMethod]
        public void TestTextWithBlockComment()
        {
            AssertTrimResult(
                "Hello /*dumb*/ world!",
                "Hello world!\n");
        }

        [TestMethod]
        public void TestTextWithBlockAndLineComments()
        {
            AssertTrimResult(
                "Hello /* dumb */ world ;!",
                "Hello world\n");
        }

        private static void AssertTrimResult(
            string text,
            string expected)
        {
            var actual = new WhiteTrimmedText(text).ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
