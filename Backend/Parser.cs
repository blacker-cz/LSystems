using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LSystems.Backend
{
    public class Parser
    {
        /// <summary>
        /// Grammar axiom
        /// </summary>
        private string _axiom;

        /// <summary>
        /// Set of rules
        /// </summary>
        private Dictionary<char, string> _rules;

        /// <summary>
        /// Number of maximum iteration
        /// </summary>
        private uint _iterations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="axiom">Grammar axiom</param>
        /// <param name="rules">Rules (one rule per line)</param>
        /// <param name="iterations">Number of maximum iterations</param>
        public Parser(string axiom, string rules, uint iterations)
        {
            _axiom = axiom.Trim();
            _iterations = iterations;

            _rules = new Dictionary<char, string>();

            // explode rules to dictionary
            using (StringReader reader = new StringReader(rules))
            {
                string line;
                string[] splits;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    splits = line.Split('=');
                    if (splits.Length != 2)
                        throw new ArgumentException("Invalid set of rules.");

                    _rules[splits[0][0]] = splits[1];
                }
            }
        }

        /// <summary>
        /// Expand grammar
        /// </summary>
        /// <returns>Expanded grammar string</returns>
        public string Expand()
        {
            // initialize output string
            string output = _axiom;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _iterations; i++)
            {
                sb.Clear();

                // for every character in input string check for expand rules
                foreach (char ch in output)
                {
                    // rule found -> expand
                    if (_rules.ContainsKey(ch))
                    {
                        sb.Append(_rules[ch]);
                    }
                    else    // rule not found -> copy
                    {
                        sb.Append(ch);
                    }
                }
                output = sb.ToString();
            }

            return output;
        }
    }
}
