using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prolog;

/*
 * Example of how to use Prolog from within another program.
 * 
 * The PrologEngine class offers two public methods for this purpose:
 * - GetAllSolutions
 * - GetAllSolutionsXml ()
 * 
 *   public SolutionSet GetAllSolutions (string sourceFileName, string query, int maxSolutionCount)
 *   public SolutionSet GetAllSolutions (string sourceFileName, string query)
 * 
 * The purpose of this method is to find all solutions for query 'query', with a maximum number
 * of solutions 'maxSolutionCount'. If the value of this parameter is <= 0, all solutions will
 * be determined.
 * 
 * 'sourceFileName' may contain the name of a Prolog source file that is to be consulted prior
 * to the execution of the query. Enter a null-value if no such source file is present.
 * 
 * The solutions that are found, are collected in an instance of the public 'SolutionSet' class.
 * A SolutionSet contains a list of variables of the 'Solution' class type, where each solution
 * contains a list of variables of the 'Variable' class type. A Variable contain the name, data type
 * and value of the Prolog variable that got instantiated as a result of executing the query.
 * 
 * 'SolutionSet' contains three other properties:
 * - string Query  : the query you provided as parameter
 * - bool Success  : true if the query succeeded, false if it did not
 * - int Count     : the number of Solutions in SolutionSet. Notice that this number may be zero
 *                   also if Success = true, i.e. the query succeeded, but no variables are available
 *                   for output.
 * - bool HasError : true if a runtime error occurred. A test on this is left out in the examples
 *                   1 and 2 below, but this obviously should always be present.
 * - string ErrMsg : the text of the error message.             
 *                  
 * In addition, 'SolutionSet' exports a ToString() method showing the above information.
 * 
 *   public string GetAllSolutionsXml (string sourceFileName, string destinFileName, string query, int maxSolutionCount)
 *   public string GetAllSolutionsXml (string sourceFileName, string destinFileName, string query)
 *   
 * These method are similar to there GetAllSolutions counterparts. The main difference is that
 * the solution set is stored in an XML-structure. This structure is written to 'destinFileName' if
 * such a name is provided, and returned as method result if 'destinFileName' is null.
 * If a runtime error occurs, the text of the error will be stored in a node named <error>.
 * 
 * Result of running the program below:
 
  Example 1

  Solution 1
  P (atom) = peter
  N (number) = 7
  Solution 2
  P (atom) = ann
  N (number) = 5
  Solution 3
  P (atom) = ann
  N (number) = 6
  Solution 4
  P (atom) = pat
  N (number) = 8
  Solution 5
  P (atom) = tom
  N (number) = 5

  Example 2

  <?xml version="1.0" encoding="Windows-1252"?>
  <solutions success="true">
    <query>age(P,N)</query>
    <solution>
      <variable name="P" type="atom">peter</variable>
      <variable name="N" type="number">7</variable>
    </solution>
    <solution>
      <variable name="P" type="atom">ann</variable>
      <variable name="N" type="number">5</variable>
    </solution>
    <solution>
      <variable name="P" type="atom">ann</variable>
      <variable name="N" type="number">6</variable>
    </solution>
    <solution>
      <variable name="P" type="atom">pat</variable>
      <variable name="N" type="number">8</variable>
    </solution>
    <solution>
      <variable name="P" type="atom">tom</variable>
      <variable name="N" type="number">5</variable>
    </solution>
  </solutions>

  Example 3

  An error occurred:
  *** input string: line 1 position 7
  age(P,)))))))))).
  *** Unexpected symbol: ")"
  *** Expected one of: (, <Identifier>, <IntLiteral>, <RealLiteral>, <ImagLiteral>
  , <StringLiteral>, **, /\, <<, /, spy, xor, -, ?=, @>=, @>, >=, >, =\=, =:, ;, :
  -, '{}', ^, mod, >>, //, nospy, \/, #, :, =.., @=<, @<, =<, <, =:=, :=, ->, \+,
  once, help, =, ==, is, \==, \=, not, <Atom>, <Anonymous>, <CutSym>, [, {, <ListP
  atternOpen>, TRY, {=, =}, <AltListOpen>, <AltListClose>, <VerbatimStringLiteral>

  Press any key to exit
 */

namespace PLx
{
  class Program
  {
    static void Main (string [] args)
    {
      PrologEngine e = new PrologEngine ();
      // Example 1 -- the age/2 predicate is a builtin example; defined in Bootstrap.cs
      
      Console.WriteLine ("Example 1");
      Console.WriteLine ();

      SolutionSet ss = e.GetAllSolutions (null, "age(P,N)");

      if (ss.Success)
      {
        for (int i = 0; i < ss.Count; i++ ) // or: foreach (Solution s in ss.NextSolution)
        {
          Solution s = ss [i];
          Console.WriteLine ("Solution {0}", i+1);

          foreach (Variable v in s.NextVariable)
            Console.WriteLine (string.Format ("{0} ({1}) = {2}", v.Name, v.Type, v.Value));
        }
      }
      else
        Console.WriteLine ("Failure");

      // Example 2 -- xml generation

      Console.WriteLine ("Example 2");
      Console.WriteLine ();

      string result = e.GetAllSolutionsXml (null, null, "age(P,N)");
      Console.WriteLine (result);
      Console.WriteLine ();

      // Example 3 -- error

      Console.WriteLine ("Example 3");
      Console.WriteLine ();
      
      ss = e.GetAllSolutions (null, "age(P,))))))))))");

      if (ss.HasError)
        Console.WriteLine ("An error occurred: {0}", ss.ErrMsg);

      Console.WriteLine ("Press any key to exit");
      Console.ReadKey ();
    }
  }
}
