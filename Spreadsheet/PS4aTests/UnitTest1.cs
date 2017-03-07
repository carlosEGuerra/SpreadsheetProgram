//Original tests written by Joe Zachary for CS 3500
//Appeneded by Ellen Brigance, February 2017
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Formulas
{
    /// <summary>
    /// PS2 Grading Tests Modiifed for PS4a
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
       
        // Tests of syntax errors detected by the constructor
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1()
        {
            Formula f = new Formula("        ", IdentityFn, TrueValid);
        }

        /// <summary>
        /// Tests a null string. 
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullString()
        {
            string s = null;
            Formula f = new Formula(s, IdentityFn, TrueValid);
        }

        /// <summary>
        /// Tests a null string. 
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullString2()
        {
            string s = null;
            Formula f = new Formula(s);
        }

        /// <summary>
        /// Tests a null string. 
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNegativeFormula()
        {
            string s = "-3 + 5 + 7";
            Formula f = new Formula(s, IdentityFn, TrueValid);
        }

        /// <summary>
        /// Tests a null normalizer.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullNormalizer()
        {
            Normalizer N = null;
            Formula f = new Formula("        ", N, TrueValid);
        }


        /// <summary>
        /// Tests a Normalizer that creates an illegal variable.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestBadNormalizer()
        {
            Formula f = new Formula("1 + x + 7 + t", s => "_" + s, TrueValid);
        }

        /// <summary>
        /// Test a validator that always makes string validity false.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestBadValidator()
        {
            Formula g = new Formula("1 + x + 7 + t", IdentityFn, FalseValid);
        }

        /// <summary>
        /// Test the zero constructor.
        /// </summary>
        [TestMethod()]
        public void TestToStringZero()
        {
            Formula g = new Formula("0");
            Assert.AreEqual(g.ToString(), "0");
        }

        /// <summary>
        /// Test the zero constructor.
        /// </summary>
        [TestMethod()]
        public void TestToString1()
        {
            string s = "x+    6";
            Formula g = new Formula(s);
            Assert.AreEqual(g.ToString(), s);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test2()
        {
            Formula f = new Formula("((2 + 5))) + 8", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3()
        {
            Formula f = new Formula("2+5*8)", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test4()
        {
            Formula f = new Formula("((3+5*7)", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test5()
        {
            Formula f = new Formula("+3", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test6()
        {
            Formula f = new Formula("-y", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test7()
        {
            Formula f = new Formula("*7", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test8()
        {
            Formula f = new Formula("/z2x", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test9()
        {
            Formula f = new Formula(")", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test10()
        {
            Formula f = new Formula("(*5)", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test11()
        {
            Formula f = new Formula("2 5", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test12()
        {
            Formula f = new Formula("x5 y", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test13()
        {
            Formula f = new Formula("((((((((((2)))))))))", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test14()
        {
            Formula f = new Formula("$", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15()
        {
            Formula f = new Formula("x5 + x6 + x7 + (x8) +", IdentityFn, TrueValid);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15a()
        {
            Formula f = new Formula("x1 ++ y1", IdentityFn, TrueValid);
        }

        /// <summary>
        /// Extra valid formula for code coverage.
        /// </summary>
        [TestMethod()]
        public void Test15b()
        {
            Formula f = new Formula("(3 + 7) + x3", IdentityFn, TrueValid);
        }

        /// <summary>
        /// Invalid formula for code coverage.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15c()
        {
            Formula f = new Formula("3 3", IdentityFn, TrueValid);
        }

        /// <summary>
        /// Invalid formula for code coverage.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15d()
        {
            Formula f = new Formula("3 3", IdentityFn, TrueValid);
        }
        // Simple tests that throw FormulaEvaluationExceptions
        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test16()
        {
            Formula f = new Formula("2+x", IdentityFn, TrueValid);
            f.Evaluate(s => { throw new UndefinedVariableException(s); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test17()
        {
            Formula f = new Formula("5/0", IdentityFn, TrueValid);
            f.Evaluate(s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18()
        {
            Formula f = new Formula("(5 + x) / (y - 3)", IdentityFn, TrueValid);
            f.Evaluate(s => 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18a()
        {
            Formula f = new Formula("(5 + x) / (3 * 2 - 12 / 2)", IdentityFn, TrueValid);
            f.Evaluate(s => 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test19()
        {
            Formula f = new Formula("x + y", IdentityFn, TrueValid);
            f.Evaluate(s => { if (s == "x") return 0; else throw new UndefinedVariableException(s); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test20()
        {
            Formula f = new Formula("x1 + x2 * x3 + x4 * x5 * x6 + x7", IdentityFn, TrueValid);
            f.Evaluate(s => { if (s == "x7") throw new UndefinedVariableException(s); else return 1; });
        }

        
        // Simple formulas
        [TestMethod()]
        public void Test21()
        {
            Formula f = new Formula("4.5e1", IdentityFn, TrueValid);
            Assert.AreEqual(45, f.Evaluate(s => 0), 1e-6);
        }


        [TestMethod()]
        public void Test21a()
        {
            Formula f = new Formula("4", IdentityFn, TrueValid);
            Assert.AreEqual(4, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test22()
        {
            Formula f = new Formula("a05", IdentityFn, TrueValid);
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod()]
        public void Test22a()
        {
            Formula f = new Formula("a1b2c3d4e5f6g7h8i9j10", IdentityFn, TrueValid);
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod()]
        public void Test23()
        {
            Formula f = new Formula("5 + x", IdentityFn, TrueValid);
            Assert.AreEqual(9, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test24()
        {
            Formula f = new Formula("5 - y", IdentityFn, TrueValid);
            Assert.AreEqual(1, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test25()
        {
            Formula f = new Formula("5 * z", IdentityFn, TrueValid);
            Assert.AreEqual(20, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test26()
        {
            Formula f = new Formula("8 / xx", IdentityFn, TrueValid);
            Assert.AreEqual(2, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test27()
        {
            Formula f = new Formula("(5 + 4) * 2", IdentityFn, TrueValid);
            Assert.AreEqual(18, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test28()
        {
            Formula f = new Formula("1 + 2 + 3 * 4 + 5", IdentityFn, TrueValid);
            Assert.AreEqual(20, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test29()
        {
            Formula f = new Formula("(1 + 2 + 3 * 4 + 5) * 2", IdentityFn, TrueValid);
            Assert.AreEqual(40, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test30()
        {
            Formula f = new Formula("((((((((((((3))))))))))))", IdentityFn, TrueValid);
            Assert.AreEqual(3, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test31()
        {
            Formula f = new Formula("((((((((((((x))))))))))))", IdentityFn, TrueValid);
            Assert.AreEqual(7, f.Evaluate(s => 7), 1e-6);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test32B()
        {
            Formula f = new Formula("((((((((((((x))))))))))))", IdentityFn, TrueValid);
            f.Evaluate(null);
        }

        /// <summary>
        /// Divide by a lookup of zero.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test32C()
        {
            Formula f = new Formula("4 / A", IdentityFn, TrueValid);
            f.Evaluate(s=>0);
        }

        /// <summary>
        /// Make sure a formula with no variables produces no variables.
        /// </summary>
        [TestMethod()]
        public void TestGetVariablesNorm()
        {
            Formula f = new Formula("8", IdentityFn, TrueValid);
            Assert.IsTrue(f.GetVariables().Count == 0);
        }

        /// <summary>
        /// Make sure a formula with all variables catches all variables, but no duplicates.
        /// </summary>
        [TestMethod()]
        public void TestGetVariables1()
        {
            Formula f = new Formula("4 + 6 -s + r3 + r3 - t + a5a", IdentityFn, TrueValid);
            Assert.IsTrue(f.GetVariables().Count == 4);
            Assert.IsTrue(f.GetVariables().Contains("s"));
            Assert.IsTrue(f.GetVariables().Contains("r3"));
            Assert.IsTrue(f.GetVariables().Contains("t"));
            Assert.IsTrue(f.GetVariables().Contains("a5a"));
        }

        /// <summary>
        /// Make sure a formula with all variables catches all variables, but no duplicates.
        /// </summary>
        [TestMethod()]
        public void TestGetVariablesZero()
        {
            Formula f = new Formula("0");
            Assert.IsTrue(f.GetVariables().Count == 0);

        }

        // Some more complicated formula evaluations
        [TestMethod()]
        public void Test32()
        {
            Formula f = new Formula("y*3-8/2+4*(8-9*2)/14*x", IdentityFn, TrueValid);
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x") ? 1 : 4), 1e-9);
        }

        [TestMethod()]
        public void Test32a()
        {
            Formula f = new Formula("a + b * c - d + 3 * 3.0 - 3.0e0 / 0.003e3", IdentityFn, TrueValid);
            Assert.AreEqual(17, (double)f.Evaluate(s => 3), 1e-9);
        }

        [TestMethod()]
        public void Test33()
        {
            Formula f = new Formula("a+(b+(c+(d+(e+f))))", IdentityFn, TrueValid);
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod()]
        public void Test34()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6", IdentityFn, TrueValid);
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod()]
        public void Test35()
        {
            Formula f = new Formula("a-a*a/a", IdentityFn, TrueValid);
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod()]
        public void Test36()
        {
            Formula f1 = new Formula("xx+3", IdentityFn, TrueValid);
            Formula f2 = new Formula("xx-3", IdentityFn, TrueValid);
            Assert.AreEqual(6, f1.Evaluate(s => 3), 1e-6);
            Assert.AreEqual(0, f2.Evaluate(s => 3), 1e-6);
        }

        [TestMethod()]
        public void Test37()
        {
            Test36();
        }

        [TestMethod()]
        public void Test38()
        {
            Test36();
        }

        [TestMethod()]
        public void Test39()
        {
            Test36();
        }

        [TestMethod()]
        public void Test40()
        {
            Test36();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test41()
        {
            Formula f = new Formula("(((((2+3*a)/(7e-5+b-c))*d+.0005e+92)-8.2)*3.14159) * ((e+3.1)-.00000000008)");
        }

        // Stress test for constructor, repeated five times to give it extra weight.
        [TestMethod()]
        public void Test42()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test43()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test44()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test45()
        {
            Test41();
        }

        /// <summary>
        /// An identity function for reuse as a parameter for the Formula Constructor.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string IdentityFn(string s)
        {
            return s;
        }

        /// <summary>
        /// An identity function for reuse as a parameter for the Formula Constructor.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool TrueValid(string s)
        {
            return true;
        }

        private bool FalseValid(string s)
        {
            return false;
        }
    }
}
