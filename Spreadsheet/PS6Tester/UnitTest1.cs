//Written by Ellen Brigance, February 2017
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Formulas;
using SS;
using System.IO;
using System.Text.RegularExpressions;

namespace PS6Tester
{
    /// <summary>
    /// A tester class for the Spreadsheet and all of its functions.
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /******************* GetNamesOfAllNonemptyCells() Tests ************************/

        /// <summary>
        /// Try to get names when all cells are empty.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(list.Count == 0);
        }

        /// <summary>
        /// Get names when we have one cell.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "4");
            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(list.Count == 1);
        }


        /// <summary>
        /// Get names from several cells.
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells3()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            List<string> checkList = new List<string>();
            checkList.Add("A1");
            checkList.Add("A2");
            checkList.Add("A3");
            checkList.Add("A4");
            checkList.Add("A5");
            checkList.Add("A6");

            s.SetContentsOfCell("A1", "4");
            s.SetContentsOfCell("A2", "4");
            s.SetContentsOfCell("A3", "4");
            s.SetContentsOfCell("A4", "s");
            s.SetContentsOfCell("A5", "s");
            s.SetContentsOfCell("A6", "s");
            s.SetContentsOfCell("A4", "=" + new Formula("s1").ToString());
            s.SetContentsOfCell("A5", "=" + new Formula("s1").ToString());
            s.SetContentsOfCell("A6", "=" + new Formula("s1").ToString());
            List<string> list = new List<string>(s.GetNamesOfAllNonemptyCells());
            foreach (string t in checkList)
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
            AbstractSpreadsheet s = new Spreadsheet();

            //Add stuff.
            s.SetContentsOfCell("A1", "4");
            s.SetContentsOfCell("A2", "4");
            s.SetContentsOfCell("A3", "4");
            s.SetContentsOfCell("A4", "s");
            s.SetContentsOfCell("A5", "s");
            s.SetContentsOfCell("A6", "s");
            s.SetContentsOfCell("A7", "=" + new Formula("s1").ToString());
            s.SetContentsOfCell("A8", "=" + new Formula("s1").ToString());
            s.SetContentsOfCell("A9", "=" + new Formula("s1").ToString());

            //Remove all of it.
            s.SetContentsOfCell("A1", "");
            s.SetContentsOfCell("A2", "");
            s.SetContentsOfCell("A3", "");
            s.SetContentsOfCell("A4", "");
            s.SetContentsOfCell("A5", "");
            s.SetContentsOfCell("A6", " ");
            s.SetContentsOfCell("A7", "    ");
            s.SetContentsOfCell("A8", "     ");
            s.SetContentsOfCell("A9", "  ");

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
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "4");
            s.SetContentsOfCell("A1", "5");
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
            AbstractSpreadsheet s = new Spreadsheet();

            //Add stuff.
            s.SetContentsOfCell("A1", "4");
            s.SetContentsOfCell("A2", "4");
            s.SetContentsOfCell("A3", "4");
            s.SetContentsOfCell("A4", "s");
            s.SetContentsOfCell("A5", "s");
            s.SetContentsOfCell("A6", "s");
            s.SetContentsOfCell("A7", "=" + new Formula("s1").ToString());
            s.SetContentsOfCell("A8", "=" + new Formula("s1").ToString());
            s.SetContentsOfCell("A9", "=" + new Formula("s1").ToString());

            //Remove a few.
            s.SetContentsOfCell("A1", "");
            s.SetContentsOfCell("A2", "");
            s.SetContentsOfCell("A3", "");
            s.SetContentsOfCell("A4", "");
            s.SetContentsOfCell("A5", "");
            s.SetContentsOfCell("A6", " ");

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
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellContents("2INVALID");
        }


        /// <summary>
        /// Null name throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContent2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellContents(null);

        }

        /// <summary>
        /// Try to get cell contents that are a double.
        /// </summary>
        [TestMethod]
        public void GetCellContent3()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A1", "145");

            object o = s.GetCellContents("A1");


            Assert.AreEqual(145, (double)o);
        }
        /// <summary>
        /// Try to get cell contents that are a string.
        /// </summary>
        [TestMethod]
        public void GetCellContent4()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "d");

            Assert.AreEqual("d", (string)(s.GetCellContents("A1")));
        }
        /// <summary>
        /// Try to get the contents of an empty cell.
        /// </summary>
        [TestMethod]
        public void GetCellContent5()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            Assert.AreEqual("", (string)s.GetCellContents("A1"));
        }

        /// <summary>
        /// Try to get the contents of a null string.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCellContent6()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            string n = null;
            s.SetContentsOfCell("a1", n);

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
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("", "4");
        }

        /// <summary>
        /// Add a double to a null cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "4");
        }

        /// <summary>
        /// Add a string to an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("", "4");
        }

        /// <summary>
        /// Add a string to an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents4()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "4");
        }

        /// <summary>
        /// Add a formula to a null cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents5()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "=" + new Formula("4").ToString());
        }


        /// <summary>
        /// Add a string to an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents6()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("565", "=" + new Formula("4").ToString());
        }


        /// <summary>
        /// Set cell contents, change the contents, check the cell.
        /// </summary>
        [TestMethod]
        public void SetCellContents7()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=" + new Formula("4").ToString());
            s.SetContentsOfCell("A1", "4");

            Assert.AreEqual(4, (double)s.GetCellContents("A1"));

        }

        //Checks for circular dependencies.

        /// <summary>
        /// Direct circular dependency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents8()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            string t = "=" + new Formula("A1 + A2").ToString();
            s.SetContentsOfCell("A1", t);
        }


        /// <summary>
        /// Indirect circular dependency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents9()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=" + new Formula("A2").ToString());
            s.SetContentsOfCell("A2", "=" + new Formula("A1").ToString());
        }

        /// <summary>
        /// Circular dependency much later.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents10()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=" + new Formula("A2 + A3").ToString());
            s.SetContentsOfCell("A3", "=" + new Formula("A4 + A5").ToString());
            s.SetContentsOfCell("A5", "=" + new Formula("A6 + A7").ToString());
            s.SetContentsOfCell("A7", "=" + new Formula("A1 + A1").ToString());

        }

        /************************ TESTS FOR GetCellValue ************************/

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValue1()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue("1A");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValue2()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue("null");
        }

        /// <summary>
        /// Cell value of a double.
        /// </summary>
        [TestMethod]
        public void GetCellValue3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");

            object o = s.GetCellValue("A1");
            double d = (double)o;
            Assert.AreEqual(5, d);

        }

        /// <summary>
        /// Cell value of a string.
        /// </summary>
        [TestMethod]
        public void GetCellValue4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5A");
            Assert.AreEqual("5A", (string)s.GetCellValue("A1"));
        }

        /// <summary>
        /// Cell value of a valid formula.
        /// </summary>
        [TestMethod]
        public void GetCellValue5()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            Formula F = new Formula("5 + A1");
            s.SetContentsOfCell("A2", "=" + F.ToString());
            object o = s.GetCellValue("A2");

            Assert.AreEqual(10, (double)o);
        }

        /// <summary>
        /// Cell value of a valid formula.
        /// </summary>
        [TestMethod]
        public void GetCellValue6()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "invalid");
            Formula F = new Formula("5 + A1");
            s.SetContentsOfCell("A2", "=" + F.ToString());
            object o = s.GetCellValue("A2");

            Assert.IsTrue(o is FormulaError);
        }

        /// <summary>
        /// Cell value of a valid formula.
        /// </summary>
        [TestMethod]
        public void GetCellValue7()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            Formula F = new Formula("A3 + A1");
            string p = "=" + F.ToString();
            s.SetContentsOfCell("A2", p);
            object o = s.GetCellValue("A2");

            Assert.AreEqual(5, (double)o);
        }

        /// <summary>
        /// Cell value of a valid formula, but values have been changed a few times.
        /// </summary>
        [TestMethod]
        public void GetCellValue8()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            Formula F = new Formula("A3 + A1 + A4 + A5");
            string p = "=" + F.ToString();
            s.SetContentsOfCell("A2", p);
            s.SetContentsOfCell("A1", "10");
            s.SetContentsOfCell("A3", "10");
            object o = s.GetCellValue("A2");

            Assert.AreEqual(20, (double)o);
        }

        /// <summary>
        /// Stress test a couple of formulas whose values have changed.
        /// </summary>
        [TestMethod]
        public void GetCellValue9()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            s.SetContentsOfCell("A2", "3");
            s.SetContentsOfCell("A3", "6"); //wrong calculations will produce a 17.
            s.SetContentsOfCell("A5", "=A4+A3");//11+8 == 19
            s.SetContentsOfCell("A4", "=A3+A2"); // = 8+3 = 11
            s.SetContentsOfCell("A3", "=A2+A1"); // = 8
            Formula F = new Formula("A5");
            string p = "=" + F.ToString();

            object o = s.GetCellValue("A5");

            Assert.AreEqual(19, (double)o);
        }

        /************************* TESTS FOR Save **********************************/

        /// <summary>
        /// Try to write a spreadsheet with no cells.
        /// </summary>
        [TestMethod]
        public void Save1()
        {
            TextWriter tw = File.CreateText("C:\\Users/ebrig/Documents/College/Save1.xml");
            Spreadsheet s = new Spreadsheet();
            s.Save(tw);
        }

        /// <summary>
        /// Try to write a spreadsheet with 1 cell.
        /// </summary>
        [TestMethod]
        public void Save2()
        {
            TextWriter tw = File.CreateText("C:\\Users/ebrig/Documents/College/Save2.xml");
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "234");
            s.Save(tw);
        }

        /// <summary>
        /// Multiple cells.
        /// </summary>
        [TestMethod]
        public void Save3()
        {
            TextWriter tw = File.CreateText("C:\\Users/ebrig/Documents/College/Save3.xml");
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "234");
            s.SetContentsOfCell("A2", "5");
            s.SetContentsOfCell("A3", "=A1 + 2");
            s.SetContentsOfCell("A4", "string");
            s.Save(tw);

        }

        /***************************** Tests for Spreadsheet constructor **************************/

        /// <summary>
        /// Multiple cells.
        /// </summary>
        [TestMethod]
        public void SSConstruct1()
        {
            TextReader tr = new StreamReader("C:\\Users/ebrig/Documents/College/Save3.xml");

            //File.ReadAllText("C:\\Users/ebrig/Documents/College/Save3.xml").ToString();
           string cellPattern = @"[a-zA-Z][a-zA-Z]*[1-9][0-9]*";
            Regex r = new Regex(cellPattern);
            Spreadsheet s = new Spreadsheet(tr, r);

            TextWriter tw = File.CreateText("C:\\Users/ebrig/Documents/College/Save3Check.xml");
            s.Save(tw);      

        }
    }
}
