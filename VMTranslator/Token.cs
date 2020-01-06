using System;
using System.Collections.Generic;
using System.Text;

namespace VMTranslator
{
    class Token
    {
        private int _vmtype;
        private string _value;

        public VMTypes Type
        {
            get { return (VMTypes)_vmtype; }
        }

        public string Value
        {
            get { return _value; }
        }

        public Token(int type, string val)
        {
            _vmtype = type;
            _value = val;
        }

        public Token(VMTypes type, string val)
        {
            _vmtype = (int)type;
            _value = val;
        }

        public override string ToString()
        {
            return _vmtype + ":" + _value;
        }
    }
}
