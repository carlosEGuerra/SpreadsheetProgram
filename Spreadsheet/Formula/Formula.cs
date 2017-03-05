// Skeleton written by Joe Zachary for CS 3500, January 2017
// Student: Carlos Guerra u0847821

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Formulas

{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>

    public struct Formula
    { 
        private IEnumerable<string> enumForm;
        private string[] symbolList;
        private Normalizer normailzer;
        private Validator validator;
        private string stringFormula;
        private HashSet<string> variableSet;

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula): this(formula, N => N, V => true)
        {

        }
        public Formula(String f, Normalizer N, Validator V)
        {
            //Checks to see if there will be at least 1 token
            if (string.IsNullOrWhiteSpace(f))
            {
                throw new FormulaFormatException("Sorry, but you are missing some input");
            }
            if(f == null || N == null || V == null)
            {
                throw new ArgumentNullException("Inputs are null");
            }
            normailzer = N;
            validator = V;
            stringFormula = "";
            variableSet = new HashSet<string>();

            //creates a list of valid symbols
            symbolList = new string[6];
            symbolList[0] = "+";
            symbolList[1] = "-";
            symbolList[2] = "*";
            symbolList[3] = "/";
            symbolList[4] = "(";
            symbolList[5] = ")";

            //converts the formula string into tokens
            enumForm = GetTokens(f);
            //gets the first token
            string firstToken = enumForm.First();
            //gets the second token
            string lastToken = enumForm.Last();

            //checkes if first token of a formula is a number, a variable, or an opening parenthesis.
            double num;
            if (!firstToken.Equals("("))
            {
                if (!double.TryParse(firstToken, out num))
                {
                    if (!isVariable(firstToken))
                    {
                        throw new FormulaFormatException("Sorry but the first can only be a number, variable or open parenthesis");
                    }
                }
            }

            // checks if last token of a formula must be a number, a variable, or a closing parenthesis.
            if (!lastToken.Equals(")"))
            {
                if (!double.TryParse(lastToken, out num))
                {
                    if (!isVariable(lastToken))
                    {
                        throw new FormulaFormatException("Sorry but the first can only be a number, variable or open parenthesis");
                    }
                }
            }
            string tokenPrior = null;
            Stack<string> parenthesisStack = new Stack<string>();
            int openeningParen = 0, closingParen = 0;
            foreach (string token in enumForm)
            {
                //Ignores the whitespace
                if (token.Equals(" "))
                {
                    continue;
                }

                //Checks to see if the token is valid
                double numb;
                if (!double.TryParse(token, out numb) && !symbolList.Contains(token) && !isVariable(token))
                {
                    throw new FormulaFormatException("Sorry but thats the wrong format for" + token);
                }

                //uses a stack to check number of parenthesis
                if (token.Equals("("))
                {
                    parenthesisStack.Push("(");
                    openeningParen++;

                }
                else if (token.Equals(")"))
                {
                    if (parenthesisStack.Count == 0 || !parenthesisStack.Peek().Equals("("))
                    {
                        throw new FormulaFormatException("Your missing a parenthesis");
                    }
                    else
                    {
                        parenthesisStack.Pop();
                        closingParen++;
                        if (closingParen > openeningParen)
                        {
                            throw new FormulaFormatException("You have too many closing parenthesis");
                        }
                    }
                }
                //checks if the number of opening parenthesis is smaller then the number of closing parenthesis
                if (closingParen > openeningParen)
                {
                    throw new FormulaFormatException("The number of closing parenthesis is greater than the number of opening parenthesis");
                }


                //Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, 
                //or an opening parenthesis.
                if (!String.IsNullOrEmpty(tokenPrior))
                {
                    double testParse;
                    if (tokenPrior.Equals("(") || symbolList.Contains(tokenPrior) && !(tokenPrior.Equals(")")))
                    {
                        if (!(double.TryParse(token, out testParse) || isVariable(token) || token.Equals("(")))
                        {
                            throw new FormulaFormatException("Line 127");
                        }
                    }

                    //Any token that immediately follows a number, a variable, or a closing parenthesis must be either an 
                    //operator or a closing parenthesis
                    else if (double.TryParse(tokenPrior, out testParse) || isVariable(tokenPrior) || tokenPrior.Equals(")"))
                    {
                        if (!(symbolList.Contains(token) || token.Equals(")")) && !(tokenPrior.Equals("(")))
                        {
                            throw new FormulaFormatException("Line 136");
                        }
                    }
                }

                //checks the requirements for PS4a part 3
                if (isVariable(token) && !isVariable(normailzer(token)))
                {
                    throw new FormulaFormatException("Sorry, but when normalized the token is not a valid variable");
                }
                else if (validator(normailzer(token)) == false)
                {
                    throw new FormulaFormatException("Sorry but when validator is run on the normailzed token it return false");
                }
                else if(isVariable(token) &&isVariable(normailzer(token)) && validator(normailzer(token)))
                {
                    variableSet.Add(normailzer(token));
                    stringFormula += normailzer(token);
                }
                else
                {
                    stringFormula += token;
                }
                tokenPrior = token;
            }

            //Checks if the correct number of parenthesis is used
            if (parenthesisStack.Count != 0)
            {
                throw new FormulaFormatException("Sorry but you are missing some parenthesis");
            }

            //Assigns the new formula to the olds
            f = stringFormula;
        }

        private bool isVariable(string token)
        {
            char[] charArr = token.ToCharArray();
            if (!char.IsLetter(charArr[0]))
            {
                return false;
            }
            for (int i = 1; i < charArr.Count(); i++)
            {
                double testDouble = 0;
                if (!(Char.IsLetter(charArr[i]) || double.TryParse(charArr[i].ToString(), out testDouble)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            if (String.IsNullOrEmpty(stringFormula) || stringFormula == "0")
            {
                return 0;
            }
            
            //initiallizes both the value and operator stacks
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            //organizes the formula into the two stacks
            foreach (string t in enumForm)
            {
                //ignores whitespace tokens
                if (string.IsNullOrWhiteSpace(t))
                {
                    continue;
                }

                double tryParseOut = 0;
                string op = "";
                double valA = 0, valB = 0;

                //Will run in the event that token t is a double
                if(double.TryParse(t, out tryParseOut))
                {
                    if(operatorStack.Count == 0)
                    {
                        valueStack.Push(tryParseOut);
                        continue;
                    }

                    if(operatorStack.Peek().Equals("/") || operatorStack.Peek().Equals("*"))
                    {
                        valA = valueStack.Pop();
                        valB = tryParseOut;
                        op = operatorStack.Pop();
                        if (op.Equals("*"))
                        {
                            valueStack.Push(valA * valB);
                        }
                        else if(op.Equals("/"))
                        {
                            if(valB == 0)
                            {
                                throw new FormulaEvaluationException("Divided By Zero");
                            }
                            valueStack.Push(valA / valB);
                        }
                    }
                    else
                    {
                        valueStack.Push(tryParseOut);
                    }
                    continue;
                }

                //Will run when t is a variable
                if (isVariable(t))
                {
                    try
                    {
                        tryParseOut = lookup(normailzer(t));        ///Forgot to put in normalizer
                    }
                    catch
                    {
                        throw new FormulaEvaluationException("You didn't define a variabele: " + t);
                    }
                    if (operatorStack.Count != 0 && (operatorStack.Peek().Equals("/") || operatorStack.Peek().Equals("*")))
                    {
                        valA = valueStack.Pop();
                        valB = tryParseOut;
                        op = operatorStack.Pop();
                        if (op.Equals("*"))
                        {
                            valueStack.Push(valA * valB);
                        }
                        else if (op.Equals("/"))
                        {
                            if(valB == 0)
                            {
                                throw new FormulaEvaluationException("Divided by zero");
                            }
                            valueStack.Push(valA / valB);
                        }
                    }
                    else
                    {
                        valueStack.Push(tryParseOut);
                    }
                    continue;
                }

                //Will run when t is a + or -
                if(t.Equals("+") || t.Equals("-"))
                {
                    if(valueStack.Count < 2)
                    {
                        operatorStack.Push(t);
                        continue;
                    }

                    if(operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        op = operatorStack.Pop();
                        valB = valueStack.Pop();
                        valA = valueStack.Pop();
                        if (op.Equals("+"))
                        {
                            valueStack.Push(valA + valB);
                        }
                        else if (op.Equals("-"))
                        {
                            valueStack.Push(valA - valB);
                        }
                    }
                    operatorStack.Push(t);
                    continue;
                }

                //Will run when t is * or /
                if(t.Equals("/") || t.Equals("*"))
                {
                    operatorStack.Push(t);
                    continue;
                }

                //Will run when t is (
                if (t.Equals("("))
                {
                    operatorStack.Push(t);
                    continue;
                }

                //will run when t is )
                if (t.Equals(")"))
                {
                    if(operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        valB = valueStack.Pop();
                        valA = valueStack.Pop();
                        op = operatorStack.Pop();
                        if (op.Equals("+"))
                        {
                            valueStack.Push(valA + valB);
                        }
                        else if (op.Equals("-"))
                        {
                            valueStack.Push(valA - valB);
                        }
                    }

                    operatorStack.Pop();

                    if(operatorStack.Count != 0 && (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/")))
                    {
                        valB = valueStack.Pop();
                        valA = valueStack.Pop();
                        op = operatorStack.Pop();
                        if(op.Equals("*"))
                        {
                            valueStack.Push(valA * valB);
                        }
                        else if (op.Equals("/"))
                        {
                            if(valB == 0)
                            {
                                throw new FormulaEvaluationException("sorry but you divided by zero");
                            }
                            valueStack.Push(valA / valB);
                        }
                    }
                    continue;
                }
            }

            //brushes up the rest
            if(operatorStack.Count == 0)
            {
                return valueStack.Pop();
            }
            else
            {
                double valB = valueStack.Pop();
                double valA = valueStack.Pop();
                string op = operatorStack.Pop();
                if (op.Equals("+"))
                {
                    return valA + valB;
                }
                else if (op.Equals("-"))
                {
                    return valA - valB;
                }
            }
            return -1;
        }

        public ISet<string> GetVariables()
        {
            if (variableSet == null)
            {
                return new HashSet<string>();
            }
            return variableSet;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(stringFormula))
            {
                return stringFormula = "0";
            }
            return stringFormula;
        }


        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>

        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);


            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.

            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>

    public delegate double Lookup(string var);
    public delegate string Normalizer(string s);
    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>

    [Serializable]
    public class UndefinedVariableException : Exception
    {

        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>

        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }


    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>

    [Serializable]
    public class FormulaFormatException : Exception
    {

        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>

        public FormulaFormatException(String message) : base(message)
        {
        }
    }



    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {

        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}