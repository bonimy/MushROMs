using System;
using System.IO;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class PathDictionaryTests
    {
        [TestMethod]
        public void PathDictionaryConstructors()
        {
            var dictionary = new PathDictionary<int>();
            Assert.AreEqual(dictionary.Comparer, PathComparer.DefaultComparer);

            var comparer = new PathComparer(StringComparer.Ordinal);
            dictionary = new PathDictionary<int>(comparer);
            Assert.AreEqual(dictionary.Comparer, comparer);

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary = new PathDictionary<int>((PathComparer)null);
            });

            // We should probably test other constructors, but if these three work, then
            // we're probably in good shape.
        }

        [TestMethod]
        public void PathDictionaryUsage()
        {
            // We test the path dictionary usage overall, since a lot of its functionality
            // is shared.

            var directory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
            var name = Path.GetFileName(directory);

            var appPath = "app.exe";
            var appEditor = new MockEditor(appPath);

            var dataPath = "data.bin";
            var dataEditor = new MockEditor(dataPath);

            var textPath = "file.txt";
            var textEditor = new MockEditor(textPath);

            var codePath = "code.cs";
            var codeEditor = new MockEditor(codePath);

            // Let's create a dictionary of editor file associations.
            var dictionary = new PathDictionary<MockEditor>
            {
                // Collect editors by path
                {appPath, appEditor },
                {dataPath, dataEditor },
                {textPath, textEditor },
                {codePath, codeEditor }
            };

            // Simple sanity test.
            var editor = dictionary[appPath];
            Assert.AreEqual(editor, appEditor);

            // Character casing should be ignored.
            editor = dictionary["DATA.BIN"];
            Assert.AreEqual(editor, dataEditor);

            // Full paths should also return appropriate editor.
            editor = dictionary[directory + "file.txt"];
            Assert.AreEqual(editor, textEditor);

            // Two file can have the same editor
            var docPath = @"C:\Document.doc";
            dictionary[docPath] = textEditor;
            Assert.AreEqual(dictionary[docPath], dictionary[textPath]);

            Assert.IsTrue(dictionary.ContainsKey(appPath));

            Assert.IsTrue(dictionary.ContainsKey(Path.GetFullPath(appPath)));

            Assert.IsTrue(dictionary.ContainsKey(Path.GetFullPath(appPath.ToLower())));

            Assert.IsTrue(dictionary.ContainsKey(appPath.ToUpper()));

            // This shouldn't throw, just return false since .cpp isn't in the dictionary.
            Assert.IsFalse(dictionary.Remove("fill.cpp"));

            // Illegal path chars should always throw.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary["|file.bin"] = new MockEditor("|file.bin");
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary.Add(">file.doc", new MockEditor(">file.doc"));
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary.ContainsKey("<file.txt");
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary.Remove("\t.tab");
            });

            Assert.ThrowsException<NotSupportedException>(() =>
            {
                dictionary.TryGetValue(@"C:\ f:exe", out MockEditor dummy);
            });

            // empty strings are not okay.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                dictionary[String.Empty] = new MockEditor(String.Empty);
            });

            // null is always forbidden.
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary.Add(null, new MockEditor(null));
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                dictionary[null] = new MockEditor(null);
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
                dictionary.TryGetValue(null, out MockEditor dummy);
            });
        }

        private class MockEditor
        {
            public string Path
            {
                get;
                private set;
            }

            public MockEditor(string path)
            {
                Path = path;
            }
        }
    }
}
