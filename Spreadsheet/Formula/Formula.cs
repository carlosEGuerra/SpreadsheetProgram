﻿// Skeleton written by Joe Zachary for CS 3500, January 2017
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
    public class Formula
    {
        private IEnumerable<string> enumForm;
        private string[] symbolList;
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
        public Formula(String formula)
        {
            //Checks to see if there will be at least 1 token
            if (string.IsNullOrEmpty(formula) || formula.Length == 0)
            {
                throw new FormulaFormatException("Sorry, but to construct a formula we need input");
            }

            //creates a list of valid symbols
            symbolList = new string[6];
            symbolList[0] = "+";
            symbolList[1] = "-";
            symbolList[2] = "*";
            symbolList[3] = "/";
            symbolList[4] = "(";
            symbolList[5] = ")";

            //converts the formula string into tokens
            enumForm = GetTokens(formula);
            //gets the first token
            string firstToken = enumForm.First();
            //gets the second token
            string lastToken = enumForm.Last();

            //checkes if first token of a formula is a number, a variable, or an opening parenthesis.
            double num;
            if(!firstToken.Equals("("))
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
                    if (!parenthesisStack.Peek().Equals("("))
                    {
                        throw new FormulaFormatException("Your missing a parenthesis");
                    }
                    else
                    {
                        parenthesisStack.Pop();
                        closingParen++;
                    }
                }
                //checks if the number of opening parenthesis is smaller then the number of closing parenthesis
                if(closingParen > openeningParen)
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
                tokenPrior = token;
            }
            //Checks if the correct number of parenthesis is used
            if(parenthesisStack.Count != 0)
            {
                throw new FormulaFormatException("Sorry but you are missing some parenthesis");
            }
        }

        private bool isVariable(string token)
        {
            char[] charArr = token.ToCharArray();
            if(!char.IsLetter(charArr[0]))
            {
                return false;
            }
            for(int i = 1; i < charArr.Count(); i++)
            {
                double testDouble = 0;
                if(!(Char.IsLetter(charArr[i]) || double.TryParse(charArr[i].ToString(), out testDouble)))
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
            //double evaluatedValue = 0;
            //initiallizes both the value and operator stacks
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            //organizes the formula into the two stacks
            foreach (string token in enumForm)
            {
                double temp;
                if (isVariable(token))
                {
                    try
                    {
                        temp = lookup(token);
                        valueStack.Push(temp);
                    }
                    catch
                    {
                        throw new FormulaEvaluationException("Hello");
                    }
                    
                }
                else if (double.TryParse(token, out temp))
                {
                    valueStack.Push(temp);
                }
                else if (symbolList.Contains(token) || token.Equals("(") || token.Equals(")"))
                {
                    operatorStack.Push(token);
                }
            }


            //Operates according to the stacks
            double value = 0;
            string operatorVal = "";
            while(operatorStack.Count != 0)
            {
                operatorVal = operatorStack.Pop();
                switch (operatorVal)
                {
                    case ("+"):
                        double b = valueStack.Pop();
                        double a = valueStack.Pop();
                        value = a + b;
                        valueStack.Push(value);
                        break;
                    case ("-"):
                        b = valueStack.Pop();
                        a = valueStack.Pop();
                        value = a - b;
                        valueStack.Push(value);
                        break;
                    case ("*"):
                        b = valueStack.Pop();
                        a = valueStack.Pop();
                        value = a * b;
                        valueStack.Push(value);
                        break;
                    case ("/"):
                        b = valueStack.Pop();
                        a = valueStack.Pop();
                        if(b == 0)
                        {
                            throw new DivideByZeroException("Sorry but a divide by zero has been detected");
                        }
                        value = a / b;
                        valueStack.Push(value);
                        break;
                    case ("("):
                        break;
                    case (")"):
                        if(operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                        {
                            b = valueStack.Pop();
                            a = valueStack.Pop();
                            string temp = operatorStack.Pop();
                            if(temp.Equals("+"))
                            {
                                value = a + b;
                                valueStack.Push(value);
                                break;
                            }
                            else
                            {
                                value = a - b;
                                valueStack.Push(value);
                                break;
                            }
                        }

                        //operatorStack.Pop();

                        if(operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                        {
                            b = valueStack.Pop();
                            a = valueStack.Pop();
                            string temp = operatorStack.Pop();
                            if (temp.Equals("*"))
                            {
                                value = a * b;
                                valueStack.Push(value);
                                break;
                            }
                            else
                            {
                                if(b == 0)
                                {
                                    throw new DivideByZeroException("Sorry but a divide by zero has been detected");
                                }
                                value = a / b;
                                valueStack.Push(value);
                                break;
                            }
                        }
                        break;
                }
            }
            return valueStack.Pop();
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