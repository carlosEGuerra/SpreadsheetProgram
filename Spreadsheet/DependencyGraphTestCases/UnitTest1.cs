using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Dependencies
{
    [TestClass]
    public class DependencyGraphTests
    {
        //*************************** TESTS FOR EMPTY LISTS *********************************

        /// <summary>
        /// Creates a new DependencyGraph, makes sure it starts empty.
        /// </summary>
        [TestMethod()]
        public void EmptyTest1()
        {
            DependencyGraph dg = new DependencyGraph();
            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// Create graph, add something, remove something, check size.
        /// </summary>
        [TestMethod()]
        public void EmptyTest2()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.RemoveDependency("a", "b");
            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// If we replace a's dependency on b, we should have 0 dependencies.
        /// </summary>
        [TestMethod()]
        public void EmptyTest3()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.ReplaceDependents("a", new HashSet<string>());
            Assert.AreEqual(0, dg.Size);
        }

        /// <summary>
        /// If we replace b's dependees with an empty list, we should have 0 dependencies.
        /// </summary>
        [TestMethod()]
        public void EmptyTest4()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.ReplaceDependees("b", new HashSet<string>());
            Assert.AreEqual(0, dg.Size);
        }

        //***************************** TESTS ON Size **********************************

        /// <summary>
        /// Add some dependencies, check size, remove all, check size.
        /// </summary>
        [TestMethod()]
        public void SizeTest1()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 1000; i++)
            {
                dg.AddDependency(i.ToString(), "A");
            }

            Assert.IsTrue(dg.Size == 1000);

            for (int i = 0; i < 1000; i++)
            {
                dg.RemoveDependency(i.ToString(), "A");
            }

            Assert.IsTrue(dg.Size == 0);

        }


        /// <summary>
        /// Add some dependencies, check size, remove dependencies that don't exist.
        /// Size should be 10.
        /// </summary>
        [TestMethod()]
        public void SizeTest2()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 10000; i++)
            {
                dg.AddDependency(i.ToString(), "A");
            }

            Assert.IsTrue(dg.Size == 10000);

            for (int i = 0; i < 10000; i++)
            {
                dg.RemoveDependency(i.ToString(), "B");
            }

            Assert.IsTrue(dg.Size == 10000);

        }

        /// <summary>
        /// Add different dependencies, add one we already have, make sure numbers are right.
        /// </summary>
        [TestMethod()]
        public void SizeTest3()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 10000; i++)
            {
                dg.AddDependency(i.ToString(), "A");
                dg.AddDependency(i.ToString(), "B");
                dg.AddDependency(i.ToString(), "C");
                dg.AddDependency(i.ToString(), "A");//redundant, shouldn't add.
            }

            Assert.IsTrue(dg.Size == 30000);
        }

        //*************************** TESTS ON HasDependents ***************************

        /// <summary>
        /// Test a non-existent item.
        /// </summary>
        [TestMethod()]
        public void HasDependents1()
        {
            DependencyGraph dg = new DependencyGraph();
            Assert.IsFalse(dg.HasDependents("a"));
        }

        /// <summary>
        /// Add dependents, check for them.
        /// </summary>
        [TestMethod()]
        public void HasDependents2()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("a", "c");
            Assert.IsTrue(dg.HasDependents("a"));
        }

        /// <summary>
        /// Add dependents, remove dependents, make sure they aren't there.
        /// </summary>
        [TestMethod()]
        public void HasDependents3()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("a", "c");
            dg.RemoveDependency("a", "c");
            Assert.IsTrue(dg.HasDependents("a"));
            dg.RemoveDependency("a", "b");
            Assert.IsFalse(dg.HasDependents("a"));
        }

        //*************************** TESTS ON HasDependees ****************************

        /// <summary>
        /// Test a non-existent item.
        /// </summary>
        [TestMethod()]
        public void HasDependees0()
        {
            DependencyGraph dg = new DependencyGraph();
            string a = null;
            Assert.IsFalse(dg.HasDependees(a));
        }

        /// <summary>
        /// Test a non-existent item.
        /// </summary>
        [TestMethod()]
        public void HasDependees1()
        {
            DependencyGraph dg = new DependencyGraph();
            Assert.IsFalse(dg.HasDependees("a"));
        }

        /// <summary>
        /// Add dependents, check for them.
        /// </summary>
        [TestMethod()]
        public void HasDependees2()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "b");
            dg.AddDependency("c", "b");
            Assert.IsTrue(dg.HasDependees("b"));
        }

        /// <summary>
        /// Add dependents, remove dependents, make sure they aren't there.
        /// </summary>
        [TestMethod()]
        public void HasDependees3()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("a", "c");
            dg.AddDependency("b", "c");
            dg.RemoveDependency("a", "c");
            Assert.IsTrue(dg.HasDependees("c"));
            dg.RemoveDependency("b", "c");
            Assert.IsFalse(dg.HasDependees("c"));
        }

        //*************************** TESTS ON GetDependents ***************************


        /// <summary>
        /// Test a non-existent item.
        /// </summary>
        [TestMethod()]
        public void HasDependents0()
        {
            DependencyGraph dg = new DependencyGraph();
            string a = null;
            Assert.IsFalse(dg.HasDependents(a));
        }

        /// <summary>
        /// Try to get dependents of an empty list.
        /// </summary>
        [TestMethod()]
        public void GetDependents1()
        {
            DependencyGraph dg = new DependencyGraph();
            IEnumerator<string> empty = dg.GetDependents("a").GetEnumerator();
            Assert.IsFalse(empty.MoveNext());
        }

        /// <summary>
        /// Add dependents, make sure each one we added is in the list.
        /// </summary>
        [TestMethod()]
        public void GetDependents2()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 10; i++)
            {
                dg.AddDependency("a", i.ToString());
            }
            IEnumerator<string> list = dg.GetDependents("a").GetEnumerator();
            int j = 0;
            while (list.MoveNext())
            {
                Assert.IsTrue(list.Current == j.ToString());
                j++;
            }

        }

        /// <summary>
        /// Add dependents, remove half, make sure we only see the ones we have left.
        /// </summary>
        [TestMethod()]
        public void GetDependents3()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 100; i++)
            {
                dg.AddDependency("a", i.ToString());
            }

            for (int i = 50; i < 100; i++)
            {
                dg.RemoveDependency("a", i.ToString());
            }

            IEnumerator<string> list = dg.GetDependents("a").GetEnumerator();

            for (int j = 0; j < 50; j++)
            {
                list.MoveNext();
                Assert.IsTrue(list.Current == j.ToString());
            }
        }

        //*************************** TESTS ON GetDependees ****************************
        /// <summary>
        /// Try to get dependents of an empty list.
        /// </summary>
        [TestMethod()]
        public void GetDependees1()
        {
            DependencyGraph dg = new DependencyGraph();
            IEnumerator<string> empty = dg.GetDependees("a").GetEnumerator();
            Assert.IsFalse(empty.MoveNext());
        }

        /// <summary>
        /// Add dependents, make sure each one we added is in the list.
        /// </summary>
        [TestMethod()]
        public void GetDependees2()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 10; i++)
            {
                dg.AddDependency(i.ToString(), "a");
            }
            IEnumerator<string> list = dg.GetDependees("a").GetEnumerator();
            int j = 0;
            while (list.MoveNext())
            {
                Assert.IsTrue(list.Current == j.ToString());
                j++;
            }

        }

        /// <summary>
        /// Add dependents, remove half, make sure we only see the ones we have left.
        /// </summary>
        [TestMethod()]
        public void GetDependees3()
        {
            DependencyGraph dg = new DependencyGraph();
            for (int i = 0; i < 100; i++)
            {
                dg.AddDependency(i.ToString(), "a");
            }

            for (int i = 50; i < 100; i++)
            {
                dg.RemoveDependency(i.ToString(), "a");
            }

            IEnumerator<string> list = dg.GetDependees("a").GetEnumerator();

            for (int j = 0; j < 50; j++)
            {
                list.MoveNext();
                Assert.IsTrue(list.Current == j.ToString());
            }
        }

        //*************************** TESTS ON AddDependency ***************************

        /// <summary>
        /// Try to pass in null strings.
        /// </summary>
        [TestMethod()]
        public void AddDependency1()
        {
            DependencyGraph dg = new DependencyGraph();
            string s = null;
            string t = null;
            dg.AddDependency(s, t);
            Assert.IsTrue(dg.Size == 0);
        }

        /// <summary>
        /// Add a dependency that already exists. Make sure the set size doesn't increase.
        /// </summary>
        [TestMethod()]
        public void AddDependency2()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("s", "t");
            dg.AddDependency("s", "t");
            dg.AddDependency("s", "t");
            dg.AddDependency("s", "t");
            Assert.IsTrue(dg.Size == 1);
        }

        //************************** TESTS ON RemoveDependency *************************

        /// <summary>
        /// Try to pass in null strings.
        /// </summary>
        [TestMethod()]
        public void RemoveDependency1()
        {
            DependencyGraph dg = new DependencyGraph();
            string s = null;
            string t = null;
            dg.RemoveDependency(s, t);
            Assert.IsTrue(dg.Size == 0);
        }

        /// <summary>
        /// Try to remove a dependency that does not exist.
        /// </summary>
        [TestMethod()]
        public void RemoveDependency2()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.RemoveDependency("s", "t");
            Assert.IsTrue(dg.Size == 0);
        }
        //************************* TESTS ON ReplaceDependents *************************

        /// <summary>
        /// Try to test a null list and a null string.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependents1()
        {
            DependencyGraph dg = new DependencyGraph();
            string s = null;
            HashSet<string> empty = null;
            dg.ReplaceDependents(s, empty);
            Assert.IsTrue(dg.Size == 0);
        }

        /// <summary>
        /// Try to replace a key that does not exist.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependents2()
        {
            DependencyGraph dg = new DependencyGraph();
            HashSet<string> list = new HashSet<string>();
            dg.ReplaceDependents("s", list);
            Assert.IsTrue(dg.Size == 0);
        }

        /// <summary>
        /// Give an item several dependents. Replace them with a different number of dependents.
        /// Check the count.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependents3()
        {
            DependencyGraph dg = new DependencyGraph();
            HashSet<string> list = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                dg.AddDependency("s", i.ToString());
            }
            for (int i = 0; i < 10; i++)
            {
                list.Add(i.ToString());
            }
            dg.ReplaceDependents("s", list);

            Assert.IsTrue(dg.Size == 10);
        }

        /// <summary>
        /// Populate a list with nulls. Make sure we don't try to add them.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependents4()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("s", "b");
            HashSet<string> list = new HashSet<string>();
            for(int i = 0; i < 10; i++)
            {
                list.Add(null);
            }
            dg.ReplaceDependents("s", list);
            Assert.IsTrue(dg.Size == 0);
        }

        //************************** TESTS ON ReplaceDependees *************************

        /// <summary>
        /// Try to test a null list and a null string.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependees1()
        {
            DependencyGraph dg = new DependencyGraph();
            string s = null;
            HashSet<string> empty = null;
            dg.ReplaceDependees(s, empty);
            Assert.IsTrue(dg.Size == 0);
        }

        /// <summary>
        /// Try to replace a key that does not exist.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependees2()
        {
            DependencyGraph dg = new DependencyGraph();
            HashSet<string> list = new HashSet<string>();
            dg.ReplaceDependees("s", list);
            Assert.IsTrue(dg.Size == 0);
        }

        /// <summary>
        /// Give an item several dependents. Replace them with a different number of dependents.
        /// Check the count.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependees3()
        {
            DependencyGraph dg = new DependencyGraph();
            HashSet<string> list = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                dg.AddDependency(i.ToString(), "t");
            }
            for (int i = 0; i < 5; i++)
            {
                list.Add(i.ToString());
            }

            dg.ReplaceDependees("t", list);

            Assert.IsTrue(dg.Size == 5);
        }

        /// <summary>
        /// Populate a list with nulls. Make sure we don't try to add them.
        /// </summary>
        [TestMethod()]
        public void ReplaceDependees4()
        {
            DependencyGraph dg = new DependencyGraph();
            dg.AddDependency("b", "s");
            HashSet<string> list = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(null);
            }
            dg.ReplaceDependees("s", list);
            Assert.IsTrue(dg.Size == 0);
        }
    }
}
