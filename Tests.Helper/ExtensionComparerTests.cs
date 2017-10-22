using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class ExtensionComparerTests
    {
        [TestMethod]
        public void ConstructorTests()
        {
            // Default constructor should never throw.
            var comparer = ExtensionComparer.DefaultComparer;

            // Ensure we are using our expected base comparer.
            Assert.AreEqual(comparer.BaseComparer, StringComparer.OrdinalIgnoreCase);

            // Ensure the base comparer is accepted by the constructor.
            comparer = new ExtensionComparer(StringComparer.Ordinal);
            Assert.AreEqual(comparer.BaseComparer, StringComparer.Ordinal);

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                comparer = new ExtensionComparer(null);
            });
        }

        [TestMethod]
        public void EqualityTests()
        {
            // Everything in this list is expected to compare to equal
            // with the default extension comparer.
            var compareAsEqual = new Parameters[]
            {
                new Parameters(".exe", ".exe"),
                new Parameters(".BIN", ".bin"),
                new Parameters(".Txt", ".TXT"),
                new Parameters(".png", "Image.png"),
                new Parameters(@"C:\path\to\..\file.doc", "Document.doc"),
                new Parameters("No extension", "Any text here is fine without a period"),
                new Parameters("No extension", String.Empty)
            };

            var comparer = ExtensionComparer.DefaultComparer;

            foreach (var parameter in compareAsEqual)
            {
                var left = parameter.Left;
                var right = parameter.Right;

                var comparison = comparer.Compare(left, right);

                Assert.AreEqual(0, comparison, SR.GetString(
                    "Comparison of \"{0}\" and \"{1}\" returned {2} (expected 0).",
                    left, right, comparison)
                    );

                var equality = comparer.Equals(left, right);

                Assert.IsTrue(equality, SR.GetString(
                    "Extension equality of \"{0}\" and \"{1}\" returned false (expected true).",
                    left, right)
                    );

                // Equal parameters should have equal hashes.
                var leftHash = comparer.GetHashCode(left);
                var rightHash = comparer.GetHashCode(right);

                Assert.AreEqual(leftHash, rightHash, SR.GetString(
                    "Hash code of extensions of \"{0}\" and \"{1}\" are unequal (expected equal).",
                    left, right)
                    );
            }
        }

        [TestMethod]
        public void InequalityTests()
        {
            // Everything in this list is expected to compare to unequal
            // with the default extension comparer.
            var compareAsUnequal = new Parameters[]
            {
                new Parameters(".exe", ".app"),
                new Parameters(".bin", "bin"),
                new Parameters(".txt", ".ini"),
                new Parameters(".jpg", "Image.png"),
                new Parameters(@"C:\path\to\..\file.doc", "Document.docx")
            };

            var comparer = ExtensionComparer.DefaultComparer;

            foreach (var parameter in compareAsUnequal)
            {
                var left = parameter.Left;
                var right = parameter.Right;

                var comparison = comparer.Compare(left, right);

                Assert.AreNotEqual(0, comparison, SR.GetString(
                    "Extension comparison of \"{0}\" and \"{1}\" returned 0 (expected nonzero).",
                    left, right)
                    );

                var equality = comparer.Equals(left, right);

                Assert.IsFalse(equality, SR.GetString(
                    "Extension equality of \"{0}\" and \"{1}\" returned true (expected false).",
                    left, right)
                    );

                // Note we do not compare hash codes. Unequal extensions do not
                // guarantee unequal hash codes.
            }
        }

        [TestMethod]
        public void TestCompareExceptions()
        {
            var comparer = ExtensionComparer.DefaultComparer;

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

            // Empty strings are okay.
            comparer.Compare(String.Empty, String.Empty);
            comparer.Equals(String.Empty, ".exe");
            comparer.GetHashCode(String.Empty);

            // null strings are forbidden
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                comparer.Compare(null, ".exe");
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                comparer.Equals(null, ".bin");
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
