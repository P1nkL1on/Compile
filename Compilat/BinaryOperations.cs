using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    class BinarySummatic : BinaryOperation
    {
        public GetValByAdress Deep()
        {
            if (a as GetValByAdress != null)
                return (a as GetValByAdress);
            if (b as GetValByAdress != null)
                return (b as GetValByAdress);
            return null;
        }
        public ASTvariable FindVariable (){
            if (a as ASTvariable != null)
                return (a as ASTvariable);
            if (b as ASTvariable != null)
                return (b as ASTvariable);
            return null;
        }
        public IOperation FindNonVariable(out IOperation other)
        {
            if (a as GetValByAdress != null)
            { other = a;  return (b); }
            if (b as GetValByAdress != null)
            { other = b;  return (a); }
            other = null;
            return null;
        }
    }
    class Summ : BinarySummatic
    {
        public Summ(IOperation left, IOperation right)
        {
            operationString = "+";
            //TypeConvertion tpcv = new TypeConvertion("IIIDDDSSSAAAAIAIAA", 2);
            //IOperation[] children = new IOperation[2] { left, right };
            //returnType = MISC.CheckTypeCorrect(this, tpcv, ref children);
            //a = children[0]; b = children[1];

            //MISC.ConsoleWriteLine(
            //tpcv.ToString(), ConsoleColor.Magenta);

            TypeConvertion tpcv = new TypeConvertion("IIIDDDSSS", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvertSumm(tpcv, ref children);
            a = children[0]; b = children[1];
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, (returnType != VT.Cdouble)? "add" : "fadd", a, b, returnType);
        }
    }
    class Diff : BinarySummatic
    {
        public Diff(IOperation left, IOperation right)
        {
            operationString = "-";
            TypeConvertion tpcv = new TypeConvertion("IIIDDD", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvertSumm(tpcv, ref children);
            a = children[1]; b = children[0];
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, (returnType != VT.Cdouble) ? "sub" : "fsub", a, b, returnType);
        }
    }

    class Mult : BinaryOperation
    {
        public Mult(IOperation left, IOperation right)
        {
            operationString = "*";
            TypeConvertion tpcv = new TypeConvertion("IIIDDD", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, (returnType != VT.Cdouble) ? "mul" : "fmul", a, b, returnType);
        }
    }

    class Qout : BinaryOperation
    {
        public Qout(IOperation left, IOperation right)
        {
            operationString = "/";
            TypeConvertion tpcv = new TypeConvertion("IIIDDD", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, (returnType != VT.Cdouble) ? "sdiv" : "fdiv", a, b, returnType);
        }
    }
}
