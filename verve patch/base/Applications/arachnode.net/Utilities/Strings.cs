#region License : arachnode.net

// // Copyright (c) 2013 http://arachnode.net, arachnode.net, LLC
// //  
// // Permission is hereby granted, upon purchase, to any person
// // obtaining a copy of this software and associated documentation
// // files (the "Software"), to deal in the Software without
// // restriction, including without limitation the rights to use,
// // copy, merge and modify copies of the Software, and to permit persons
// // to whom the Software is furnished to do so, subject to the following
// // conditions:
// // 
// // LICENSE (ALL VERSIONS/EDITIONS): http://arachnode.net/r.ashx?3
// // 
// // The above copyright notice and this permission notice shall be
// // included in all copies or substantial portions of the Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// // OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// // WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// // FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// // OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arachnode.Utilities
{
    public class MarkovChainNode : Dictionary<string, MarkovChainNode>
    {
        public int ChainCount { get; set; }
        public MarkovChainNode Parent { get; set; }
    }

    public class Strings
    {
        private MarkovChainNode _markovChain = new MarkovChainNode();

        public MarkovChainNode GenerateMarkovChain(string input)
        {
            MarkovChainNode currentMarkovChainNode = null;
            bool isFirstWord = true;

            foreach (string word in UserDefinedFunctions.ExtractWords(input, true, false))
            {
                if (isFirstWord)
                {
                    if (!_markovChain.ContainsKey(word))
                    {
                        _markovChain.Add(word, new MarkovChainNode());
                    }

                    _markovChain.ChainCount++;

                    currentMarkovChainNode = _markovChain[word];
                    currentMarkovChainNode.Parent = _markovChain;

                    isFirstWord = false;

                    continue;
                }

                currentMarkovChainNode.ChainCount++;

                if (!currentMarkovChainNode.ContainsKey(word))
                {
                    currentMarkovChainNode.Add(word, new MarkovChainNode());
                }

                MarkovChainNode parent = currentMarkovChainNode;
                currentMarkovChainNode = currentMarkovChainNode[word];
                currentMarkovChainNode.Parent = parent;
            }

            return _markovChain;
        }

        public string GetMostCommonChain(string chainStart, bool decrementChainCount)
        {
            if (_markovChain.Count != 0)
            {
                MarkovChainNode startingNode = _markovChain;

                if (!string.IsNullOrEmpty(chainStart))
                {
                    foreach (string word in UserDefinedFunctions.ExtractWords(chainStart, true, false))
                    {
                        if (startingNode.ContainsKey(word))
                        {
                            startingNode = startingNode[word];
                        }
                        else
                        {
                            if (startingNode == _markovChain)
                            {
                                return null;
                            }

                            break;
                        }
                    }
                }

                /**/

                StringBuilder mostCommonChain = new StringBuilder();

                if (!string.IsNullOrEmpty(chainStart))
                {
                    mostCommonChain.Append(chainStart + " ");
                }

                /**/

                KeyValuePair<string, MarkovChainNode> keyValuePair = startingNode.OrderByDescending(mcn => mcn.Value.ChainCount).FirstOrDefault();

                while (keyValuePair.Key != null || keyValuePair.Value != null)
                {
                    mostCommonChain.Append(keyValuePair.Key + " ");
                    if (decrementChainCount)
                    {
                        keyValuePair.Value.ChainCount--;
                    }

                    keyValuePair = keyValuePair.Value.OrderByDescending(mcn => mcn.Value.ChainCount).FirstOrDefault();
                }

                return mostCommonChain.ToString();
            }

            return null;
        }
    }
}