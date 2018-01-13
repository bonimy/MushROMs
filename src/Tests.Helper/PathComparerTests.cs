using System;
using System.IO;
<<<<<<< HEAD
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helper;
=======
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
>>>>>>> refs/remotes/origin/master

namespace Tests.Helper
{
    [TestClass]
    public class PathComparerTests
    {
        [TestMethod]
        public void PathComparerConstructors()
        {
            // Default constructor should never throw.
            var comparer = PathComparer.DefaultComparer;

<<<<<<< HEAD
            // Ensure we are using our expected base compater.
=======
            // Ensure we are using our expected base comparer.
>>>>>>> refs/remotes/origin/master
            Assert.AreEqual(comparer.BaseComparer, StringComparer.OrdinalIgnoreCase);

            // Ensure the base comparer is accepted by the constructor.
            comparer = new PathComparer(StringComparer.Ordinal);
            Assert.AreEqual(comparer.BaseComparer, StringComparer.Ordinal);

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                comparer = new PathComparer(null);
            });
        }

        [TestMethod]
        public void PathComparerEquality()
        {
            // I don't like these being in the unit tests, but they are necessary.
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var name = Path.GetFileName(directory);

            // Everything in this list is expected to compare to equal
            // with the default path comparer.
            var compareAsEqual = new Parameters[]
            {
                new Parameters("app.exe", "app.exe"),
                new Parameters("DATA.BIN", "data.bin"),
                new Parameters("File.Txt", "FILE.TXT"),
                new Parameters("Forward/../Image.png", "Image.png"),
                new Parameters(@"Dir/Source.cs", @"Dir/Dummy/../Source.cs"),
                new Parameters(@"C:\path\to\..\file.doc", @"C:\path\file.doc"),
                new Parameters(@"../" + name + "/Document.doc", "Document.doc"),
                new Parameters("app.exe", directory + "/app.exe")
            };

            var comparer = PathComparer.DefaultComparer;

            foreach (var parameter in compareAsEqual)
            {
                var left = parameter.Left;
                var right = parameter.Right;

                var comparison = comparer.Compare(left, right);

                Assert.AreEqual(0, comparison, SR.GetString(
<<<<<<< HEAD
                    "Path comparision of \"{0}\" and \"{1}\" returned {2} (expected 0).",
=======
                    "Path comparison of \"{0}\" and \"{1}\" returned {2} (expected 0).",
>>>>>>> refs/remotes/origin/master
                    left, right, comparison)
                    );

                var equality = comparer.Equals(left, right);

                Assert.IsTrue(equality, SR.GetString(
                    "Path equality of \"{0}\" and \"{1}\" returned false (expected true).",
                    left, right)
                    );

                // Equal parameters should have equal hashes.
                var leftHash = comparer.GetHashCode(left);
                var rightHash = comparer.GetHashCode(right);

                Assert.AreEqual(leftHash, rightHash, SR.GetString(
                    "Hash code of paths of \"{0}\" and \"{1}\" are unequal (expected equal).",
                    left, right)
                    );
            }
        }

        [TestMethod]
        public void PathComparerInequality()
        {
            var comparer = PathComparer.DefaultComparer;
            var directory = AppDomain.CurrentDomain.BaseDirectory;

            // Illegal path chars should always throw.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                comparer.Compare(">file.exe", ".exe");
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                comparer.Equals("|pipe.bin", "pipe.bin");
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                comparer.GetHashCode("a\t.tab");
            });

            Assert.ThrowsException<NotSupportedException>(() =>
            {
                comparer.GetHashCode(@"C:\f Z:\l");
            });

            // Empty strings are not okay.
            Assert.ThrowsException<ArgumentException>(() =>
            {
                comparer.Compare(String.Empty, "path.exe");
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                comparer.Equals(String.Empty, directory);
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                comparer.GetHashCode(String.Empty);
            });

            // null strings are forbidden
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                comparer.Compare(null, "path.exe");
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                comparer.Equals(null, directory);
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                comparer.GetHashCode(null);
            });
        }

        private struct Parameters
        {
            public string Left
            {
                get;
                private set;
            }
<<<<<<< HEAD
=======

>>>>>>> refs/remotes/origin/master
            public string Right
            {
                get;
                private set;
            }

            public Parameters(string left, string right)
            {
                Left = left;
                Right = right;
            }
        }
    }
}
