using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{

    class Assum : BinaryOperation
    {
        public bool defining;
        public string requiredUpdate;
        public Assum(IOperation left, IOperation right)
        {
            defining = false;
            operationString = "=";
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvertAsum(ref children);
            a = children[0]; b = children[1]; ;
            requiredUpdate = "none";
            //
            int foundNumber = -1;
            for (int i = ASTTree.variables.Count - 1; i >= 0; i--)
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
                defining = MISC.defineVariable(foundNumber);
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

            ASTvariable vari = null;
            if (a as GetValByAdress != null) vari = ((a as GetValByAdress).from as ASTvariable);
            if (a as Define != null) vari = (a as Define).var;

            if (vari != null && vari.everUsed > 0 && vari.adress.typ != VAT.Global)// && !vari.wasLoaded)
            {
                LLVM.varisReload.Add(vari);
                vari.reloadedTimes++;
                LLVM.CommandOrderQueueCode += String.Format("{0}{1} = load {2}, {3} {4}\n", MISC.tabsLLVM(depth), vari.ToLLVM(), vari.returnTypes().ToLLVM(), vari.returnTypes().TypeOfPointerToThis().ToLLVM(), MISC.RemoveCall(vari.ToLLVM()));
                vari.reloadedTimes--;
            }

            if (a as GetValByAdress != null)
            {
                // variable? from
                ASTvariable vari2 = ((a as GetValByAdress).from as ASTvariable);
                // NOT случай, когда переменная просто проходная- не глобальная, не параметр, и обращение идет прямо на неё
                if (!(vari2 != null /* && !vari.everPointed*/ && vari2.adress.typ != VAT.Parameter))
                {
                    (a as GetValByAdress).LLVM_isLeftOperand = true;
                    string number = b.ToLLVM(depth), add_last = a.ToLLVM(depth);
                    LLVM.AddToCode(b.returnTypes().ToLLVM() + " " + number + "," + add_last);
                    return "";
                }
            }
            if (!(vari != null && vari.adress.typ == VAT.Global))
                return String.Format("{0}store {3} {4}, {1} {2}", MISC.tabsLLVM(depth), a.returnTypes().TypeOfPointerToThis().ToLLVM(), MISC.RemoveCall(a.ToLLVM(depth)), b.returnTypes().ToLLVM(), b.ToLLVM(depth));
            return "";
        }
        public string LLVMGLOBAL(int depth)
        {
            return MISC.tabsLLVM(depth)
                + a.ToLLVM(depth)
                + " = global "
                + ((b as ASTvalue != null) ? b.returnTypes().ToLLVM() + " " : "")
                + b.ToLLVM(depth);
        }
        public override void Trace(int depth)
        {
            Console.Write(MISC.tabs(depth));
            if (!defining)
                MISC.ConsoleWrite(operationString, ConsoleColor.Yellow);
            else
                MISC.ConsoleWrite(operationString + "", ConsoleColor.Red);
            MISC.ConsoleWriteLine("   " + returnType.ToString(), ConsoleColor.DarkGreen);
            a.Trace(depth + 1);
            MISC.finish = true;
            b.Trace(depth + 1);
        }
    }

    public class Equal : BinaryOperation
    {
        public bool CallFromIf;
        public Equal(IOperation left, IOperation right)
        {
            operationString = "==";
            IOperation[] children = new IOperation[2] { left, right };
            returnType = TypeConverter.TryConvertEqual(ref children);
            a = children[0]; b = children[1];
            returnType = new ValueType(VT.Cboolean);
            CallFromIf = false;
        }
        public override string ToLLVM(int depth)
        {
            if (!CallFromIf)
                return LLVM.BinaryEqualToLLVM(depth, "eq", a, b, a.returnTypes());
            MISC.LLVMtmpNumber++;
            int num = MISC.LLVMtmpNumber;
            string kids = String.Format("{0}, {1}\n", a.ToLLVM(depth), b.ToLLVM(depth));
            return String.Format("{3} {1} {2}", num, a.returnTypes().ToLLVM(), kids, "eq", MISC.tabsLLVM(depth));
        }

        public IOperation getTrueEqual()
        {
            //return this;
            bool notB = false, notA = false;
            if (b as ASTvalue != null && (b as ASTvalue).getValueType == new ValueType(VT.Cboolean) && (bool)((b as ASTvalue).getValue) == true)
                notB = true;
            if (a as ASTvalue != null && (a as ASTvalue).getValueType == new ValueType(VT.Cboolean) && (bool)((a as ASTvalue).getValue) == true)
                notA = true;

            if (notA && !notB) return b;
            if (notB && !notA) return a;

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
