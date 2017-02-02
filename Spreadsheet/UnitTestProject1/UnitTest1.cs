using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;
using System.Text;

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
            Assert.AreEqual(false, dg.HasDependees("s"));
            Assert.AreEqual(false, dg.HasDependents("s"));
        }

        [TestMethod]
        public void TestWith1Dependent()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("s", "t");
            Assert.AreEqual(1, dg.Size);
            Assert.AreEqual(true, dg.HasDependees("t"));
            Assert.AreEqual(false, dg.HasDependents("t"));
            Assert.AreEqual(false, dg.HasDependees("s"));
            Assert.AreEqual(true, dg.HasDependents("s"));
        }


        [TestMethod]
        public void TestReplaceWith1Dependee()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("s", "t");
            dg.AddDependency("s", "d");
            Assert.AreEqual(true, dg.HasDependents("s"));
            Assert.AreEqual(2, dg.Size);
        }


        [TestMethod]
        public void TestAddWith100000Dependencies()
        {
            DependencyGraph dg = new DependencyGraph();
            for(int i = 0; i < 100000; i++)
            {
                dg.AddDependency(i.ToString(), i.ToString());
            }
            Assert.AreEqual(100000, dg.Size);
            for (int i = 0; i < 100000; i++)
            {
                dg.RemoveDependency(i.ToString(), i.ToString());
            }
            Assert.AreEqual(0, dg.Size);
        }

        [TestMethod]
        public void TestReplace()
        {
            DependencyGraph dg = new DependencyGraph();
            for(int i  = 0; i < 10; i++)
            {
                dg.AddDependency(i.ToString(), (i + 1).ToString());
            }
            Assert.AreEqual(10, dg.Size);
            Assert.AreEqual(true, dg.HasDependents("0"));
            dg.ReplaceDependees("0", new HashSet<string>() { "a" });
            Assert.AreEqual(true, dg.HasDependents("0"));
            dg.ReplaceDependents("a", new HashSet<string>() { "b" });
            Assert.AreEqual(true, dg.HasDependents("0"));
            Assert.AreEqual(false, dg.HasDependents("b"));
            dg.ReplaceDependents("0", new HashSet<string>() { "a", "b" });
            HashSet<string> hashTest = new HashSet<string>() { "a", "b" };
            Assert.IsTrue(hashTest.SetEquals(dg.GetDependents("0")));
        }

        [TestMethod]
        public void TestGetDependees()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 10; i++)
            {
                dg.AddDependency(i.ToString(), (i + 1).ToString());
            }
            HashSet<string> hashTest = new HashSet<string>() { "1" };
            HashSet<string> hashTest2 = new HashSet<string>() { "9" };
            Assert.IsTrue(hashTest.SetEquals(dg.GetDependees("2")));
            Assert.IsTrue(hashTest2.SetEquals(dg.GetDependees("10")));
        }

        [TestMethod]
        public void TestReplaceDependees()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 10; i++)
            {
                dg.AddDependency(i.ToString(), (i + 1).ToString());
            }
            HashSet<string> hashTest = new HashSet<string>() { "1", "2", "3" };
            dg.ReplaceDependees("5", new HashSet<string>() { "1", "2", "3" });
            Assert.IsTrue(hashTest.SetEquals(dg.GetDependees("5")));
        }

        [TestMethod]
        public void TestAddDependencyWithPreExistingKey()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("s", "t");
            dg.AddDependency("d", "f");
            dg.AddDependency("s", "f");
            dg.AddDependency("d", "t");
            HashSet<string> dependentsForS = new HashSet<string>() { "t", "f" };
            Assert.IsTrue(dependentsForS.SetEquals(dg.GetDependents("s")));
        }

        [TestMethod]
        public void TestExtremeCase()
        {
            DependencyGraph dg = new DependencyGraph();
            Random rand = new Random();
            StringBuilder s = new StringBuilder();
            for(int i = 0; i < 1e6; i++)
            {
                dg.AddDependency(i.ToString(), s.ToString());
            }
            Assert.AreEqual(1e6, dg.Size);
        }
    }
}
