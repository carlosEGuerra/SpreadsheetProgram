// Skeleton implementation written by Joe Zachary for CS 3500, January 2017.
//Appended by Ellen Brigance, January 28, 2017

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
    public class DependencyGraph
    {
        /// <summary>
        ///  The represnetation of the dependeees of a dependency graph as defined above.
        ///  In this dictionary representation, the key is the dependee and the value is a 
        ///  HashSet contained within each key is its list of dependents.
        /// </summary>
        private Dictionary<string, HashSet<string>> dependees;

        /// <summary>
        /// The represnetation of the dependents of a dependency graph as defined above.
        /// In this dictionary representation, the key is the dependent and the value is a 
        /// HashSet contained within each key is its list of dependees.
        /// </summary>
        private Dictionary<string, HashSet<string>> dependents;

        /// <summary>
        /// Keeps track of the number of dependencies we have whenever we alter the list.
        /// </summary>
        private int dependencyCount = 0;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependees = new Dictionary<string, HashSet<string>>();
            dependents = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// Throws ArgumentNullException if null parameter passed in.
        /// </summary>
        public DependencyGraph(DependencyGraph dg)
        {
            if(dg == null)
            {
                throw new ArgumentNullException();
            }


            //CORRECTED FOR REGRADE: Needed to copy dependents and dependees over instead of referencing them.
            this.dependees = new Dictionary<string, HashSet<string>>();
            this.dependents = new Dictionary<string, HashSet<string>>();

            //Add each Dependee of of dg
            foreach (KeyValuePair<string, HashSet<string>> p in dg.dependees)
            {
                this.ReplaceDependents(p.Key, p.Value);
            }

            //Add each Dependent of dg
            foreach (KeyValuePair<string, HashSet<string>> p in dg.dependents)
            {
                this.ReplaceDependees(p.Key, p.Value);
            }

        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                return dependencyCount;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// Throws an ArgumentNullException when passed null parameter is passed.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            if (!dependees.ContainsKey(s))//If the string is not in our list, return false.
            {
                return false;
            }

            if (dependees[s].Count > 0)//Check the list under the Dependency.
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// Throws ArgumentNullException if null parameter passed in.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            if (!dependents.ContainsKey(s))//If the string is not in our list, return false.
            {
                return false;
            }

            if (dependents[s].Count > 0)//If the dependent is in the list, check if anything still depends on it.
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// Throws ArgumentNullException if null parameter passed in.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            if (dependees.ContainsKey(s))
            {
                IEnumerable<string> fullList = dependees[s];
                return fullList;
            }

            //If s does not exist in the context, return an empty list.
            List<string> emptyList = new List<string>();
            IEnumerable<string> noList = emptyList;
            return noList;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// Throws ArgumentNullException if null parameter passed in.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }

            if (dependents.ContainsKey(s))
            {
                IEnumerable<string> fullList = dependents[s];
                return fullList;
            }

            //If s does not exist in the context, return an empty list.
            List<string> emptyList = new List<string>();
            IEnumerable<string> noList = emptyList;
            return noList;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Throws ArgumentNullException if null parameter passed in.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            bool countNotIncremented = true;
            if (s == null || t == null)
            {
                    throw new ArgumentNullException();
            }

            //DEAL WITH THE DEPENDEES FIRST.
            if (!dependees.ContainsKey(s))//If we don't have the key s yet
            {
                dependees.Add(s, new HashSet<string>()); //Add s to the set.
                //dependees[s].Add(t); //Give it a dependent.
            }

            if (dependees.ContainsKey(s)) //If we have the key s already
            {
                if (!dependees[s].Contains(t)) //If it doesn't have t already
                {
                    dependees[s].Add(t); //Give it a dependent.
                    dependencyCount++;//Add the dependency ONLY ONCE.
                    countNotIncremented = false;
                }
            }

            //THEN DEAL WITH THE DEPENDENTS
            if (!dependents.ContainsKey(t)) //if we don't have t already.
            {
                dependents.Add(t, new HashSet<string>()); //Add t to the set.
                dependents[t].Add(s); //Give it a dependee.
            }

            if (dependents.ContainsKey(t)) //If we have the key t already
            {
                if (!dependents[t].Contains(s)) //If it doesn't have s already
                {
                    dependents[t].Add(s); //Give it a dependent.
                    if (countNotIncremented)
                    {
                        dependencyCount++;
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Throws ArgumentNullException if null parameter passed in.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            bool countNotdecremented = true;
            if (s == null || t == null)
            {
                    throw new ArgumentNullException();
            }

            //If we don't have s or t, do nothing.
            if (!dependees.ContainsKey(s) && !dependents.ContainsKey(t))
            {
                return;
            }

            //DEAL WITH THE DEPENDEES FIRST.
            if (dependees.ContainsKey(s))//If we have the key s
            {
                if (dependees[s].Contains(t))//If s has t as dependent
                {
                    dependees[s].Remove(t);
                    dependencyCount--;
                    countNotdecremented = false;

                }
            }

            //THEN DEAL WITH THE DEPENDENTS
            if (dependents.ContainsKey(t)) //if we have t already.
            {
                if (dependents[t].Contains(s))//If t depends on s
                {
                    dependents[t].Remove(s);
                    if (countNotdecremented)
                    {
                        dependencyCount--;
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Throws ArgumentNullException if null parameter passed in.
        /// Throws ArgumentNullException if null string is encourntered in newDependents.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null)
            {
               throw new ArgumentNullException();
            }

            IEnumerator<string> depEnumerator = newDependents.GetEnumerator();
            string t;//New dependents to be added to s.

            if (dependees.ContainsKey(s))
            {
                dependencyCount = dependencyCount - dependees[s].Count;//Decrement the master list.
                foreach(string r in dependees[s])//For each dependent of s, we must remove s from the list of dependees.
                {
                    if(dependents[r] != null && dependents[r].Contains(s))
                    {
                        dependents[r].Remove(s);
                    }
                }
                dependees[s].Clear();//Remove all dependents from s.
            }

            while (depEnumerator.MoveNext())
            {
                t = depEnumerator.Current;
                if (t == null)
                {
                    throw new ArgumentNullException();
                }
                this.AddDependency(s, t); //Add all dependents to s.
            }

        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Throws ArgumentNullException if null parameter passed in.
        /// Throws ArgumentNullException if null string is encourntered in newDependees.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null || newDependees == null)
            {
                throw new ArgumentNullException();
            }

            IEnumerator<string> depEnumerator = newDependees.GetEnumerator();
            string s;//New dependents to be added to s.

            if (dependents.ContainsKey(t))
            {
                dependencyCount = dependencyCount - dependents[t].Count;
                foreach (string r in dependents[t])//For each dependee of t, we must remove t from the list of dependents.
                {
                    if (dependees[r] != null && dependees[r].Contains(t))
                    {
                        dependees[r].Remove(t);
                    }
                }
                dependents[t].Clear();//Remove all dependents from s. 
            }

            while (depEnumerator.MoveNext())
            {
                s = depEnumerator.Current;
                if (s == null)
                {
                    throw new ArgumentNullException();
                }

                this.AddDependency(s, t); //Add all dependeess to t.
            }
            return;
        }
    }
}
