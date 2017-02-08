// Skeleton implementation written by Joe Zachary for CS 3500, January 2017.
// Student: Carlos Guerra, u0847821

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    /// 
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dependent; //a dependent with a list of dependees
        private Dictionary<string, HashSet<string>> dependee; //a dependee with a list of dependents

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependee = new Dictionary<string, HashSet<string>>();
            dependent = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
               int dependentCount = 0;
                foreach(string s in dependee.Keys)
                {
                    dependentCount += dependee[s].Count;
                }
                return dependentCount;
            }   
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (!dependee.ContainsKey(s))
            {
                return false;
            }
            return dependee[s].Count != 0;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (!dependent.ContainsKey(s))
            {
                return false;
            }
            return dependent[s].Count != 0;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<string> dependentList = new HashSet<string>();      //added a check to make sure that the key s is contained in the dependee list
            if (!dependee.ContainsKey(s))
            {
                return dependentList;
            }
            else
            {
                dependentList = dependee[s];
                return dependentList;
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<string> dependeeList = new HashSet<string>();       //added a chekc to make sure that the key s is contained in the dependent list
            if (!dependent.ContainsKey(s))
            {
                return dependeeList;
            }
            else
            {
                dependeeList = dependent[s];
                return dependeeList;
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (dependee.ContainsKey(s))
            {
                dependee[s].Add(t);
            }
            if (dependent.ContainsKey(t))       //deleted an else that fixed most of my code
            {
                dependent[t].Add(s);
            }

            if (!dependee.ContainsKey(s))
            {
                dependee.Add(s, new HashSet<string>() { t });
                if (!dependent.ContainsKey(t))
                {
                    dependent.Add(t, new HashSet<string>() { s });
                }
            }
            else if (!dependent.ContainsKey(t))
            {
                dependent.Add(t, new HashSet<string>() { s });
            }         
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if(dependee.ContainsKey(s) && dependee[s].Contains(t))      //added a check to ensure that the dependees contain s and t is contained in the hashset
            {
                dependee[s].Remove(t);
                dependent[t].Remove(s);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependee.ContainsKey(s))
            {
                foreach (string str in dependee[s])
                {
                    dependent[str].Remove(s);
                }
                dependee[s] = new HashSet<string>();
                foreach (string dependents in newDependents)
                {
                    this.AddDependency(s, dependents);
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (dependent.ContainsKey(t))
            {
                foreach (string str in dependent[t])
                {
                    dependee[str].Remove(t);
                }
                dependent[t] = new HashSet<string>();
                foreach (string dependees in newDependees)
                {
                    this.AddDependency(dependees, t);
                }
            }
        }
    }
}