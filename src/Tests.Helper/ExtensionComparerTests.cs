// <copyright file="ExtensionComparerTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class ExtensionComparerTests : StringComparerTests
    {
        protected sealed override StringComparer DefaultComparer
        {
            get
            {
                return ExtensionComparer.Default;
            }
        }

        public void DefaultBaseComparer()
        {
            // Ensure we are using our expected base comparer.
            Assert.AreEqual(
                ExtensionComparer.Default.BaseComparer,
                StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void CustomBaseComparer()
        {
            // Ensure the base comparer is accepted by the constructor.
            var comparer = new ExtensionComparer(
                StringComparer.Ordinal);

            Assert.AreEqual(
                comparer.BaseComparer,
                StringComparer.Ordinal);
        }

        [TestMethod]
        public void NullBaseComparer()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new ExtensionComparer(null);
            });
        }

        [TestMethod]
        public void ExtCaseMatch()
        {
            AssertEquality(
                ".exe",
                ".exe");
        }

        [TestMethod]
        public void ExtInvariantCaseMatch()
        {
            AssertEquality(
                ".BIN",
                ".bin");
        }

        [TestMethod]
        public void ExtToNameCaseMatch()
        {
            AssertEquality(
                ".png",
                "Image.png");
        }

        [TestMethod]
        public void NameToNameInvariantCaseMatch()
        {
            AssertEquality(
                "Notes.Txt",
                "README.TXT");
        }

        [TestMethod]
        public void FullPathInvariantCaseMatch()
        {
            AssertEquality(
                @"C:\path\to\..\file.doc",
                "Document.doc");
        }

        [TestMethod]
        public void NoExtension()
        {
            AssertEquality(
                "No extension",
                "Any text here is fine without a period");
        }

        [TestMethod]
        public void NoExtensionToEmptyString()
        {
            AssertEquality(
                "No extension",
                String.Empty);
        }

        [TestMethod]
        public void CompareToNullString()
        {
            // TODO: Determine behavior for null strings.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CompareToInvalidString()
        {
            // TODO: Determine behavior for ill-formed strings.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TrivialInequality()
        {
            AssertInequality(
                ".exe",
                ".app");
        }

        [TestMethod]
        public void ExtensionVsNameOfExtension()
        {
            AssertInequality(
                ".bin",
                "bin");
        }

        [TestMethod]
        public void NameWithDifferentExtension()
        {
            AssertInequality(
                "Image.png",
                "Image.jpg");
        }
    }
}
