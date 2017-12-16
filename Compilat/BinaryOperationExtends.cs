﻿using System;
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
            return MISC.tabsLLVM(depth) 
                + a.ToLLVM(depth) 
                + " = "
                + ((b as ASTvalue != null)? b.returnTypes().ToLLVM() + " " : "")
                + b.ToLLVM(depth);
        }
    }

    public class Equal : BinaryOperation
    {
        public Equal(IOperation left, IOperation right)
        {
            operationString = "==";
            IOperation[] children = new IOperation[2] {left, right };
            returnType = TypeConverter.TryConvertEqual(ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, "eq", a, b, a.returnTypes());
        }
        //IOperation goDeeper(IOperation fromOperation)
        //{
        //    if ((fromOperation as Equal) != null)
        //    {
        //        if (a as ASTvalue != null && (a as ASTvalue).getValueType == new ValueType(VT.Cboolean) && (bool)((a as ASTvalue).getValue) == true)
        //            return goDeeper(b); 
        //        if (b as ASTvalue != null && (b as ASTvalue).getValueType == new ValueType(VT.Cboolean) && (bool)((b as ASTvalue).getValue) == true)
        //            return goDeeper(a);
        //    }
        //    return fromOperation;
        //}
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
        }
        public override string ToLLVM(int depth)
        {
            return LLVM.BinaryToLLVM(depth, "ne", a, b, a.returnTypes());
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
