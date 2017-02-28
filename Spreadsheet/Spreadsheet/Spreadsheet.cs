//Skeleton Written by Joe Zachary for CS 3500
//Appeneded and implementd by Ellen Brigance, February 2017
using System;
using System.Collections.Generic;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Schema;

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
        private Cells allCells;

        /// <summary>
        /// To keep track of all dependents and dependees in the spreadsheet.
        /// </summary>
        private DependencyGraph dependencies;

        /// <summary>
        /// Keeps track of te current Regex if we're employing one.
        /// </summary>
        private string isValidReg;

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            //Set all instance variables.
            allCells = new Cells();
            dependencies = new DependencyGraph();
            Changed = false;
            isValidReg = @"[a-zA-Z][a-zA-Z]*[1-9][0-9]*";
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        /// <param name="isValid"></param>
        public Spreadsheet(Regex isValid)
        {
            //Set all instance variables.
            allCells = new Cells();
            dependencies = new DependencyGraph();
            Changed = false;
            isValidReg = isValid.ToString();
        }

        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        /// <param name="source"></param>
        /// <param name="newIsValid"></param>
        public Spreadsheet(TextReader source, Regex newIsValid)
        {
            //Set all instance variables.
            allCells = new Cells();
            dependencies = new DependencyGraph();
            Changed = false;
            isValidReg = newIsValid.ToString();

            //Create schema
            XmlSchemaSet sc = new XmlSchemaSet();


            sc.Add(null, "Spreadsheet.xsd");//Add schema to be validated against.

            //Configure validaton. 
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;


            //Add the cells and the validator Regex.
            using (XmlReader xReader = XmlReader.Create(source, settings))
            {
                while (xReader.Read())
                {
                    if (xReader.IsStartElement())
                    {
                        switch (xReader.Name)
                        {
                            case "spreadsheet":
                                string fileValidator = xReader["IsValid"];
                                Regex oldIsValid;
                                try //If the IsValid is not a c# regex, throw spreadsheet read expression.
                                {
                                    oldIsValid = new Regex(fileValidator);
                                }
                                catch
                                {
                                    throw new SpreadsheetReadException("Invalid Validator found in XML file");
                                }

                                isValidReg = oldIsValid.ToString(); //We have a valid Regex, use this one as our Spreadsheet Validator.
                                break;

                            case "cell": //If we have a cell, add it to the Spreadsheet.
                                string cellName = xReader["cell"].ToUpper();
                                string cellContents = xReader["contents"];

                                if (allCells.GetSheet().ContainsKey(cellName)) //We already added the cell.
                                {
                                    throw new SpreadsheetReadException("Duplicate cell names found in XML file.");
                                }

                                else
                                {
                                    //Check validity with oldIsValid
                                    try
                                    {
                                        SetCellContents(cellName, cellContents);
                                    }
                                    catch (InvalidNameException)
                                    {
                                        throw new SpreadsheetReadException("Invalid cell name found.");
                                    }
                                    catch (FormulaFormatException)
                                    {
                                        throw new SpreadsheetReadException("Invalid formula found.");
                                    }
                                    catch (CircularException)
                                    {
                                        throw new SpreadsheetReadException("Circular dependency found.");
                                    }

                                    //Check validity with newIsValid
                                    isValidReg = newIsValid.ToString();

                                    try
                                    {
                                        SetCellContents(cellName, cellContents);
                                    }
                                    catch (InvalidNameException)
                                    {
                                        throw new SpreadsheetVersionException("Invalid cell name found.");
                                    }
                                    catch (FormulaFormatException)
                                    {
                                        throw new SpreadsheetVersionException("Invalid formula found.");
                                    }
                                    catch (CircularException)
                                    {
                                        throw new SpreadsheetReadException("Circular dependency found.");
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Display any validation errors.
        /// **Citation: Author: Joe Zachary (https://github.com/UofU-CS3500-S17/examples/blob/master/RegexAndXML/XML/XML.cs)
        /// </summary>
        /// <author name ="Joe Zachary">
        /// </author>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            throw new SpreadsheetReadException("File not consistent with Spreadsheet schema.");
        }


        // ADDED FOR PS6
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get; protected set;

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
            //XmlWriterSettings settings = new XmlWriterSettings(); //Must use for proper spacing/newlines.

            List<string> cells = new List<string>(GetNamesOfAllNonemptyCells()); //Get all of our cells.

            using (XmlWriter xWrite = XmlWriter.Create(dest))
            {

                xWrite.WriteStartDocument();
                xWrite.WriteStartElement("","spreadsheet",""); // Generates "<spreadsheet>" 
                xWrite.WriteAttributeString("IsValid", isValidReg); //IsValid="IsValid regex goes here" 


                foreach (string c in cells)
                {
                    string contents;
                    // xWrite.WriteWhitespace("\n\t");//Make sure to enter and indent each time.
                    xWrite.WriteStartElement("cell");// <cell
 

                    if (allCells.GetSheet()[c][0] is string || allCells.GetSheet()[c][0] is double)
                    {
                        contents = allCells.GetSheet()[c][0].ToString(); //contents = "cell contents go here"
                    }

                    else if (allCells.GetSheet()[c][0] is Formula)
                    {
                        contents = "=" + allCells.GetSheet()[c][0].ToString();
                    }
                    else
                    {
                        contents = allCells.GetSheet()[c][0].ToString(); //contents="cell contents go here"
                    }

                    xWrite.WriteAttributeString("name", c); // name="cell name goes here"
                    xWrite.WriteAttributeString("content", contents);
                    xWrite.WriteFullEndElement();//</cell>
                }
                //Done with cells. Close out the document.
                xWrite.WriteFullEndElement();
                xWrite.WriteEndDocument();
            }
            //MAKE SURE TO CHANGE THE CURRENT STATE OF THE SPREADSHEET.
            Changed = false;
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
            if (name == null || !ValidCellCheck(name))
            {
                throw new InvalidNameException();
            }

            else
            {
                if (!allCells.GetSheet().ContainsKey(name))//If we haven't declared the cell yet
                {
                    return 0; //The value of the cell is automatically 0.
                }

                return allCells.GetSheet()[name][1];//Otherwise, return the set value of the cell. 
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> list = new HashSet<string>();
            Dictionary<string, object[]> map = allCells.GetSheet();

            foreach (KeyValuePair<string, object[]> p in allCells.GetSheet())
            {
                object val = p.Value[0];

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
                object o = allCells.GetSheet()[name][0];
                if (o is Formula)
                {
                    return (Formula)o;
                }
                if (o is Double)
                {
                    return (Double)o;
                }

                return (string)o;
            }

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
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !ValidCellCheck(name))
            {
                throw new InvalidNameException();
            }

            HashSet<string> H = new HashSet<string>();
            double d;
            bool setDone = false;
            //SETTING CONTENT, REEVALUATING THE VALUE FOR A DOUBLE.
            if (Double.TryParse(content, out d))
            {
                H = new HashSet<string>(SetCellContents(name, d));
                setDone = true;
            }

            //FOR FORMULA
            if (!String.IsNullOrWhiteSpace(content) && setDone == false)
            {
                if (content[0].Equals('='))
                {
                    string f = content.Substring(1);
                    Formula F = new Formula(f, s => s.ToUpper(), t => ValidCellCheck(t));
                    H = new HashSet<string>(SetCellContents(name, F));
                    setDone = true;
                }
            }

            if (content is string && setDone == false) //FOR STRING
            {
                H = new HashSet<string>(SetCellContents(name, content));
            }

            Changed = true; //WE'VE MADE A CHANGE TO THE SPREADSHEET.
            return H;
        }

        // MODIFIED PROTECTION FOR PS6
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
        protected override ISet<string> SetCellContents(string name, double number)
        {
            bool valid = ValidCellCheck(name);
            HashSet<string> set = new HashSet<string>();

            if (!valid) //If the name of the cell is null or invalid
            {
                throw new InvalidNameException();
            }

            allCells.SetCell(name, number, number);//Add the contents to the cell. 
            set.Add(name);

            List<string> list = new List<string>(GetCellsToRecalculate(name));//Ordered version
            UpdateCellValues(list); //Update values of cells.

            foreach (string s in list)
            {
                set.Add(s);
            }

            return set;
        }

        // MODIFIED PROTECTION FOR PS6
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
        protected override ISet<string> SetCellContents(string name, string text)
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
            allCells.SetCell(name, text, text);//Add contents and value to the cell.

            List<string> list = new List<string>(GetCellsToRecalculate(name));//Ordered version
            UpdateCellValues(list); //Update values of cells.

            foreach (string s in list)
            {
                set.Add(s);
            }

            return set;
        }

        // MODIFIED PROTECTION FOR PS6
        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }

            bool valid = ValidCellCheck(name);
            HashSet<string> set = new HashSet<string>();

            //Save the old dependency graph and the old cell contents.
            DependencyGraph oldDg = new DependencyGraph(dependencies);

            object oldContent = null;

            if (allCells.GetSheet().ContainsKey(name))
            {
                oldContent = allCells.GetSheet()[name][0];
            }

            List<string> formVars = new List<string>(formula.GetVariables());

            //Add the contents.
            allCells.SetCell(name, formula, null);//Add contents to the cell representation.
            dependencies.ReplaceDependees(name, formVars); //Replace the dependees of the current cell. 


            //Find circular dependencies.
            try
            {
                List<string> toRecalculate = new List<string>(GetCellsToRecalculate(name));
                foreach (string s in toRecalculate)
                {
                    set.Add(s);
                }
                UpdateCellValues(toRecalculate); //Update values of cells.
            }
            catch (CircularException)
            {
                dependencies = oldDg; //Revert back to the old graph.
                if (oldContent != null)
                {
                    allCells.GetSheet()[name][0] = oldContent;
                }
                if (oldContent == null)
                {
                    allCells.GetSheet().Remove(name);
                }

                throw new CircularException();
            }

            //   List<string> list = new List<string>(GetCellsToRecalculate(name));//Ordered version

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
            List<string> set = new List<string>();

            if (!valid) //If the name of the cell is null or invalid
            {
                throw new InvalidNameException();
            }

            //TRYING THIS
            set = new List<string>(dependencies.GetDependents(name));


            /*            foreach (KeyValuePair<string, object[]> p in allCells.GetSheet())//Look at all of the cells we have.
                        {
                            object o = GetCellContents(p.Key);
                            if (o is Formula)
                            {
                                Formula contents = (Formula) o;

                                //If any of the variables of the formula we're looking at contain the name of the cell.
                                if (contents.GetVariables().Contains(name))
                                {
                                    set.Add(p.Key);
                                }
                            }

                        }

                */

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

            if (String.IsNullOrEmpty(s))
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            String cellPattern;

            if (String.IsNullOrEmpty(isValidReg) || String.IsNullOrWhiteSpace(isValidReg))
            {
                cellPattern = @"[a-zA-Z][a-zA-Z]*[1-9][0-9]*";
            }
            else
            {
                cellPattern = isValidReg;
            }

            Regex regCell = new Regex(cellPattern);

            string check = regCell.Match(s).ToString();
            if (check != s)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Evaluates the values of all cells needing to be updated.
        /// </summary>
        /// <param name="recalcList"></param>
        /// <returns></returns>
        private void UpdateCellValues(List<string> recalcList)
        {
            foreach (string s in recalcList)
            {
                object content = allCells.GetSheet()[s][0];
                object val = allCells.GetSheet()[s][1];

                if (content is double || content is string) //If the current content is simply a double or string
                {
                    continue; //No update needed. Move to next cell.
                }
                if (content is Formula)//Formulas must be reevaluated.
                {
                    try
                    {
                        Formula f = (Formula)content;
                        double newVal = f.Evaluate(VarLookup);
                        allCells.GetSheet()[s][1] = newVal; //Update the value of the current cell.
                    }
                    catch (FormulaEvaluationException) //If our formula could not be properly evaluated
                    {
                        allCells.GetSheet()[s][1] = new FormulaError();//The value of the cel is an error.
                    }
                }
            }
        }

        /// <summary>
        /// For use with the Evaluate function of Formula:
        /// var is the current variable of the formula.
        /// Throws a FormulaEvaluationException if the lookup value was not a valid double. 
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        private double VarLookup(string var)
        {
            //Evaluate looks at one variable each time the lookup is called.

            //Empty/undeclared cell.
            if (!allCells.GetSheet().ContainsKey(var))
            {
                return 0;//Value of empty cell is always 0.
            }

            //Look up the given cell.
            object curContents = allCells.GetSheet()[var][0];
            object curVal = allCells.GetSheet()[var][1];

            //Formula or double where value is already determined.
            if ((curContents is double || curContents is Formula) && curVal is double)
            {
                return (double)curVal;
            }
            //If the value at a variable is a string
            if ((curContents is string || curContents is Formula) && (curVal is string || curVal is FormulaError))
            {
                allCells.GetSheet()[var][1] = new FormulaError();//Can't operate with a string.
                throw new FormulaEvaluationException("One or more cell values in formula was not a double.");
            }

            return 0;
        }
    }
}
