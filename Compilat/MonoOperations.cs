using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    class Define : BinaryOperation
    {
        ValueType defineType;

        public string varName;
        public Define(string s)
        {
            //string firstPart = s.Substring(0, s.IndexOf("$") + 1);  // int&a,b
            //while (s.IndexOf(',') > 0)
            //{
            //    int at = s.IndexOf(',');
            //    s = s.Substring(0, s.IndexOf(',')) + ';' + firstPart + s.Substring(s.IndexOf(',') + 1);
            //    //
            //}

            string[] ss = s.Split('$');
            //string varName;
            VT varType;
            bool everDefined = false;
            for (int i = 0; i < ASTTree.variables.Count; i++)
                if (ASTTree.variables[i].name == ss[1] && MISC.isVariableAvailable(i))
                { everDefined = true; break; }

            if (ss[1].Length > 0 && !everDefined)
                varName = ss[1];
            else
                throw new Exception("Can not define " + ((everDefined) ? "again " : "") + "variable with name \"" + ss[1] + "\"");

            ss[0].ToLower();
            varType = detectType(ss[0]);

            //for (int i =0; i < varName.Length; i++){
            //bool isPointer = false;
            int pointerLevel = 0;

            while (varName.IndexOf('*') == 0) { pointerLevel++; varName = varName.Substring(1); }
            returnType = new ValueType(varType, pointerLevel);
            defineType = returnType;

            //____________________________________

            if (varName.LastIndexOf("]") == varName.Length - 1 && varName.IndexOf("[") > 0)
            {
                List<string> inBr = MISC.splitByQuad(varName);  // [] [] [ ] []
                varName = varName.Substring(0, varName.IndexOf('['));

                for (int ib = 0; ib < inBr.Count; ib++)
                {
                    string inBrack = inBr[ib];
                    int length = 0;
                    if (inBrack != "")
                    {
                        IOperation arrayLength = BinaryOperation.ParseFrom(inBrack);
                        if (arrayLength.returnTypes() != VT.Cint)
                            throw new Exception("Int only can be array length parameter");
                        //IOperation arrayLength = BinaryOperation.ParseFrom(inBrack);
                        //if (arrayLength as ASTvalue == null || arrayLength.returnTypes() != VT.Cint)
                        //    throw new Exception("Incorrect array length parameters!");

                        //length = (int)(arrayLength as ASTvalue).getValue;
                        //if (length < 1)
                        //    throw new Exception("Array length should be 1 and more!");
                    }

                    //for (int i = 0; i < length; i++)
                    //{
                    //    // as default variable
                    //    ASTvariable newVar = new ASTvariable(new ValueType(varType, pointerLevel), varName + "#" + i, 0);
                    //    ASTTree.variables.Add(newVar);
                    //    MISC.pushVariable(ASTTree.variables.Count - 1);
                    //    ASTTree.tokens.Add(newVar);
                    //}
                    defineType = defineType.TypeOfPointerToThis();
                }
               
            }
            //_________________________________________

            ASTvariable NV = new ASTvariable(defineType, varName, pointerLevel);
            ASTTree.variables.Add(NV);
            MISC.pushVariable(ASTTree.variables.Count - 1);

            ASTTree.tokens.Add(NV);
            a = NV;

            returnType = a.returnTypes();

            b = new ASTvalue(new ValueType(VT.Cadress), (object)(ASTTree.variables.Count - 1));
        }
        public static VT detectType(string s)
        {
            if (s == "double") return VT.Cdouble;
            if (s == "int") return VT.Cint;
            if (s == "string") return VT.Cstring;
            if (s == "char") return VT.Cchar;
            if (s == "bool" || s == "boolean") return VT.Cboolean;
            if (s == "void") return VT.Cvoid;
            return VT.Cunknown;
        }
        public override void Trace(int depth)
        {
            Console.Write(MISC.tabs(depth));
            MISC.ConsoleWriteLine("DEFINE [" + defineType.ToString() + "]", ConsoleColor.DarkGreen);
            a.Trace(depth + 1);
            MISC.finish = true;
            b.Trace(depth + 1);
        }
    }

    class Mins : MonoOperation
    {
        public Mins(IOperation val)
        {
            operationString = "-";
            TypeConvertion tpcv = new TypeConvertion("IIDD", 1);
            IOperation[] children = new IOperation[1] { val };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0];
        }

    }
    class Nega : MonoOperation
    {
        public Nega(IOperation val)
        {
            operationString = "!";
            TypeConvertion tpcv = new TypeConvertion("BB", 1);
            IOperation[] children = new IOperation[1] { val };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0];
        }

    }

    class Incr : MonoOperation
    {
        public Incr(IOperation val)
        {
            operationString = "++";
            TypeConvertion tpcv = new TypeConvertion("IIDD", 1);
            IOperation[] children = new IOperation[1] { val };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0];
        }
    }
    class Dscr : MonoOperation
    {
        public Dscr(IOperation val)
        {
            operationString = "--";
            TypeConvertion tpcv = new TypeConvertion("IIDD", 1);
            IOperation[] children = new IOperation[1] { val };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0];
        }
    }
    class Adrs : MonoOperation
    {
        //public ValueType wantType;
        public Adrs(IOperation val)
        {
            if (val as GetValByAdress == null)
                throw new Exception("Can not get adress of non-variable token!");
            operationString = "Get adress";
            a = val;
            returnType = a.returnTypes().TypeOfPointerToThis();
        }

    }
    class GetValByAdress : MonoOperation
    {
        public ValueType pointerType;
        public GetValByAdress(IOperation adress, ValueType retType, bool stop)
        {
            operationString = "get";
            a = adress;
            returnType = retType;
        }
        public GetValByAdress(IOperation adress, ValueType retType)
        {
            operationString = "get";
            a = adress;
            try
            {
                operationString = ASTTree.variables[(int)((adress as ASTvalue).getValue)].name;
                returnType = ASTTree.variables[(int)((adress as ASTvalue).getValue)].returnTypes();
                return;
            }
            catch (Exception e)
            {
                returnType = retType.TypeOfPointedByThis();
            };

            //a = adress;
            ////if (a.returnTypes() != ValueType.Cadress)
            ////    throw new Exception("You can get value only by number of memory slot!");

            //returnType = retType;

            //if (retType == VT.Cadress)
            //{
            //    //IOperation dep = a; int res = -1;
            //    //while ((a as GetValByAdress) != null)
            //    //{
            //    //    a.Trace(0);
            //    //    res = (a as GetValByAdress).GetAdress();
            //    //    a = (a as GetValByAdress).a;
            //    //}
            //    //if (res >= 0)
            //    //    returnType = ASTTree.variables[res].returnTypes();

            //    //Console.WriteLine(a.returnTypes() + " /// " + res + " //// " + returnType.ToString());
            //    //Console.ReadKey();
            //    IOperation dep = a; int res = -1;
            //    while (a.returnTypes() == ValueType.Cadress)
            //    {
            //        a.Trace(0);
            //        res = ((a as GetValByAdress) != null) ? (a as GetValByAdress).GetAdress() : (int)((a as ASTvalue).getValue);
            //        a = ASTTree.variables[res];
            //        returnType = a.returnTypes();
            //        dep = new GetValByAdress(new ASTvalue(ValueType.Cadress, (object)res), returnType, true);
            //    }
            //    a = dep;
            //}

        }

        public int GetAdress()
        {
            if (a as ASTvalue != null)
            {
                int variableNumber = (int)((a as ASTvalue).getValue);
                if (variableNumber >= ASTTree.variables.Count)
                    return -1;
                return variableNumber;
            }
            return -1;
        }

        public override void Trace(int depth)
        {
            Console.Write(MISC.tabs(depth)); MISC.ConsoleWrite(operationString, ConsoleColor.Green); MISC.ConsoleWriteLine(" " + returnType.ToString().Substring(1), ConsoleColor.DarkGreen);
            if (a != null) //if (returnType == VT.Cadress)
            {
                MISC.finish = true;
                a.Trace(depth + 1);
            }
        }
    }


    class StructureDefine : MonoOperation
    {
        public List<IOperation> values;

        public StructureDefine(string S)
        {
            operationString = "List values";
            //returnType = new ValueType(VT.Cadress);
            ValueType curVt = new ValueType(VT.Cunknown);
            values = new List<IOperation>();

            if (S.Length == 0) return;
            string[] sSplited = MISC.splitBy(S, ',').ToArray();
            for (int i = 0; i < sSplited.Length; i++)
            {
                try
                {
                    values.Add(BinaryOperation.ParseFrom(sSplited[i]));

                    if (i > 0 && curVt != values[values.Count - 1].returnTypes())
                        throw new Exception("Define struct must contain only monotype args");

                    curVt = values[values.Count - 1].returnTypes();
                }
                catch (Exception e)
                {
                    throw new Exception("Can not parse define from \"" + sSplited[i] + "\"");
                }
            }
            returnType = curVt.TypeOfPointerToThis();
        }
        public override void Trace(int depth)
        {
            Console.WriteLine(MISC.tabs(depth) + operationString);
            for (int i = 0; i < values.Count; i++)
            {
                if (i == values.Count - 1)
                    MISC.finish = true;
                values[i].Trace(depth + 1);
            }
        }
    }

    class Conv : MonoOperation
    {
        public Conv(IOperation val, ValueType toType)
        {
            operationString = String.Format("{0}<-", toType.ToString().Substring(1));
            a = val;
            returnType = toType;
        }
    }
}
