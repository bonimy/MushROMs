using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Helper
{
    public abstract class StringComparerTests
    {
        protected abstract StringComparer DefaultComparer
        {
            get;
        }

        protected void AssertEquality(
            string left,
            string right)
        {
            var comparer = DefaultComparer;
            var equality = comparer.Equals(left, right);
            Assert.IsTrue(equality);

            var comparison = comparer.Compare(left, right);
            Assert.AreEqual(comparison, 0);

            var leftHash = comparer.GetHashCode(left);
            var rightHash = comparer.GetHashCode(right);
            Assert.AreEqual(leftHash, rightHash);
        }

        protected void AssertInequality(
            string left,
            string right)
        {
            var comparer = DefaultComparer;
            var equality = comparer.Equals(left, right);
            Assert.IsFalse(equality);

            var comparison = comparer.Compare(left, right);
            Assert.AreNotEqual(comparison, 0);

            // Do not test hash codes. Collision is still possible.
        }
    }
}
