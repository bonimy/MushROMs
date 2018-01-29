// <copyright file="SelectionTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MushROMs;

namespace Tests.Helper
{
    [TestClass]
    public class SelectionTests
    {
        [TestMethod]
        public void TestSelections()
        {
            // Test the empty selection
            var selection = Selection1D.Empty;
            Assert.AreEqual(selection.Count, 0);
            TestSelection(selection);
            Assert.IsTrue(selection.IsEmpty);

            for (var i = -0x1000; i <= 0x1000; i++)
            {
                Assert.IsFalse(selection.Contains(i));
            }

            // Test the single selection.
            selection = new SingleSelection1D(0x100);
            Assert.AreEqual(selection.Count, 1);
            TestSelection(selection);
            Assert.IsFalse(selection.IsEmpty);
            Assert.IsTrue(selection.Contains(0x100));
            for (var i = -0x1000; i <= 0x1000; i++)
            {
                if (i == 0x100)
                {
                    continue;
                }

                Assert.IsFalse(selection.Contains(i));
            }

            // Test the line selection.
            selection = new LineSelection1D(0x80, 0x80);
            Assert.AreEqual(selection.Count, 0x80);
            TestSelection(selection);
            Assert.IsFalse(selection.IsEmpty);
            for (var i = -0x1000; i <= 0x1000; i++)
            {
                if (i >= 0x80 && i < 0x100)
                {
                    Assert.IsTrue(selection.Contains(i));
                }
                else
                {
                    Assert.IsFalse(selection.Contains(i));
                }
            }

            selection = new GateSelection1D(
                selection,
                new SingleSelection1D(0x10),
                (x, y) => x | y);
            Assert.AreEqual(selection.Count, 0x81);
            TestSelection(selection);
            Assert.IsFalse(selection.IsEmpty);
            for (var i = -0x1000; i <= 0x1000; i++)
            {
                if ((i >= 0x80 && i < 0x100) || i == 0x10)
                {
                    Assert.IsTrue(selection.Contains(i));
                }
                else
                {
                    Assert.IsFalse(selection.Contains(i));
                }
            }
        }

        private void TestSelection(Selection1D selection)
        {
            // Add each selected index to the list
            var list = new List<int>();
            foreach (var index in selection)
            {
                list.Add(index);
            }

            // Assert the size of the list is the same as the size of the selection.
            Assert.AreEqual(list.Count, selection.Count);

            // Assert every item in the list is added to the selection.
            for (var i = list.Count; --i >= 0;)
            {
                Assert.IsTrue(selection.Contains(list[i]));
            }
        }
    }
}
