using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    class LsEq : BinaryOperation
    {
        public LsEq(IOperation left, IOperation right)
        {
            operationString = "<=";
            TypeConvertion tpcv = new TypeConvertion("IIBDDB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryEqualToLLVM(depth, "sle", a, b, returnType);
        }
    }
    class Less : BinaryOperation
    {
        public Less(IOperation left, IOperation right)
        {
            operationString = "<";
            TypeConvertion tpcv = new TypeConvertion("IIBDDB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryEqualToLLVM(depth, "slt", a, b, returnType);
        }
    }
    class MrEq : BinaryOperation
    {
        public MrEq(IOperation left, IOperation right)
        {
            operationString = ">=";
            TypeConvertion tpcv = new TypeConvertion("IIBDDB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryEqualToLLVM(depth, "sge", a, b, returnType);
        }
    }
    class More : BinaryOperation
    {
        public More(IOperation left, IOperation right)
        {
            operationString = ">";
            TypeConvertion tpcv = new TypeConvertion("IIBDDB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryEqualToLLVM(depth, "sgt", a, b, returnType);
        }
    }
}
