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
    }

    class Equal : BinaryOperation
    {
        public Equal(IOperation left, IOperation right)
        {
            operationString = "==";
            IOperation[] children = new IOperation[2] {left, right };
            returnType = TypeConverter.TryConvertEqual(ref children);
            a = children[0]; b = children[1];
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
        }
    }
}
