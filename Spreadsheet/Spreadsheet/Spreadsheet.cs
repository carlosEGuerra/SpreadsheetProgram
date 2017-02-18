using System;
using System.Collections.Generic;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;

namespace SS
{
    /// <summary>
    /// A Spreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string s is a valid cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names.  On the other hand, 
    /// "Z", "X07", and "hello" are not valid cell names.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// A Dictionary representation of the spreadsheet where each item
        /// in allCells contains a string Key and an object Value.
        /// The Key represents the name of the cell and the Value represents 
        /// the Contents of the cell.
        /// </summary>
        private Cells allCells = new Cells();

        /// <summary>
        /// To keep track of all dependents and dependees in the spreadsheet.
        /// </summary>
        private DependencyGraph dependencies = new DependencyGraph();

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> list = new HashSet<string>();
            Dictionary<string, object> map = allCells.GetSheet();

            foreach (KeyValuePair<string, object> p in allCells.GetSheet())
            {
                object val = p.Value;

                if (val == null)//If the string contents are null.
                {
                    continue;
                }
                if (val is string //If the contents of the cell are Null, an empty string, or whitespace, it is empty.
                    && (String.IsNullOrEmpty((string)val) || String.IsNullOrWhiteSpace((string)val)))
                {
                    continue;
                }

                list.Add(p.Key);//Otherwise, the cell has contents. Add it to the list.
            }

            return list;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            bool valid = ValidCellCheck(name);

            if (!valid) //If the name of the cell is null or invalid
            {
                throw new InvalidNameException();
            }

            else if (!allCells.GetSheet().ContainsKey(name))//If our sheet hasn't yet formally added the cell
            {
                return "";//Return an empty string.
            }

            else
            {
                return allCells.GetSheet()[name]; //Return the contents at the specified cell.
            }

        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            bool valid = ValidCellCheck(name);
            HashSet<string> set = new HashSet<string>();

            if (!valid) //If the name of the cell is null or invalid
            {
                throw new InvalidNameException();
            }

            allCells.SetCell(name, number);//Change the contents of the cell.
            set.Add(name);

           // IEnumerator<string> dentsEnumer = dependencies.GetDependents(name).GetEnumerator();

            set = new HashSet<string>(GetCellsToRecalculate(name));

            return set;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }

            bool valid = ValidCellCheck(name);
            HashSet<string> set = new HashSet<string>();

            if (!valid) //If the name of the cell is null or invalid
            {
                throw new InvalidNameException();
            }

            set.Add(name); //Add name to the set.
            allCells.SetCell(name, text);//Add contents to the cell.

            set = new HashSet<string>(GetCellsToRecalculate(name));//Find all cells tat depend on the current cell.

            return set;
        }

        /// <summary>
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if(name == null)
            {
                throw new InvalidNameException();
            }

            bool valid = ValidCellCheck(name);
            HashSet<string> set = new HashSet<string>();

            HashSet<string> toRecalculate = new HashSet<string>(GetCellsToRecalculate(name));

            if (!valid) //If the name of the cell is null or invalid
            {
                throw new InvalidNameException();
            }

           HashSet<string> formVars = (HashSet<string>)formula.GetVariables();


            //For circular dependency, check that none of the variables we're adding to the dependents are equal to the name.
            if (formVars.Contains(name))
            {
                throw new CircularException();
            }

            //Check that none of the variables of the formula already depend on the current cell. 
            foreach (string s in formVars)
            {
                // IEnumerator<string> dentsEnum = dependencies.GetDependents(s).GetEnumerator();
                IEnumerator<string> deesEnum =  dependencies.GetDependees(s).GetEnumerator();
                List<string> dees = new List<string>();

                while (deesEnum.MoveNext())
                {
                    dees.Add(deesEnum.Current);
                }

                if (dees.Contains(name))
                {
                    throw new CircularException();
                }

                if (toRecalculate.Contains(s))
                {
                    throw new CircularException();
                }
            }


            //No circular dependencies at this point. Add the variable names to the set.
            set = new HashSet<string>(GetCellsToRecalculate(name));

            allCells.SetCell(name, formula);//Add contents to the cell representation.
            dependencies.ReplaceDependees(name, formVars); //Replace the dependees of the current cell. 

            return set;
        }


        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            bool valid = ValidCellCheck(name);
            HashSet<string> set = new HashSet<string>();

            if (!valid) //If the name of the cell is null or invalid
            {
                throw new InvalidNameException();
            }

            foreach (KeyValuePair<string, object> p in allCells.GetSheet())//Look at all of the cells we have.
            {
                if (GetCellContents(p.Key) is Formula)
                {
                    Formula contents = (Formula)GetCellContents(p.Key);

                    //If any of the variables of the formula we're looking at contain the name of the cell.
                    if (contents.GetVariables().Contains(name))
                    {
                        set.Add(p.Key);
                    }
                }
           
            }

            return set;
        }

        /// <summary>
        /// Checks if a string is a valid cell name.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// 
        private bool ValidCellCheck(string s)
        {
            if (s == null)
            {
                return false;
            }
            String cellPattern = @"[a-zA-Z][a-zA-Z]*[1-9][0-9]*";

            Regex regCell = new Regex(cellPattern);

            string check = regCell.Match(s).ToString();
            if (check != s)
            {
                return false;
            }
            return true;
        }
        
    }
}
