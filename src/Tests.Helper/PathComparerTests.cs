// <copyright file="PathComparerTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved
//     Licensed under GNU Affero General Public License.
//     See LICENSE in project root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

using System;
using System.IO;
using Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Helper
{
    [TestClass]
    public class PathComparerTests : StringComparerTests
    {
        protected sealed override StringComparer DefaultComparer
        {
            get
            {
                return PathComparer.Default;
            }
        }

        public void DefaultBaseComparer()
        {
            // Ensure we are using our expected base comparer.
            Assert.AreEqual(
                PathComparer.Default.BaseComparer,
                StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void CustomBaseComparer()
        {
            // Ensure the base comparer is accepted by the constructor.
            var comparer = new PathComparer(
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
                new PathComparer(null);
            });
        }

        [TestMethod]
        public void PathCaseMatch()
        {
            AssertEquality(
                "app.exe",
                "app.exe");
        }

        [TestMethod]
        public void PathInvariantCaseMatch()
        {
            AssertEquality(
                "DATA.BIN", "data.bin");
        }

        [TestMethod]
        public void RelativeBackwardsMatch()
        {
            AssertEquality(
                "Forward/../Image.png",
                "Image.png");
        }

        [TestMethod]
        public void AltPathSeparatorMatch()
        {
            AssertEquality(
                @"dir\file.ext",
                @"dir/file.ext");
        }

        [TestMethod]
        public void AbsoluteBackwardsMatch()
        {
            AssertEquality(
                @"C:\path\to\..\file.doc",
                @"C:\path\file.doc");
        }

        [TestMethod]
        public void RelativeToAbsoluteMatch()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            AssertEquality(
                "app.exe",
                directory + @"\app.exe");
        }

        [TestMethod]
        public void CompareToNullString()
        {
            // TODO: Determine nehavior for null strings.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CompareToInvalidString()
        {
            // TODO: Determine behavior for ill-formed strings.
            Assert.Inconclusive();
        }

        [TestMethod]
        public void FileNameInequality()
        {
            AssertInequality(
                "program.exe",
                "game.exe");
        }

        [TestMethod]
        public void ExtensionInequality()
        {
            AssertInequality(
                "program.exe",
                "program.app");
        }

        [TestMethod]
        public void DirectoryInequality()
        {
            AssertInequality(
                "program.exe",
                @"path\program.exe");
        }
    }
}
