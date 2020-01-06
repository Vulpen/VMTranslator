using System;

namespace VMTranslator
{
    /// <summary>
    /// Represents all the possible types of tokens.
    /// </summary>
    public enum VMTypes
    {
        None = 0,
        ArithmeticOp = 1,
        MemSeg,
        MemOp,
        Literal,
        Variable,
        FunctionOp,
        FunctionName,
        FunctionArg,
        FunctionReturn,
        BranchOp,
        Label,
        EndLine,
        EndFile
    }



}