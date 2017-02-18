//Written by Ellen Brigance, February 2017
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Formulas;

namespace SS
{
    /// <summary>
    /// A tester class for the Spreadsheet and all of its functions.
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /***************** GetNamesOfAllNonemptyCells() Tests ************************/
       
        /// <summary>
        /// Try to get names when all cells are empty.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells1()
        {
            Spreadsheet s = new Spreadsheet();
            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(list.Count == 0);
        }

        /// <summary>
        /// Get names when we have one cell.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 4);
            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(list.Count == 1);
        }


        /// <summary>
        /// Get names from several cells.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells3()
        {
            Spreadsheet s = new Spreadsheet();

            List<string> checkList = new List<string>();
            checkList.Add("A1");
            checkList.Add("A2");
            checkList.Add("A3");
            checkList.Add("A4");
            checkList.Add("A5");
            checkList.Add("A6");

            s.SetCellContents("A1", 4);
            s.SetCellContents("A2", 4);
            s.SetCellContents("A3", 4);
            s.SetCellContents("A4", "s");
            s.SetCellContents("A5", "s");
            s.SetCellContents("A6", "s");
            s.SetCellContents("A4", new Formula("s1"));
            s.SetCellContents("A5", new Formula("s1"));
            s.SetCellContents("A6", new Formula("s1"));
            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());
            foreach(string t in checkList)
            {
                Assert.IsTrue(list.Contains(t));
            }
            Assert.IsTrue(list.Count == 6);
        }


        /// <summary>
        /// Add cell contents, empty all cell contents, 
        /// should have no nonempty cells.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells4()
        {
            Spreadsheet s = new Spreadsheet();
            
            //Add stuff.
            s.SetCellContents("A1", 4);
            s.SetCellContents("A2", 4);
            s.SetCellContents("A3", 4);
            s.SetCellContents("A4", "s");
            s.SetCellContents("A5", "s");
            s.SetCellContents("A6", "s");
            s.SetCellContents("A4", new Formula("s1"));
            s.SetCellContents("A5", new Formula("s1"));
            s.SetCellContents("A6", new Formula("s1"));

            //Remove all of it.
            s.SetCellContents("A1", "");
            s.SetCellContents("A2", "");
            s.SetCellContents("A3", "");
            s.SetCellContents("A4", "");
            s.SetCellContents("A5", "");
            s.SetCellContents("A6", " ");
            s.SetCellContents("A4", "    ");
            s.SetCellContents("A5", "     ");
            s.SetCellContents("A6", "  ");

            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());

            Assert.IsTrue(list.Count == 0);
            
        }

        /// <summary>
        /// Add cell contents, reapeat the add. We should only report back 1 nonempty.
        /// should have no nonempty cells.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells5()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 4);
            s.SetCellContents("A1", 5);
            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(list.Count == 1);
        }

        /// <summary>
        /// Add cell contents, get rid of a few. Only report back 
        /// active nonempty cells.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells6()
        {
            Spreadsheet s = new Spreadsheet();

            //Add stuff.
            s.SetCellContents("A1", 4);
            s.SetCellContents("A2", 4);
            s.SetCellContents("A3", 4);
            s.SetCellContents("A4", "s");
            s.SetCellContents("A5", "s");
            s.SetCellContents("A6", "s");
            s.SetCellContents("A7", new Formula("s1"));
            s.SetCellContents("A8", new Formula("s1"));
            s.SetCellContents("A9", new Formula("s1"));

            //Remove a few.
            s.SetCellContents("A1", "");
            s.SetCellContents("A2", "");
            s.SetCellContents("A3", "");
            s.SetCellContents("A4", "");
            s.SetCellContents("A5", "");
            s.SetCellContents("A6", " ");

            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());

            Assert.IsTrue(list.Count == 3);
        }

        /********************* Tests for GetCellContents(string) ********************/

        /// <summary>
        /// invalid name throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContent1()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("2INVALID");       
        }


        /// <summary>
        /// Null name throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContent2()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);

        }

        /// <summary>
        /// Try to get cell contents that are a double.
        /// </summary>
        [TestMethod]
        public void GetCellContent3()
        {
            Spreadsheet s = new Spreadsheet();
            double d = 145;
            s.SetCellContents("A1", d);
     
            Assert.AreEqual(d, (double) s.GetCellContents("A1"));
        }
        /// <summary>
        /// Try to get cell contents that are a string.
        /// </summary>
        [TestMethod]
        public void GetCellContent4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "d");

            Assert.AreEqual("d", (string)s.GetCellContents("A1"));
        }
        /// <summary>
        /// Try to get the contents of an empty cell.
        /// </summary>
        [TestMethod]
        public void GetCellContent5()
        {
            Spreadsheet s = new Spreadsheet();

            Assert.AreEqual("", (string)s.GetCellContents("A1"));
        }

        /// <summary>
        /// Try to get the contents of a null string.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCellContent6()
        {
            Spreadsheet s = new Spreadsheet();
            string n = null;
            s.SetCellContents("a1", n);

            Assert.AreEqual("", (string)s.GetCellContents("A1"));
        }

        /************* EXTRA TESTS FOR SetCellContents *********************/

        /// <summary>
        /// Add a double an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("", 4);
        }

        /// <summary>
        /// Add a double to a null cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, 4);
        }

        /// <summary>
        /// Add a string to an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("", "4");
        }

        /// <summary>
        /// Add a string to an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, "4");
        }

        /// <summary>
        /// Add a formula to a null cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents5()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, new Formula("4"));
        }


        /// <summary>
        /// Add a string to an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents6()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("565", new Formula("4"));
        }


        /// <summary>
        /// Set cell contents, change the contents, check the cell.
        /// </summary>
        [TestMethod]
        public void SetCellContents7()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("4"));
            s.SetCellContents("A1", "4");

            Assert.AreEqual("4", (string)s.GetCellContents("A1"));

        }

        //Checks for circular dependencies.

        /// <summary>
        /// Direct circular dependency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents8()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A1 + A2"));
        }


        /// <summary>
        /// Indirect circular dependency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents9()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2"));
            s.SetCellContents("A2", new Formula("A1"));
        }

        /// <summary>
        /// Circular dependency much later.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents10()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2 + A3"));
            s.SetCellContents("A3", new Formula("A4 + A5"));
            s.SetCellContents("A5", new Formula("A6 + A7"));
            s.SetCellContents("A7", new Formula("A1 + A1"));

        }

    }
}
