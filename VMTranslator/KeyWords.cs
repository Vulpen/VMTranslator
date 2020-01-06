using System;
using System.Collections.Generic;
using System.Text;

namespace VMTranslator
{
    /// <summary>
    /// Represents all the Keywords of the VM Language
    /// </summary>
    class KeyWords
    {
        private Dictionary<String, VMTypes> Keywords;

        public KeyWords()
        {
            Initialize();
        }

        public bool ResolveKeyword(ref string word, out VMTypes retType)
        {
            retType = VMTypes.None;
            if (Keywords.ContainsKey(word))
            {
                retType = Keywords[word];
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool ResolveKeyword(string word, out Token retType)
        {
            VMTypes type = VMTypes.None;
            retType = null;
            if(!ResolveKeyword(ref word, out type)) return false;
            retType = new Token(type, word);
            return true;
        }

        private void Initialize()
        {
            Keywords = new Dictionary<string, VMTypes>();
            //MemOps
            Keywords.Add("push", VMTypes.MemOp);
            Keywords.Add("pop", VMTypes.MemOp);

            //MemSegs
            Keywords.Add("constant", VMTypes.MemSeg);
            Keywords.Add("static", VMTypes.MemSeg);
            Keywords.Add("local", VMTypes.MemSeg);
            Keywords.Add("pointer", VMTypes.MemSeg);
            Keywords.Add("this", VMTypes.MemSeg);
            Keywords.Add("that", VMTypes.MemSeg);
            Keywords.Add("temp", VMTypes.MemSeg);
            Keywords.Add("argument", VMTypes.MemSeg);

            //Arithmetic Ops
            Keywords.Add("add", VMTypes.ArithmeticOp);
            Keywords.Add("sub", VMTypes.ArithmeticOp);
            Keywords.Add("neg", VMTypes.ArithmeticOp);
            Keywords.Add("eq", VMTypes.ArithmeticOp);
            Keywords.Add("gt", VMTypes.ArithmeticOp);
            Keywords.Add("lt", VMTypes.ArithmeticOp);
            Keywords.Add("and", VMTypes.ArithmeticOp);
            Keywords.Add("or", VMTypes.ArithmeticOp);
            Keywords.Add("not", VMTypes.ArithmeticOp);

            //Function Commands
            Keywords.Add("function", VMTypes.FunctionOp);
            Keywords.Add("call", VMTypes.FunctionOp);
            Keywords.Add("return", VMTypes.FunctionReturn);


            //Branch Commands
            Keywords.Add("label", VMTypes.BranchOp);
            Keywords.Add("goto", VMTypes.BranchOp);
            Keywords.Add("if-goto", VMTypes.BranchOp);

        }
    }
}
