using System;
using System.Collections.Generic;
using System.Text;

namespace VMTranslator
{
    /// <summary>
    /// Handles translating a series of VM tokens to strings in the Hack Assembly language.
    /// </summary>
    class HackWriter
    {

        public bool TokensToHack(ref List<Token> Tokens, out string Hack)
        {
            Hack = new string(string.Empty);
            Token first = Tokens[0];

            switch (first.Type)
            {
                default:
                    throw new Exception("Unexpected first token.");
                case VMTypes.MemOp:
                    MemoryOperation(ref Tokens, ref Hack);
                    break;
                case VMTypes.FunctionOp:
                    throw new NotImplementedException("Function commands not yet implemented.");
                    break;
                case VMTypes.ArithmeticOp:
                    ArithmeticOperation(ref Tokens, ref Hack);
                    break;
            }

            return false;
        }

        private bool ArithmeticOperation(ref List<Token> Tokens, ref string Hack)
        {
            throw new Exception("Writing arithmetic ops not handled.");
            switch (Tokens[0].Value)
            {
                case "add":
                    break;
                case "sub":
                    break;
                case "neg":
                    break;
                case "eq":
                    break;
                case "gt":
                    break;
                case "lt":
                    break;
                case "and":
                    break;
                case "or":
                    break;
                case "not":
                    break;
            }
            return false;
        }

        private bool MemoryOperation(ref List<Token> Tokens, ref string Hack)
        {
            StringBuilder sb = new StringBuilder();
            if(Tokens[0].Value == "push")
            {
                if(Tokens[1].Value == "constant")
                {
                    sb.Clear();
                    sb.AppendLine(WriteComment(ref Tokens));
                    sb.AppendLine("@" + Tokens[2].Value);
                    sb.AppendLine("D=A");
                    sb.AppendLine("@SP");
                    sb.AppendLine("A=M");
                    sb.AppendLine("M=D");
                    sb.AppendLine("@SP");
                    sb.AppendLine("M=M+1");
                    Hack = sb.ToString();
                    return true;
                }

                if (Tokens[1].Value == "pointer")
                {
                    if (Tokens[2].Value == "0")
                    {
                        //pushes value inside @THIS to stack
                        sb.Clear();
                        sb.AppendLine(WriteComment(ref Tokens));
                        sb.AppendLine("@THIS");
                        sb.AppendLine("D=M");
                        sb.AppendLine("@SP");
                        sb.AppendLine("A=M");
                        sb.AppendLine("M=D");
                        sb.AppendLine("A=A+1");
                        Hack = sb.ToString();
                        return true;
                    }
                    else if (Tokens[2].Value == "1")
                    {
                        //pushes value inside @THAT to stack
                        sb.Clear();
                        sb.AppendLine(WriteComment(ref Tokens));
                        sb.AppendLine("@THAT");
                        sb.AppendLine("D=M");
                        sb.AppendLine("@SP");
                        sb.AppendLine("A=M");
                        sb.AppendLine("M=D");
                        sb.AppendLine("A=A+1");
                        Hack = sb.ToString();
                        return true;
                    }
                    return false;
                }

                //if (Tokens[1].Value == "this" || Tokens[1].Value == "that")
                //{
                //    sb.Clear();
                //    sb.AppendLine(WriteComment(ref Tokens));
                //    sb.AppendLine("@" + Tokens[1].Value.ToUpper());
                //    sb.AppendLine("D=M");
                //    sb.AppendLine("@SP");
                //    sb.AppendLine("A=M");
                //    sb.AppendLine("M=D");
                //    sb.AppendLine("A=A+1");
                //    Hack = sb.ToString();
                //    return true;
                //}

                if (Tokens[1].Value == "local" || Tokens[1].Value == "temp" || Tokens[1].Value == "argument" || Tokens[1].Value == "this" || Tokens[1].Value == "that")
                {
                    sb.Clear();
                    sb.AppendLine(WriteComment(ref Tokens));
                    int offset;
                    if (!int.TryParse(Tokens[2].Value, out offset)) throw new Exception("Unrecognized symbol in MemOp.");

                    switch (Tokens[1].Value)
                    { 
                        case "local":
                            sb.AppendLine("@LCL");
                            break;
                        case "temp":
                            sb.AppendLine("@5");
                            break;
                        case "argument":
                            sb.AppendLine("@ARG");
                            break;
                        case "this":
                            sb.AppendLine("@THIS");
                            break;
                        case "that":
                            sb.AppendLine("@THAT");
                            break;
                    }
                    sb.AppendLine("A=M");
                    for (int i = 0; i < offset; i++)
                    {
                        sb.AppendLine("A=A+1");
                    }
                    sb.AppendLine("D=M");
                    sb.AppendLine("@SP");
                    sb.AppendLine("A=M");
                    sb.AppendLine("M=D");
                    sb.AppendLine("@SP");
                    sb.AppendLine("M=M+1");
                    Hack = sb.ToString();
                    return true;
                }

                if (Tokens[1].Value == "static")
                {
                    int offset;
                    if (!int.TryParse(Tokens[2].Value, out offset)) throw new Exception("Unrecognized symbol in MemOp.");

                    sb.Clear();
                    sb.AppendLine(WriteComment(ref Tokens));
                    sb.AppendLine("@Foo." + offset);
                    sb.AppendLine("D=M");
                    sb.AppendLine("@SP");
                    sb.AppendLine("A=M");
                    sb.AppendLine("M=D");
                    sb.AppendLine("A=A-1");
                    Hack = sb.ToString();
                }

                return false;
            }
            else if(Tokens[0].Value == "pop")
            {
                if (Tokens[1].Value == "static" && Tokens[2].Type == VMTypes.Literal)
                {
                    sb.Clear();
                    sb.AppendLine(WriteComment(ref Tokens));
                    sb.AppendLine("@" + "Foo." + Tokens[2].Value);
                    sb.AppendLine("M=D");
                    Hack = sb.ToString();
                    return true;
                }

                if (Tokens[1].Value == "pointer")
                {
                    if (Tokens[2].Value == "0")
                    {
                        //Pop from stack into @THIS
                        sb.Clear();
                        sb.AppendLine(WriteComment(ref Tokens));
                        sb.AppendLine("@SP");
                        sb.AppendLine("M=M-1");
                        sb.AppendLine("A=M");
                        sb.AppendLine("D=M");
                        sb.AppendLine("@THIS");
                        sb.AppendLine("M=D");
                        Hack = sb.ToString();
                        return true;
                    }
                    else if (Tokens[2].Value == "1")
                    {
                        //pop from stack into @THAT
                        sb.Clear();
                        sb.AppendLine(WriteComment(ref Tokens));
                        sb.AppendLine("@SP");
                        sb.AppendLine("M=M-1");
                        sb.AppendLine("A=M");
                        sb.AppendLine("D=M");
                        sb.AppendLine("@THAT");
                        sb.AppendLine("M=D");
                        Hack = sb.ToString();
                        return true;
                    }
                    return false;
                }

                if (Tokens[1].Value == "local" || Tokens[1].Value == "temp" || Tokens[1].Value == "argument" || Tokens[1].Value == "this" || Tokens[1].Value == "that")
                {
                    sb.Clear();
                    sb.AppendLine(WriteComment(ref Tokens));
                    int offset;
                    if (!int.TryParse(Tokens[2].Value, out offset)) throw new Exception("Unrecognized symbol in MemOp.");

                    sb.AppendLine("@SP");
                    sb.AppendLine("M=M-1");
                    sb.AppendLine("A=M");
                    sb.AppendLine("D=M");

                    switch (Tokens[1].Value)
                    {
                        case "local":
                            sb.AppendLine("@LCL");
                            sb.AppendLine("A=M");
                            break;
                        case "temp":
                            sb.AppendLine("@5");
                            break;
                        case "argument":
                            sb.AppendLine("@ARG");
                            sb.AppendLine("A=M");
                            break;
                        case "this":
                            sb.AppendLine("@THIS");
                            sb.AppendLine("A=M");
                            break;
                        case "that":
                            sb.AppendLine("@THAT");
                            sb.AppendLine("A=M");
                            break;
                    }
                    
                    for (int i = 0; i < offset; i++)
                    {
                        sb.AppendLine("A=A+1");
                    }
                    
                    sb.AppendLine("M=D");
                    Hack = sb.ToString();
                    return true;
                }

                if (Tokens[1].Value == "static")
                {
                    sb.Clear();
                    sb.AppendLine(WriteComment(ref Tokens));
                    sb.AppendLine("A=M");
                    sb.AppendLine("D=M");
                    sb.AppendLine("A=A-1");
                    Hack = sb.ToString();
                    return true;
                }
                return false;
            }
            return false;
        }

        private string WriteComment(ref List<Token> Tokens)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("// ");
            foreach(Token tkn in Tokens)
            {
                sb.Append(tkn.Value + " ");
            }
            return sb.ToString();
        }
    }
}
