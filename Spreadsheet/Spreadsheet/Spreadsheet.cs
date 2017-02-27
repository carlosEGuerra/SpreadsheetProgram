using System;
using System.Collections.Generic;
using Formulas;
using System.Text.RegularExpressions;
using System.IO;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, object> cell;

        public override bool Changed
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public Spreadsheet()
        {
            cell = new Dictionary<string, object>();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || !valid(name))            //changes the name check to name == null and check the validity of the name, also added a check to make sure that dictionary contains name
            {
                throw new InvalidNameException();
            }

            //checks the object type of the values
            object obj = "";
            cell.TryGetValue(name, out obj);
            if (obj == null)
            {
                return "";
            }
            if (obj.GetType() == typeof(String))
            {
                return Convert.ChangeType(obj, typeof(String));
            }
            else if (obj.GetType() == typeof(double))
            {
                return Convert.ChangeType(obj, typeof(double));
            }
            else if(obj.GetType().Equals(typeof(Formula)))
            {
                return Convert.ChangeType(obj, typeof(Formula));
            }
            else
            {
                return obj;
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> retVal = new HashSet<string>();
            foreach(string s in cell.Keys)
            {
                if(String.IsNullOrWhiteSpace(cell[s].ToString()))       //changed to check if the string value at cell[s] is null or whitespace
                {
                    continue;
                }
                else
                {
                    retVal.Add(s);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
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
            if (name == null || !valid(name))         //checked if name is valid instead of null and checked if name == null, also added a check if the dictionary contains the name
            {
                throw new InvalidNameException();
            }
            Dictionary<string, object> prevState = new Dictionary<string, object>();
            foreach (string key in cell.Keys)
            {
                prevState.Add(key, cell[key]);
            }

            checkForCircularDependency(name, formula);          //added a check for circular dependencies
            try
            {
                 if (!cell.ContainsKey(name))             //added a check if dictionary contains the key and adds it if isn't
                 {
                    cell.Add(name, formula);
                    GetCellsToRecalculate(name);
                 }
                 else if (cell.ContainsKey(name))
                 {
                    cell.Remove(name);
                    cell[name] = formula;
                    GetCellsToRecalculate(name);
                 }
            }
            catch (CircularException)
            {
                cell = prevState;
                throw new CircularException();
            }

            HashSet<string> retVal = new HashSet<string>();
            retVal.Add(name);

            IEnumerable<string> s = GetCellsToRecalculate(name);
            foreach(string str in GetCellsToRecalculate(name))
            {
                retVal.Add(str);
            }
           
            return retVal;
        }

        //Need Help With this
        private void checkForCircularDependency(string name, Formula formula)
        {
            Dictionary<string, object> cellTemp = new Dictionary<string, object>();
            foreach(string s in cell.Keys)
            {
                cellTemp.Add(s, cell[s]);
            }

            if (cellTemp.ContainsKey(name))
            {
                cellTemp[name] = formula;
            }
            else
            {
                cellTemp.Add(name, formula);
            }

            ISet<string> formVar = formula.GetVariables();
            if (formVar.Contains(name))
            {
                throw new CircularException();
            }

            HashSet<string> subset;
            GetCellsToRecalculate(subset = new HashSet<string>(GetDirectDependents(name)));
               
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
            if (text == null)                           //changed to == 
            {
                throw new ArgumentNullException();
            }
            if (name == null || !valid(name))                            //checked validation of name, added a check to ensure that dictionary contains name
            {
                throw new InvalidNameException();
            }

            //adds to the set if it contains name in variables
            HashSet<string> dependentCells = new HashSet<string>();
            dependentCells.Add(name);
            foreach(string s in cell.Keys)
            {
                IEnumerable<string> sr = GetCellsToRecalculate(s);
                foreach(string str in sr)
                {
                    if (str.Contains(name))
                    {
                        dependentCells.Add(str);
                    }
                }
            }

            //sets the contents of the name cell to the text
            cell[name] = text;
            return dependentCells;
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
            if (name == null || !valid(name))               //changes form .equals null to == null, added a check to check if name == null
            {
                throw new InvalidNameException();
            }

            cell[name] = number;

            HashSet<string> retVal = new HashSet<string>();
            retVal.Add(name);
            IEnumerable<string> dep = GetCellsToRecalculate(name);
            foreach(string s in dep)
            {
                retVal.Add(s);
            }
            return retVal;
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
            else if(!valid(name))
            {
                throw new InvalidNameException();
            }

            HashSet<string> depCell = new HashSet<string>();
            foreach(string s in cell.Keys)
            {
                Formula form  = new Formula(cell[s].ToString());
                if (form.GetVariables().Contains(name))
                {
                    depCell.Add(s);
                    continue;
                }
            }
            return depCell;
        }

        /// <summary>
        /// This returns whether or not a name of a cell is valid or not.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool valid(string cell)
        {
            if (Regex.IsMatch(cell, "^[a-zA-Z]+[1-9]+[0-9]*$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // ADDED FOR PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the IsValid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            
        }

        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if(content == null)
            {
                throw new ArgumentNullException();
            }
            else if(name == null || !valid(name))
            {
                throw new InvalidNameException();
            }

            //if content is a double then the named cells contents will be replaced with the 
            //new contents
            if(content.GetType() == typeof(double))
            {
                Double dub = 0;
                Double.TryParse(content, out dub);
                cell[name] = dub;
            }

            //gets the index of the = and runs it through
            int index = content.IndexOf("=");
            if(index == 0)
            {
                Formula form = new Formula(content.Substring(1, content.Length - 1), s => s.toUpper(), valid(form));
                form.Evaluate()
            }
        }
    }
}