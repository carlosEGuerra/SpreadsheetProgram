using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using SS;

namespace Spreadsheet
{
    class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, object> cell;

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
            if (name.Equals(null))
            {
                throw new InvalidNameException();
            }

            //checks the object type of the values
            object obj;
            cell.TryGetValue(name, out obj);
            if (obj.GetType() == typeof(string))
            {
                return Convert.ChangeType(obj, typeof(string));
            }
            else if (obj.GetType() == typeof(double))
            {
                return Convert.ChangeType(obj, typeof(double));
            }
            else
            {
                return Convert.ChangeType(obj, typeof(Formula));
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
                if(s == null)
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
            if(name.Equals(null) || !name.IsNormalized())
            {
                throw new InvalidNameException();
            }

            //adds a set that if name is contained in variables then it adds it to the return set
            HashSet<string> retVal = new HashSet<string>();
            retVal.Add(name);
            foreach (string s in cell.Keys)
            {
                Formula form = new Formula(cell[s].ToString());
                if (form.GetVariables().Contains(name))
                {
                    retVal.Add(name);
                }
            }
            return retVal;
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
            if (text.Equals(null))
            {
                throw new ArgumentNullException();
            }
            if (name.Equals(null))
            {
                throw new InvalidNameException();
            }

            //adds to the set if it contains name in variables
            HashSet<string> dependentCells = new HashSet<string>();
            dependentCells.Add(name);
            foreach(string s in cell.Keys)
            {
                object str;
                cell.TryGetValue(s, out str);
                if (str.Equals(name))
                {
                    dependentCells.Add(name);
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
            if (name.Equals(null))
            {
                throw new InvalidNameException();
            }

            cell[name] = number;

            HashSet<string> retVal = new HashSet<string>();
            retVal.Add(name);

            //checks to see if a variable contained in values is contained
            foreach(string s in cell.Keys)
            {
                Formula form = new Formula(cell[s].ToString());
                if (form.GetVariables().Contains(name))
                {
                    retVal.Add(name);
                    continue;
                }
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
            if (name.Equals(null))
            {
                throw new ArgumentNullException();
            }

            HashSet<string> depCell = new HashSet<string>();
            foreach(string s in cell.Keys)
            {
                Formula form  = new Formula(cell[s].ToString());
                if (form.GetVariables().Contains(name))
                {
                    depCell.Add(name);
                    continue;
                }
            }
            return depCell;
        }
    }
}