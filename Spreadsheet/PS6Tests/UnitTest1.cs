using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace SS
{
    /// <summary>
    /// Tester class for the updated spreadsheet.
    /// </summary>
    [TestClass]
    public class PS6PersonalTester
    {

        /*********************** TESTS ON SetContentsOfCell *************************/
        [TestMethod]
        public void TestMethod1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "1.5");
        }
    }
}
