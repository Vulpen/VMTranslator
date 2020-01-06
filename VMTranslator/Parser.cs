using System;
using System.Collections.Generic;
using System.Text;

namespace VMTranslator
{
    class Parser
    {
        private KeyWords _VMKeyWords;

        public Parser()
        {
            _VMKeyWords = new KeyWords();
        }

        /// <summary>
        /// Parses a line of VMCode into a list of tokens.
        /// </summary>
        /// <param name="vmCode">The line of VM language code.</param>
        /// <param name="parsedTokens">The list of tokens parsed from the line</param>
        /// <returns></returns>
        public bool ParseString(string vmCode, out List<Token> parsedTokens)
        {
            parsedTokens = new List<Token>();
            if (vmCode == null || vmCode.Length < 2) return false;

            string[] keys = vmCode.Split();

            if (keys.Length == 0) return false;

            if(!SimpleParse(ref keys, out parsedTokens))
            {
                throw new Exception("Could not successfully parse text");
            }

            if(!ContextParse(ref parsedTokens))
            {
                throw new Exception("Could not successfully context parse tokens");
            }

            if(!CheckTokens(ref parsedTokens))
            {
                throw new Exception("Could not successfully check tokens");
            }

            return false;
        }


        /// <summary>
        /// Goes through the VMCode keys array and only resolves keywords.
        /// </summary>
        /// <param name="Tokens"></param>
        /// <returns></returns>
        private bool SimpleParse(ref string[] keys, out List<Token> Tokens)
        {
            Tokens = new List<Token>();
            foreach(string key in keys)
            {
                Token curToken = null;
                if(_VMKeyWords.ResolveKeyword( key, out curToken))
                {
                    if (curToken != null)
                    {
                        Tokens.Add(curToken);
                    }
                }
                else
                {
                    curToken = new Token(VMTypes.None, key);
                    Tokens.Add(curToken);
                }
                
            }

            if(keys.Length != Tokens.Count)
            {
                throw new Exception("Generated tokens does not match number of strings.");
            }
            return true;
        }

        /// <summary>
        /// Changes tokens depending on the type of the first keyword in the list.
        /// </summary>
        /// <param name="Tokens"></param>
        /// <returns></returns>
        private bool ContextParse(ref List<Token> Tokens)
        {
            Token token = Tokens[0];
            Token curToken = null;
            switch (token.Type)
            {
                case VMTypes.MemOp:
                    if (Tokens.Count != 3) throw new Exception("MemoryOp contains unexpected token count.");
                    if(Tokens[1].Type == VMTypes.MemSeg && Tokens[2].Type == VMTypes.None)
                    {
                        int val;
                        if (int.TryParse(Tokens[2].Value, out val))
                        {
                            Tokens[2] = new Token(VMTypes.Literal, Tokens[2].Value);
                        }
                        else
                        {
                            Tokens[2] = new Token(VMTypes.Variable, Tokens[2].Value);
                        }
                    }
                    break;

                case VMTypes.ArithmeticOp:
                    if (Tokens.Count != 1) throw new Exception("ArithmeticOp contains unexpected token count.");
                    break;

                case VMTypes.BranchOp:
                    if (Tokens.Count != 2) throw new Exception("BranchOp contains unexpected token count.");
                    Tokens[1] = new Token(VMTypes.Label, Tokens[1].Value);
                    break;

                case VMTypes.FunctionOp:
                    if (Tokens.Count <= 2) throw new Exception("FunctionOp contains unexpected token count.");
                    Tokens[1] = new Token(VMTypes.FunctionName, Tokens[1].Value);
                    for(int i = 2; i < Tokens.Count; i++)
                    {
                        Tokens[i] = new Token(VMTypes.FunctionArg, Tokens[i].Value);
                    }
                    break;

                case VMTypes.FunctionReturn:
                    if (Tokens.Count != 1) throw new Exception("ArithmeticOp contains unexpected token count.");
                    break;

                default:
                case VMTypes.None:
                    throw new Exception("Could not context parse word " + token.Value);
                    break;

            }


            //At this point all tokens should NOT have a type of none
            return true;
        }


        /// <summary>
        /// Checks if a series of tokens is grammatically correct.
        /// </summary>
        /// <param name="Tokens"></param>
        /// <returns></returns>
        private bool CheckTokens(ref List<Token> Tokens)
        {
            Token firstToken = Tokens[0];
            Token curToken = null;
            switch(firstToken.Type)
            {
                default:
                    throw new Exception("Could not check token set");
                    return false;
                    break;
                case VMTypes.MemOp:
                    curToken = Tokens[1];
                    if(Tokens[1].Type == VMTypes.MemSeg)
                    {
                       
                        curToken = Tokens[2];
                        if(Tokens[2].Type == VMTypes.Literal)
                        {
                            if(Tokens[0].Value == "pop" && Tokens[1].Value == "constant")
                            {
                                throw new Exception("Pop not defined on MemSeg 'constant'");
                            }
                            return true;
                        }
                        else
                        {
                            throw new Exception("Attempt to operate non-literal with memseg");
                        }
                    }
                    else
                    {
                        throw new Exception("MemOp on non-segment");
                    }
                    return true;
                    break;
                case VMTypes.FunctionOp:
                    throw new NotImplementedException("Function operations not yet implemented.");
                    break;
                case VMTypes.ArithmeticOp:
                    if (Tokens.Count != 1 || Tokens[0].Type != VMTypes.ArithmeticOp) throw new Exception("Arithmetic operation contains unexpected symbols");
                    return true;
                    break;

            }

            return false;
        }
    }
}
