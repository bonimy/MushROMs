using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class ExtensionDictionaryTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            var dictionary = new ExtensionDictionary<int>();
            Assert.AreEqual(dictionary.Comparer, ExtensionComparer.DefaultComparer);

            var comparer = new ExtensionComparer(StringComparer.Ordinal);
            dictionary = new ExtensionDictionary<int>(comparer);
            Assert.AreEqual(dictionary.Comparer, comparer);

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary = new ExtensionDictionary<int>((ExtensionComparer)null);
            });

            // We should probably test other constructors, but if these three work, then
            // we're probably in good shape.
        }

        [TestMethod]
        public void UsageTests()
        {
            // We test the extension dictionary usage overall, since a lot of its functionality
            // is shared.

            // Let's create a dictionary of editor file associations.
            var dictionary = new ExtensionDictionary<Editor>
            {
                // Add file association for each editor.
                { ".txt", Editor.TextEditor },
                { ".bin", Editor.BinaryEditor },
                { ".rtf", Editor.RichTextEditor },
                { ".cs", Editor.SourceCodeEditor }
            };

            // Simple sanity test.
            var editor = dictionary[".txt"];
            Assert.AreEqual(editor, Editor.TextEditor);

            // File name with the extension should return appropriate editor.
            var file = "Data.bin";
            editor = dictionary[file];
            Assert.AreEqual(editor, Editor.BinaryEditor);

            // Full paths should also return appropriate editor.
            editor = dictionary[@"C:\path\to\..\file.rtf"];
            Assert.AreEqual(editor, Editor.RichTextEditor);

            // Two extensions can have the same editor.
            file = "Document.doc";
            dictionary[file] = Editor.TextEditor;
            Assert.AreEqual(dictionary[file], dictionary[".txt"]);

            // File types are not implied.
            Assert.AreNotEqual(dictionary[".doc"], dictionary[".rtf"]);

            // But we can explicitly define them again.
            dictionary[".doc"] = Editor.RichTextEditor;
            Assert.AreEqual(dictionary[file], Editor.RichTextEditor);

            Assert.IsTrue(dictionary.ContainsKey(".cs"));

            Assert.IsFalse(dictionary.ContainsKey(".cpp"));

            // This shouldn't throw, just return false since .cpp isn't in the dictionary.
            Assert.IsFalse(dictionary.Remove(".cpp"));

            // Illegal path chars should always throw.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary["|file.bin"] = Editor.BinaryEditor;
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary.Add(">file.doc", Editor.RichTextEditor);
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary.ContainsKey("<file.txt");
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary.Remove("\t.tab");
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary.TryGetValue("\b.exe", out Editor dummy);
            });

            // empty strings are okay.
            dictionary[String.Empty] = Editor.None;

            // null is always forbidden.
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary.Add(null, Editor.None);
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary[null] = Editor.None;
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary.ContainsKey(null);
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary.Remove(null);
            });

            // even try-get should throw with null keys.
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary.TryGetValue(null, out Editor dummy);
            });
        }

        private enum Editor
        {
            None,
            TextEditor,
            ImageEditor,
            BinaryEditor,
            RichTextEditor,
            SourceCodeEditor
        }
    }
}
