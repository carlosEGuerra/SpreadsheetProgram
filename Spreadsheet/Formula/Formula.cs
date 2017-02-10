// Skeleton written by Joe Zachary for CS 3500, January 2017
// Appended by Ellen Brigance, February 2017

using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Saves the string passed into formula for use later.
        /// </summary>
        private string finalForm;

        /// <summary>
        /// The current Normalizer being used on the entire formula.
        /// </summary>
        private Normalizer finalNorm;

        /// <summary>
        /// The current Validator being used to impose variable rules on the entire formula.
        /// </summary>
        private Validator finalValid;

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

            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            //If a Formula is created with the original one-parameter constructor, the Normalizer
            //should be the identity function and the validator should be a method that always returns true.
            finalNorm = f => f;
            finalValid = f => true;

            //Check for tokens.
            //No tokens, throw FormulaFormatException.
            if (string.IsNullOrWhiteSpace(formula)) //CORRECTED FAILED TEST: TESTED FOR formula == null, failed.
            {
                throw new FormulaFormatException("No tokens given, formula format is invalid.");
            }

            Stack<string> formStack = new Stack<string>();
            finalForm = formula;
        
            //Deal with the formula, using evaluation of stacks.
            IEnumerable<string> s;
            IEnumerator<string> testToken;
            s = GetTokens(finalForm);//Convert string to tokens, one at a time. Deal with them on the stacks accordingly.

            testToken = s.GetEnumerator();

            while (testToken.MoveNext())//FIX LOOPING CONDITION TO END WITH ENUMERATOR
            {

                //  FIGURE OUT HOW TO ACCESS THE ELEMENTS OF AN IENUMERATOR

                string token = testToken.Current;

                double output = -1;

                //Left in in case we need this check later.
                //if (Double.TryParse((token), out output) && output < 0)//Check for negative numbers
                //{
                //    throw new FormulaFormatException("Invalid token found, negative numbers not allowed.");
                //}

                if (!Double.TryParse((token), out output))//Check for negative numbers
                {
                    output = -1;
                }

                //CHECKS FOR INVALID CHARACTERS (not an operator, a variable, or an open/closed paren)
                if ((!isOperator(token)) && !(output >= 0) && !Char.IsLetter(token[0]) && token != "(" && token != ")")//FIXED TEST 17 WITH EQUALS SIGN
                {
                    throw new FormulaFormatException("Invalid token. Symbol not allowed.");
                }
                //IF WE HAVE A DOUBLE.
                if (formStack.Count == 0 && output >= 0)
                {
                    formStack.Push(token);
                    continue;
                }

                if (formStack.Count > 0 && formStack.Peek() == "(" && !isOperator(token))//CORRECTED TEST 10, ADDED ISOPERATOR CHECK.
                {
                    formStack.Push(token);
                    continue;
                }
                if (formStack.Count > 0 && output >= 0 && (formStack.Peek() == "/" || formStack.Peek() == "*"
                    || formStack.Peek() == "+" || formStack.Peek() == "-"))
                {
                    formStack.Push(token);
                    continue;
                }
                if (output >= 0 && formStack.Count > 0 && !(isOperator(token) || formStack.Peek() == "("))
                {
                    throw new FormulaFormatException("Formula format invalid. Two operands in sequence.");
                }

                //IF WE HAVE AN OPEN PARENTESES.
                if (formStack.Count == 0 && token == "(")//Stack empty.
                {
                    formStack.Push(token);
                    continue;
                }
                if (formStack.Count > 0 && token == "(" && (formStack.Peek() == "("
                    || isOperator(formStack.Peek())))//Peek is a paren or operand.
                {
                    formStack.Push(token);
                    continue;
                }
                if (formStack.Count > 0 && token == "(" && (!isOperator(formStack.Peek())
                    || formStack.Peek() != "("))
                {
                    throw new FormulaFormatException("Formula invalid. Open parentheses adjacent to operand.");
                }
                //IF WE HAVE A CLOSED PARENTHESES.
                if (formStack.Count == 0 && token == ")")
                {
                    throw new FormulaFormatException("Formula invalid. Too many closing parenteses.");
                }
                if (formStack.Count > 0 && token == ")") //If the stack is full, pop until we see a "("
                {
                    Stack<String> tempStack = new Stack<string>();
                    while (formStack.Count > 0)
                    {
                        if (formStack.Peek() != "(")
                        {
                            tempStack.Push(formStack.Pop());
                        }

                        if (formStack.Count == 0 || formStack.Peek() == "(")// FIXED TEST 2 AND 3: NEEDED TO BREAK BEFORE PEEKING AT AN EMPTY STACK.
                        {
                            break;
                        }
                    }

                    if (formStack.Count == 0)//We never saw a "("
                    {
                        throw new FormulaFormatException("Formula invalid. Too many closing parenteses.");
                    }
                    if (formStack.Peek() == "(")//We found a "(", pop it, put everything back on the stack.
                    {
                        formStack.Pop();
                        while (tempStack.Count > 0) //
                        {
                            formStack.Push(tempStack.Pop());
                        }

                        continue;
                    }
                }
                //IF WE HAVE AN OPERATOR.
                if (isOperator(token) && formStack.Count == 0)
                {
                    throw new FormulaFormatException("Formula invalid. Operator without operands.");
                }
                if (isOperator(token) && formStack.Count > 0 && isOperator(formStack.Peek()))//Stack top is also operand.
                {
                    throw new FormulaFormatException("Formula invalid. Two adjacent operands.");
                }
                if (isOperator(token) && formStack.Count > 0 && formStack.Peek() == "(")
                {
                    throw new FormulaFormatException("Formula invalid. Operator without operand.");
                }
                if (isOperator(token) && formStack.Count > 0)//At this point, we assume we have a valid operand.
                {
                    formStack.Push(token);
                    continue;
                }
                //IF WE HAVE A VARIABLE.
                if (formStack.Count == 0 && Char.IsLetter(token[0]))
                {
                    formStack.Push(token);
                    continue;
                }
                if (Char.IsLetter(token[0]) && formStack.Count > 0 && (isOperator(formStack.Peek()) || formStack.Peek() == "("))//Have an operator or an open paren.
                {
                    if (isOperator(formStack.Peek()))
                    {
                        formStack.Push(token);
                        continue;
                    }

                    //formStack.Push(token);//May need this in the future.
                    //continue;
                }
                if (Char.IsLetter(token[0]) && formStack.Count > 0 && (!isOperator(formStack.Peek()) || formStack.Peek() != "("))//Have no operator or open paren.
                {
                    throw new FormulaFormatException("Formula invalid. Two operands adjacent.");
                }

                if (Char.IsLetter(token[0]) && formStack.Count > 0 && (formStack.Peek() == "/" || formStack.Peek() == "*"
                  || formStack.Peek() == "+" || formStack.Peek() == "-"))
                {
                    formStack.Push(token);
                    continue;
                }

            }

            if (formStack.Count > 0 && isOperator(formStack.Peek()))//CORRECTED FOR TEST 15.
            {
                throw new FormulaFormatException("Formula cannot end in operator.");
            }
            if (formStack.Count > 0 && formStack.Contains("("))
            {
                throw new FormulaFormatException("Formula invalid, not enough closing parentheses.");
            }

        }

        /// <summary>
        /// Formula constructor takes three parameters: a formula string (like usual),
        /// a Normalizer, and a Validator (in that order).  The purpose of a Normalizer is
        /// to convert variables into a canonical form.  The purpose of a Validator is to 
        /// impose extra restrictions on the validity of a variable, beyond the ones 
        /// already built into the Formula definition. 
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="n"></param>
        /// <param name="v"></param>
        public Formula(string f, Normalizer N, Validator V)
        {
            if(f == null || N == null || V == null)
            {
                throw new ArgumentNullException();
            }


            Formula formCheck = new Formula(f); //Will perform all checks to make sure formula is syntactically correct.

          finalNorm = N;
          finalValid = V;

          finalForm = formCheck.finalForm;
            string fixedString = ""; 

            //Check to make sure the Normalizer and Validator don't invalidate our formula.
            if (N != null)
            {

                IEnumerable<string> s = GetTokens(finalForm);//Convert valid string to tokens.
                IEnumerator<string> testToken = s.GetEnumerator();

                while (testToken.MoveNext())
                {
                    string cur = testToken.Current;

                    if (IsValidVariable(cur))//If the current token we're looking at is a variable,
                    {
                        string normCur = finalNorm(cur);//Normalize the current token.
                        if (!IsValidVariable(normCur))//If token is not a legal variable according to the standard Formula rules
                        {
                            throw new FormulaFormatException("Normalizer resulted in illegal variable.");
                        }
                        if (!finalValid(normCur))
                        {
                            throw new FormulaFormatException("Validator found illegal variable.");
                        }
                        if (IsValidVariable(normCur))
                        {
                            cur = normCur;
                        }
                    }

                    fixedString = fixedString + cur; //Rebuild the global formula.
                }

                //Finished lookin through all tokens. If no exceptions were thrown N(x) should be used in place of x in the constructed formula.
                if (!String.IsNullOrEmpty(fixedString))
                {
                    finalForm = fixedString;
                }
            }
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

            Stack<string> opStack = new Stack<string>();
            Stack<Double> valStack = new Stack<Double>();

            IEnumerable<string> s;
            IEnumerator<string> testToken;
            s = GetTokens(finalForm);//Convert string to tokens, one at a time. Deal with them on the stacks accordingly.
            testToken = s.GetEnumerator();

            //testToken.Reset(); //Move the iterator back to the beginning of the formula.
            while (testToken.MoveNext())
            {
                string token = testToken.Current;

                double output = -1;

                if (!Double.TryParse((token), out output))//Check for negative numbers
                {
                    output = -1;
                }

                //t IS A DOUBLE

                //If * or / is at the top of the operator stack, pop the value stack, pop the operator stack,
                //and apply the popped operator to t and the popped number. Push the result onto the value stack. 
                if (output >= 0 && opStack.Count > 0 && (opStack.Peek() == "*" || opStack.Peek() == "/"))
                {
                    double total = 0;
                    if (opStack.Peek() == "*")
                    {
                        total = output * valStack.Pop();
                    }
                    if (opStack.Peek() == "/")
                    {
                        if (output == 0)//ASK ABOUT THIS. WAS INFORMED BY TA TO DO A TRY CATCH.
                        {
                            throw new FormulaEvaluationException("Cannot divide by zero.");
                        }

                        total = valStack.Pop() / output; //Check for divide by zero error. ***ASK TA IF WE HANDLE THIS**

                    }

                    opStack.Pop();
                    valStack.Push(total);
                    continue;
                }
                //Otherwise, push t onto the value stack
                if (valStack.Count >= 0 && output >= 0)
                {
                    valStack.Push(output);
                    continue;
                }

                //t IS A VARIABLE

                //Proceed as in the previous case, using the looked-up value of t in place of t
                if (Char.IsLetter(token[0]) && opStack.Count > 0 && (opStack.Peek() == "*" || opStack.Peek() == "/"))
                {
                    double total = 0;
                    double numVal;
                    try
                    {
                        numVal = lookup(token);
                   
                    }
                    catch
                    {
                        throw new FormulaEvaluationException("No lookup value given.");
                    }
                    if (opStack.Peek() == "*")
                    {
                        total = numVal * valStack.Pop();
                    }
                    if (opStack.Peek() == "/")
                    {
                                            
                       if(numVal == 0)
                        {
                            throw new FormulaEvaluationException("Division by zero.");
                        }

                        total = valStack.Pop() / numVal;
                    }
                    opStack.Pop();
                    valStack.Push(total);
                    continue;
                }
                //Otherwise, push t onto the value stack
                if (Char.IsLetter(token[0]))
                {
                    double numVal;
                    try
                    {
                        numVal = lookup(token);
                  
                    }
                    catch
                    {
                        throw new FormulaEvaluationException("No lookup value given.");
                    }
                    valStack.Push(numVal);
                    continue;
                }

                //t IS A "+" OR "-"

                //If + or - is at the top of the operator stack, pop the value stack twice and the operator stack once.
                //Apply the popped operator to the popped numbers. Push the result onto the value stack.
                if ((token == "+" || token == "-") && opStack.Count > 0)
                {
                    double val1;
                    double val2;
                    double total;
                    if (opStack.Peek() == "+")
                    {
                        val1 = valStack.Pop();
                        val2 = valStack.Pop();
                        total = val2 + val1;
                        valStack.Push(total);
                    }

                    if (opStack.Peek() == "-")
                    {
                        val1 = valStack.Pop();
                        val2 = valStack.Pop();
                        total = val2 - val1;
                        valStack.Push(total);
                    }

                    if (opStack.Peek() == "-" || opStack.Peek() == "+")
                    {
                        opStack.Pop();
                    }


                    //Whether or not you did the first step, push t onto the operator stack
                    opStack.Push(token);
                    continue;
                }

                if ((token == "+" || token == "-") && opStack.Count == 0)
                {
                    //Whether or not you did the first step, push t onto the operator stack
                    opStack.Push(token);
                    continue;
                }



                //t IS A "/" OR "*" 
                if (token == "/" || token == "*") //Push t onto the operator stack
                {
                    opStack.Push(token);
                    continue;
                }

                //t IS A "("
                if (token == "(")
                {
                    opStack.Push(token);//Push t onto the operator stack
                    continue;
                }

                //t IS A ")"
                if (token == ")")
                {
                    //If + or - is at the top of the operator stack, pop the value stack twice and the operator stack once.
                    //Apply the popped operator to the popped numbers. Push the result onto the value stack.
                    if (opStack.Count > 0 && (opStack.Peek() == "+" || opStack.Peek() == "-"))
                    {
                        double val1 = valStack.Pop();
                        double val2 = valStack.Pop();
                        double total;
                        if (opStack.Peek() == "+")
                        {
                            total = val2 + val1;
                            valStack.Push(total);
                        }

                        if (opStack.Peek() == "-")
                        {
                            total = val2 - val1;
                            valStack.Push(total);
                        }

                        opStack.Pop();
                    }

                    //Whether or not you did the first step, the top of the operator stack will be a (.Pop it.
                    if (opStack.Count > 0)
                    {
                        opStack.Pop();
                    }

                    //After you have completed the previous step, if *or / is at the top of the operator stack, 
                    //pop the value stack twice and the operator stack once. Apply the popped operator to the popped numbers. 
                    //Push the result onto the value stack.
                    if (opStack.Count > 0 && (opStack.Peek() == "/" || opStack.Peek() == "*"))
                    {
                        double val1 = valStack.Pop();
                        double val2 = valStack.Pop();
                        double total;
                        if (opStack.Peek() == "*")
                        {
                            total = val2 * val1;
                            valStack.Push(total);
                        }

                        if (opStack.Peek() == "/")
                        {
                            if (val1 == 0)
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero.");//FIXED FOR TEST 18 AND 18A: CHECK FOR DIVIDE BY ZERO.
                            }
                            total = val2 / val1;
                            valStack.Push(total);
                        }

                        opStack.Pop();
                    }
                    continue;
                }
            }

            //Operator stack is empty
            //Value stack will contain a single number.  Pop it and report as the value of the expression
            double value = -1;
            if (opStack.Count == 0)
            {
                return valStack.Pop();
            }

            //Operator stack is not empty
            //There will be exactly one operator on the operator stack, and it will be either + or -. 
            //There will be exactly two values on the value stack. Apply the operator to the two values
            //and report the result as the value of the expression.

            if (opStack.Count > 0)
            {
                double val1 = valStack.Pop();
                double val2 = valStack.Pop();
                if (opStack.Peek() == "+")
                {
                    value = val2 + val1;
                    valStack.Push(value);
                }

                if (opStack.Peek() == "-")
                {
                    value = val2 - val1;
                    valStack.Push(value);
                }

                opStack.Pop();
            }

            return valStack.Pop();
        }

        /// <summary>
        /// Returns a string version of the Formula (in normalized form).
        /// </summary>
        public override string ToString()
        {

            return finalNorm(finalForm);
        }

        /// <summary>
        /// Returns an ISet<string> that contains each distinct 
        /// variable (in normalized form) that appears in the Formula.
        /// </summary>
        public ISet<string> GetVariables()
        {

            ISet<string> set = new HashSet<string>();

            IEnumerable<string> s = GetTokens(finalForm);//Convert valid string to tokens.
            IEnumerator<string> testToken = s.GetEnumerator();

            while (testToken.MoveNext())
            {
                string cur = testToken.Current;
                if (IsValidVariable(cur))//If we hav a valid variable
                {
                    if(finalNorm != null)//See if we can normalize it.
                    {
                        cur = finalNorm(cur);
                    }
                    set.Add(cur);//Then add it to our list.
                }
            }

            return set;
        }

        /// <summary>
        /// Returns true if string s is a valid variable according to our formula guidelines.
        /// </summary>
        /// <param name="s"></param>
        private bool IsValidVariable(string s)
        {
            // Pattern to identify a valid variable.
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            Regex regVar = new Regex(varPattern);

            

            string check = regVar.Match(s).ToString();
            if (check != s)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if we are looking at a valid operand. False, otherwise.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isOperator(string s)
        {
            if (s == "/" || s == "*" || s == "+" || s == "-")
            {
                return true;
            }
            return false;
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
    /// <param name="var"></param>
    public delegate double Lookup(string var);

    /// <summary>
    /// The purpose of a Normalizer is to convert variables into a canonical form.
    /// </summary>
    /// <param name="s"></param>
    public delegate string Normalizer(string s);

    /// <summary>
    /// The purpose of a Validator is to impose extra restrictions on the validity of a variable
    /// </summary>
    /// <param name="s"></param>
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