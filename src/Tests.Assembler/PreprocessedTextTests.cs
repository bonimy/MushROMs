using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler;

namespace Tests.Assembler
{
    [TestClass]
    public class PreprocessedTextTests
    {
        [TestMethod]
        public void ResolveEmptyString()
        {
            var preprocessed = new PreprocessedText(String.Empty);

            Assert.AreEqual(preprocessed.Length, 0);
            Assert.AreEqual(preprocessed.ToString(), String.Empty);
        }

        [TestMethod]
        public void ResolveNullString()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new PreprocessedText(null);
            });
        }

        [TestMethod]
        public void AddTrailingNewLine()
        {
            // Append newline to end of text files.
            var preprocessed = new PreprocessedText("Hello");
            var text = preprocessed.ToString();
            Assert.AreEqual(text, "Hello\n");
        }

        [TestMethod]
        public void LimitOneTrailingNewLine()
        {
            // Don't append if newline already exists.
            var preprocessed = new PreprocessedText("Hello\nWorld!\n");
            var text = preprocessed.ToString();
            Assert.AreEqual(text, "Hello\nWorld!\n");
        }

        [TestMethod]
        public void ResolveTrigraphs()
        {
            // There might be a better way to design this string...
            var text = "??=??) ??! abc??(??\'??>??/??< 0\n zxy??-";
            var result = "#] | abc[^}\\{ 0\n zxy~\n";

            var preprocessed = new PreprocessedText(text, true, true);
            Assert.AreEqual(preprocessed.ToString(), result);
        }
    }
}
