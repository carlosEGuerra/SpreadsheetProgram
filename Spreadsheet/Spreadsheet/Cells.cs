using System;
using Dependencies;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    /// <summary>
    /// A cell class to represent cells of a spreadsheet.
    /// /summary>
    public class Cells
    {
        private Dictionary<string, object> Sheet; 
        /// <summary>
        /// Zero constructor for an empty cell.
        /// </summary>
        public Cells()
        {
            Sheet = new Dictionary<string, object>();
        }

        /// <summary>
        /// Assign contents to a cell. Does nothing if the cell name is null.
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// </summary>
        public void SetCell(string s, object c)
        {
            if(s == null)
            {
                return;
            }

            if (!Sheet.ContainsKey(s)) //If we've not explicitly added the cell to our set.
            {
                Sheet.Add(s, c);
                return;
            }

            Sheet[s] = c;
            return;
        }

        /// <summary>
        /// Returns the entire contents of the represented spreadsheet.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetSheet()
        {
            return this.Sheet;
        }

        //public object GetValue()
        //{
        //    if (contents.GetType() == typeof(Formula))
        //    {
        //        return ((Formula)contents); 
        //    }
        //}
    }
}
