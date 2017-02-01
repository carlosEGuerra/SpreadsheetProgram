using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBlankDependency()
        {
            DependencyGraph dg = new DependencyGraph();
            Assert.AreEqual(0, dg.Size);
            //Assert.AreEqual(false, dg.HasDependees("s"));
            //Assert.AreEqual(false, dg.HasDependents("s"));
        }

        [TestMethod]
        public void TestWith1Dependent()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("s", "t");
            Assert.AreEqual(1, dg.Size);
            Assert.AreEqual("t", dg.GetDependents("s"));

        }
    }
}
