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
        public ASTvariable var;
        public string varName;
        public Define(string s, bool autoAssume)
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

            ASTvariable NV = new ASTvariable(defineType, varName, pointerLevel, MISC.GetCurrentVariableAdressType());
            var = NV;
            ASTTree.variables.Add(NV);
            MISC.pushVariable(ASTTree.variables.Count - 1);
            if (autoAssume)
                MISC.defineVariable(ASTTree.variables.Count - 1);

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
            MISC.ConsoleWriteLine("DEFINE"/* [" + defineType.ToString() + "]"*/, ConsoleColor.DarkYellow);
            MISC.finish = true; a.Trace(depth + 1);
            
            //b.Trace(depth + 1);
        }
        public override string ToLLVM(int depth)
        {
            string res = var.ToLLVM();
            //if (var.everUsed > 0 && !var.wasLoaded)
            //{ var.wasLoaded = true; LLVM.CommandOrderQueueCode += String.Format("{0}{1} = load {2}, {3} {4}\n", MISC.tabsLLVM(depth), "%~" + var.name, var.returnTypes().ToLLVM(), var.returnTypes().TypeOfPointerToThis().ToLLVM(), var.ToLLVM()); }
            LLVM.AddToCode(String.Format("{0}{1} = alloca {2}\n", MISC.tabsLLVM(depth), res, var.returnTypes().ToLLVM()));
            return res;
            
            // %X = alloca i32 ; we have a pointer for %X, we like writing int*X = new int(); or some shit like it
            // now we can store i32 100, i32* %X
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
            TypeConvertion tpcv = new TypeConvertion("II", 1);
            IOperation[] children = new IOperation[1] { val };
            returnType = TypeConverter.TryConvert(tpcv, ref children);
            a = children[0];
        }
        public override string ToLLVM(int depth)
        {
            return "++";
        }
    }
    class Dscr : MonoOperation
    {
        public Dscr(IOperation val)
        {
            operationString = "--";
            TypeConvertion tpcv = new TypeConvertion("II", 1);
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

            //try { ((val as GetValByAdress).from as ASTvariable).everPointed = true; }
            //catch (Exception e) { }

            returnType = a.returnTypes().TypeOfPointerToThis();
        }
        public override string ToLLVM(int depth)
        {
            return a.ToLLVM(depth);//LLVM.ParamToLLVM(depth, "", returnType, a);
        }
    }
    class GetValByAdress : MonoOperation
    {
        public bool LLVM_isLeftOperand = false;
        ASTvariable variable;

        public IOperation from;
        IOperation adder;
        public GetValByAdress(IOperation adress, ValueType retType)
        {
            operationString = "get";
            a = adress;
            try
            {
                variable = ASTTree.variables[(int)((adress as ASTvalue).getValue)];
                operationString = variable.name;
                returnType = variable.returnTypes();
                from = variable;
                return;
            }
            catch (Exception e)
            {
                variable = null;
                try
                {
                    adder = (a as BinarySummatic).FindNonVariable(out from);
                }catch (Exception E){
                    // case if you have only *d, but not d[0], so there is no summ action in there
                    from = adress; adder = new ASTvalue(new ValueType(VT.Cint), (object)0);
                }
                returnType = retType.TypeOfPointedByThis();
            };
        }
        protected ASTvariable FindOriginalVariable()
        {
            if (variable != null)
                return variable;
            if (a as BinarySummatic != null)
            {
                ASTvariable var = (a as BinarySummatic).FindVariable();
                GetValByAdress deeper = (a as BinarySummatic).Deep();
                if (var == null)
                    return deeper.FindOriginalVariable();
                return var;
            }
            return null;
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
            if (a.returnTypes().pointerLevel == 1)
                return -2;
            return -1;
        }
        public override string ToLLVM(int depth)
        {
            if (variable == null)
            {
                bool no_need_getelementptr = false;
                if ((adder as ASTvalue) != null && ((adder as ASTvalue).getValueType == VT.Cint) && (int)((adder as ASTvalue).getValue) == 0)
                    no_need_getelementptr = true;

               // MISC.LLVMtmpNumber += (no_need_getelementptr || LLVM_isLeftOperand) ? ((LLVM_isLeftOperand)? 0 : 1) : 2;   // need only 1 variable if not getelementptr
                string store_or_load_from = "????";
                if (!no_need_getelementptr)
                {
                    MISC.LLVMtmpNumber++;
                    int num = MISC.LLVMtmpNumber;
                    LLVM.AddToCode(MISC.tabsLLVM(depth) + "%tmp" + (num) + " = " + LLVM.ParamToLLVM(depth, "getelementptr", returnTypes(), from, adder));
                    store_or_load_from = "%tmp" + num;
                }
                else
                    store_or_load_from = ""+from.ToLLVM(0);
                MISC.LLVMtmpNumber++;
                int num2 = MISC.LLVMtmpNumber;
                if (!LLVM_isLeftOperand)
                {
                    LLVM.AddToCode(MISC.tabsLLVM(depth) + "%tmp" + (num2 + 1) + " = " + LLVM.Load(returnTypes(), from.returnTypes().ToLLVM() + " " +store_or_load_from) + "\n");
                    return "%tmp" + (num2 + 1);
                }
                else
                {
                    LLVM_isLeftOperand = false;
                    LLVM.AddToCode(MISC.tabsLLVM(depth) + "store ");
                    return String.Format(" {0} {1}, align {2}", from.returnTypes().ToLLVM(), store_or_load_from, MISC.SyzeOf(returnTypes()));
                }
            }
            return variable.ToLLVM();
        }
        
        public override void Trace(int depth)
        {
            if (variable == null)
            {
                Console.Write(MISC.tabs(depth));
                MISC.ConsoleWrite(operationString, ConsoleColor.DarkGreen);
                MISC.ConsoleWrite(" " + returnType.ToString().Substring(1), ConsoleColor.Red);//
                MISC.ConsoleWriteLine(" by adress", ConsoleColor.DarkGreen);
                if (a != null)
                {
                    MISC.finish = true;//
                    a.Trace(depth + 1);
                }
            }
            else
            {
                //MISC.finish = true;
                variable.Trace(depth);
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
            operationString = String.Format("{0}", toType.ToString().Substring(1));
            a = val;
            returnType = toType;
        }
    }
}
