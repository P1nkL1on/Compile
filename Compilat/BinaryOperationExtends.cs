using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{

    class Assum : BinaryOperation
    {
        public string requiredUpdate;
        public Assum(IOperation left, IOperation right)
        {
            operationString = "=";
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvertAsum(ref children);
            a = children[0]; b = children[1]; ;
            requiredUpdate = "none";
            //
            int foundNumber = -1;
            for (int i = ASTTree.variables.Count - 1; i >= 0 ; i--)
            {
                if ((a as Define) != null && ASTTree.variables[i].name == (a as Define).varName)
                {
                    foundNumber = i;
                    break;
                }
                if (a as GetValByAdress != null)
                {
                    foundNumber = (a as GetValByAdress).GetAdress();
                    break;
                }
            }
            if (foundNumber == -2) // it is a pointer typed
                return;
            if (foundNumber < 0)
                throw new Exception("Assumming not a variable!");
            else
                MISC.defineVariable(foundNumber);
            //
        }
        public string GetAssumableName
        {
            get
            {
                if ((a as ASTvariable) != null) return (a as ASTvariable).name;
                if ((a as Define) != null) return (a as Define).varName;
                return "-";
            }
        }
        public List<IOperation> GetStructDefine()
        {
            if (b as StructureDefine == null)
                return new List<IOperation>();
            return (b as StructureDefine).values;
        }
        public override string ToLLVM(int depth)
        {
            if (a as GetValByAdress != null)
            {
                (a as GetValByAdress).LLVM_isLeftOperand = true;
                string number = b.ToLLVM(depth), add_last = a.ToLLVM(depth);
                LLVM.AddToCode(b.returnTypes().ToLLVM() + " " +number +","+ add_last);
                return "";
            }
            return MISC.tabsLLVM(depth)
                + a.ToLLVM(depth)
                + " = "
                + ((b as ASTvalue != null) ? b.returnTypes().ToLLVM() + " " : "")
                + b.ToLLVM(depth);
        }
        public string LLVMGLOBAL(int depth)
        {
            return MISC.tabsLLVM(depth)
                + a.ToLLVM(depth)
                + " = global "
                + ((b as ASTvalue != null) ? b.returnTypes().ToLLVM() + " " : "")
                + b.ToLLVM(depth);
        }
    }

    public class Equal : BinaryOperation
    {
        public Equal(IOperation left, IOperation right)
        {
            operationString = "==";
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvertEqual(ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryEqualToLLVM(depth, "eq", a, b, a.returnTypes());
        }

        public IOperation getTrueEqual()
        {
            if (b as ASTvalue != null && (b as ASTvalue).getValueType == new ValueType(VT.Cboolean) && (bool)((b as ASTvalue).getValue) == true)
                return a;
            if (a as ASTvalue != null && (a as ASTvalue).getValueType == new ValueType(VT.Cboolean) && (bool)((a as ASTvalue).getValue) == true)
                return b;
            return this;
        }
    }
    class Uneq : BinaryOperation
    {
        public Uneq(IOperation left, IOperation right)
        {
            operationString = "!=";
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvertEqual(ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryEqualToLLVM(depth, "ne", a, b, a.returnTypes());
        }
    }

    class AND : BinaryOperation
    {
        public AND(IOperation left, IOperation right)
        {
            operationString = "&";
            TypeConvertion tpcv = new TypeConvertion("BBB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, "ang", a, b, a.returnTypes());
        }
    }

    class OR : BinaryOperation
    {
        public OR(IOperation left, IOperation right)
        {
            operationString = "|";
            TypeConvertion tpcv = new TypeConvertion("BBB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, "or", a, b, a.returnTypes());
        }
    }

    class ANDS : BinaryOperation
    {
        public ANDS(IOperation left, IOperation right)
        {
            operationString = "&&";
            TypeConvertion tpcv = new TypeConvertion("BBB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, "and", a, b, a.returnTypes());
        }
    }

    class ORS : BinaryOperation
    {
        public ORS(IOperation left, IOperation right)
        {
            operationString = "||";
            TypeConvertion tpcv = new TypeConvertion("BBB", 2);
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, "or", a, b, a.returnTypes());
        }
    }
}
