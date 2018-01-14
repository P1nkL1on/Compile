using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public class ASTTree
    {
        public static List<ASTFunction> funcs = new List<ASTFunction>();
        string original;
        string path;
        public static List<IASTtoken> tokens = new List<IASTtoken>();
        public static List<ASTvariable> variables = new List<ASTvariable>();
        public static CommandOrder GlobalVars = new CommandOrder();

        public static ConsoleColor clr = ConsoleColor.Black;
        static List<int> generatedTime = new List<int>();
        static DateTime data;

        public void TraceLLVM()
        {
            MISC.ConsoleWriteLine("\nLLVM\n\n", ConsoleColor.Magenta);
            ToLLVM(true);
            string seps = " *,\n",
                   types = "i1_i32_i64_i16_f32_f64_void_i8_double_",
                   opers = "add_mul_sub_sdiv_fdiv_fadd_fmul_fsub_br_eq_ne_sgt_sge_slt_sle_or_and_icmp_",
                   vars = "label_define_declare_tail_call_global_constant_getelementptr_load_align_ret_store_alloca_private_";
            string code = LLVM.CurrentCode;
            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == ';')
                    Console.ForegroundColor = ConsoleColor.Cyan;
                if (Console.ForegroundColor != ConsoleColor.Cyan)
                {
                    if ((" {}(),").IndexOf(code[i]) >= 0)
                        Console.ForegroundColor = ConsoleColor.Gray;
                    if (code.Substring(i).IndexOf(':') < code.Substring(i).IndexOf('\n') && code.Substring(i).IndexOf(':') > 0)
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    if (code[i] == '%')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (code.Length > i - 6 && i >= 6 && code.Substring(i - 6).IndexOf("label") == 0)
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    }
                    if (code[i] == '@') Console.ForegroundColor = ConsoleColor.Red;


                    for (int jj = 0; jj < seps.Length; jj++)
                    {
                        string nowword = (code.Substring(i).IndexOf(seps[jj]) < 0) ? "" : code.Substring(i).Remove(code.Substring(i).IndexOf(seps[jj]));
                        if (nowword.Length > 1)
                        {
                            nowword += "_";
                            if (opers.IndexOf(nowword) >= 0)
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            if (vars.IndexOf(nowword) >= 0)
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            if (types.IndexOf(nowword) >= 0)
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                    }
                }
                else
                {
                    if (code[i] == '\n')
                        Console.ForegroundColor = ConsoleColor.Gray;
                }
                if (!(code[i] == '\n' && i > 0 && code[i - 1] == '\n'))
                    Console.Write(code[i]);
            }
        }
        public String ToLLVM(bool writeToFile)
        {
            data = DateTime.Now;
            if (GlobalVars.CommandCount != 0)
                GlobalVars.TryTraceLLVMGlobalVars();
            for (int i = 0; i < funcs.Count; i++)
                if (funcs[i] != null)
                {
                    MISC.LLVMtmpNumber = 0;
                    LLVM.AddToCode(funcs[i].ToLLVM(i) + "\n\n");
                    LLVM.AddToCode("\n");
                }
            string pathLLVM = path.Remove(path.LastIndexOf('.')) + "LLVM.ll";
            System.IO.File.WriteAllText(pathLLVM, LLVM.CurrentCode);
            generatedTime.Add((DateTime.Now - data).Seconds * 1000 + (DateTime.Now - data).Milliseconds);
            return LLVM.CurrentCode;
        }
        public void Trace()
        {
            if (funcs.Count <= 0)
                return;
            Console.Clear();
            clr = ConsoleColor.Black;

            Console.WriteLine(String.Format("Path:\n\t{0}\nCode:\n\n{1}", path, original));

            //Console.WriteLine("\nTokens:");
            //for (int i = 0; i < tokens.Count; i++)
            //    tokens[i].TraceMore(0);
            Console.WriteLine("\n\nVariables\n local\tname\t\ttype\t\tused\tadress");
            for (int i = 0; i < variables.Count; i++)
                variables[i].TraceMore(0);
            if (GlobalVars.CommandCount > 0)
            {
                Console.Write(MISC.tabs(0));
                MISC.ConsoleWriteLine("Global vars:", ConsoleColor.Black, ConsoleColor.Cyan);
                GlobalVars.Trace(1);
            }
            Console.WriteLine("\nFunctions:");
            for (int i = 0; i < funcs.Count; i++)
            {
                if (funcs[i] != null)
                    MISC.ConsoleWriteLine(String.Format("  {0}: {1}", funcs[i].getName, funcs[i].getArgsString), ConsoleColor.Green);
                else
                    MISC.ConsoleWriteLine(String.Format("  {0}:", "null"), ConsoleColor.DarkGreen);
            }
            Console.WriteLine();
            for (int i = 0; i < funcs.Count; i++)
                if (funcs[i] != null)
                {
                    clr = (funcs[i].getName.ToLower() == "main") ? ConsoleColor.DarkRed : ConsoleColor.DarkGreen;
                    funcs[i].Trace(0);
                }
            //
            TraceLLVM();
            Console.ResetColor();
            int allign = 6;
            Console.WriteLine(String.Format("\nTime spend, seconds:\n    {0} - text parsed and trimmed\n    {1} - AST tree builded\n    {2} - LLVM IR generated\n"
                , (generatedTime[0] / 1000.0).ToString().PadRight(allign), (generatedTime[1] / 1000.0).ToString().PadRight(allign), (generatedTime[2] / 1000.0).ToString().PadRight(allign)));
            return;
        }

        static List<char> brStack = new List<char>();
        void ClearTree()
        {
            funcs = new List<ASTFunction>();
            tokens = new List<IASTtoken>();
            variables = new List<ASTvariable>();
            MISC.ClearStack();
            GlobalVars = new CommandOrder();
            MISC.ResetAdressing();
            MISC.LLVMtmpNumber = 0;
        }

        public ASTTree(string s, string path)
        {
            LLVM.CurrentCode = "";
            this.path = path;
            string sTrim = "";
            ClearTree();

            data = DateTime.Now;

            sTrim = FuncTrimmer(s); // remove last ^
            original = s;
            string[] funcParseMaterial = sTrim.Split('^');
            generatedTime.Add((DateTime.Now - data).Seconds * 1000 + (DateTime.Now - data).Milliseconds); data = DateTime.Now;
            try
            {
                for (int i = 0; i < funcParseMaterial.Length; i++)
                {
                    if (funcParseMaterial[i].IndexOf("(") >= 0)
                        funcs.Add(new ASTFunction(funcParseMaterial[i]));
                    else
                    {
                        IOperation def = BinaryOperation.ParseFrom(funcParseMaterial[i]);
                        if ((def as Assum) == null && (def as Define) == null)
                            throw new Exception("Can not parse function or define:\t " + MISC.StringFirstLetters(funcParseMaterial[i], 20, true));
                        else
                            GlobalVars.MergeWith(new CommandOrder(new ICommand[] { def }));
                    }
                }
                // after function declaration we have int foo(); int foo(){return 0;}; need to make them a one function
                for (int i = 0; i < funcs.Count; i++)
                    for (int j = i + 1; j < funcs.Count; j++)
                        if (funcs[i] != null && funcs[j] != null)
                            if (MISC.CompareFunctionSignature(funcs[i], funcs[j]))
                            { funcs[i] = funcs[j]; funcs[j] = null; }
                
            }
            catch (Exception e)
            {
                MISC.ConsoleWriteLine("ERROR:\n" + e.Message, ConsoleColor.Red);
                ClearTree();
                return;
            } 
            generatedTime.Add((DateTime.Now - data).Seconds * 1000 + (DateTime.Now - data).Milliseconds);
        }

        static string FuncTrimmer(string s)
        {
            string[] deleteWords = new string[] { "const " };
            //string[] deleteWordsPrefixes = new string[] { "__" };
            //string[] deleteLinesPrefixes = new string[] { "#", "extern" };
            string res = "";
            int brL = 0, isStr = 0, isCmt = 0, isChar = 0;

            s += " \n";
            for (int i = 0; i < s.Length; i++)
            {
                string subs = s.Substring(i);
                for (int w = 0; w < deleteWords.Length; w++)
                    if (subs.IndexOf(deleteWords[w]) == 0)
                    { i += deleteWords[0].Length - 1; continue; }
                //for (int w = 0; w < deleteWordsPrefixes.Length; w++)
                //    if (subs.IndexOf(deleteWordsPrefixes[w]) == 0)
                //    { i += subs.IndexOf(' '); continue; }
                //for (int w = 0; w < deleteWordsPrefixes.Length; w++)
                //    if (subs.IndexOf(deleteLinesPrefixes[w]) == 0)
                //    { i += subs.IndexOf('\n'); continue; }

                string add = s[i] + "";
                if (isStr == 0 && isChar == 0 && isCmt == 0)
                {
                    if (s[i] == ' ' || s[i] == '\t' || s[i] == '\n' || s[i] == '\r')
                        add = "";

                    if (s[i] == '{') brL++;
                    if (s[i] == '}')
                    {
                        brL--;
                        if (brL == 0) add += '^';   // function separator
                    }
                    if (s[i] == ';' && brL == 0)
                        add = "^";

                    if (s[i] == '\"')
                        isStr = 1;
                    if (s[i] == '\'')
                        isChar = 1;
                    if (s[i] == '/' && i < s.Length - 2 && s[i + 1] == '/')
                    {
                        add = ""; isCmt = 1;
                    }
                    if (s[i] == '/' && i < s.Length - 2 && s[i + 1] == '*')
                    {
                        add = ""; isCmt = 2;
                    }
                }
                else
                {
                    if (isCmt == 0)
                    {
                        if (isStr == 0 && isChar > 0 && i > 0 && s[i] == '\'' && (s[i - 1] != '\\' || (s[i - 1] == '\\' && i > 1 && s[i - 2] == '\\')))
                            isChar = 0;
                        if (isChar == 0 && isStr > 0 && i > 0 && s[i] == '\"' && (s[i - 1] != '\\' || (s[i - 1] == '\\' && i > 1 && s[i - 2] == '\\')))
                            isStr = 0;
                    }
                    else
                    {
                        add = "";
                        if (isCmt == 1 && s[i] == '\n') isCmt = 0;
                        if (isCmt == 2 && s[i] == '*' && i < s.Length - 2 && s[i + 1] == '/') { isCmt = 0; i++; }
                    }

                }
                res += add;
            }

            if (res.Length < 1)
                throw new Exception("Empty function on input!");
            return res.Remove(res.Length - 1); ;
        }
    }



    //public enum ValueType
    //{
    //    Cint = 0,
    //    Cdouble = 1,
    //    Cchar = 2,
    //    Cstring = 3,
    //    Cboolean = 4,
    //    Carray = 5,
    //    Cvariable = 6,
    //    Cvoid = 7,
    //    Cadress = 8,
    //    Unknown = 9
    //}

    public enum VT
    {
        Cunknown = 0,
        Cint = 1,
        Cdouble = 2,
        Cchar = 3,
        Cstring = 4,
        Cboolean = 5,
        Cvoid = 6,
        Cadress = 7
    }

    public struct ValueType
    {
        public VT rootType;
        public int pointerLevel;

        public override string ToString()
        {
            string res = rootType.ToString();
            for (int i = 0; i < pointerLevel; i++)
                res += "*";
            return res;
        }
        public string ToLLVM()
        {
            string pointer = "".PadLeft(pointerLevel, '*');
            switch (rootType)
            {
                case VT.Cint:
                    return "i32" + pointer;
                case VT.Cdouble:
                    return "f64" + pointer;
                case VT.Cchar:
                    return "i8" + pointer;
                case VT.Cboolean:
                    return "i1" + pointer;
                case VT.Cvoid:
                    return "void" + pointer;
                case VT.Cstring:
                    return "i8*" + pointer;
                default:
                    return "???" + pointer; ;
            }
        }

        public ValueType TypeOfPointerToThis()
        {
            return new ValueType(rootType, pointerLevel + 1);
        }

        public ValueType TypeOfPointedByThis()
        {
            if (pointerLevel <= 0)
                throw new Exception("Not pointer type can not point to anything!");
            return new ValueType(rootType, pointerLevel - 1);
        }

        public ValueType(VT type, int level)
        {
            rootType = type;
            pointerLevel = level;
        }

        public ValueType(VT type)
        {
            rootType = type;
            pointerLevel = 0;
        }

        public static bool operator ==(ValueType obj1, ValueType obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }
            if (obj1.rootType == VT.Cstring && obj2.rootType == VT.Cchar && obj2.pointerLevel == obj1.pointerLevel + 1)
                return true;
            if (obj2.rootType == VT.Cstring && obj1.rootType == VT.Cchar && obj1.pointerLevel == obj2.pointerLevel + 1)
                return true;
            return (obj1.pointerLevel == obj2.pointerLevel && obj1.rootType == obj2.rootType);
        }

        public static bool operator !=(ValueType obj1, ValueType obj2)
        {
            return !(obj1 == obj2);
        }

        public static bool operator ==(ValueType obj1, VT obj2)
        {
            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return (obj1.pointerLevel == 0 && obj1.rootType == obj2);
        }

        public static bool operator !=(ValueType obj1, VT obj2)
        {
            return !(obj1 == obj2);
        }
    }

    public struct TypeConvertion
    {
        public List<ValueType>[] from;
        public ValueType[] to;

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < to.Length; i++, res += "\n")
            {
                res += to[i].ToString().Substring(1) + " ( ";
                for (int j = 0; j < from[i].Count; j++, res += (from[i].Count > j) ? ", " : "")
                    res += from[i][j].ToString().Substring(1);
                res += " )";
            }
            return res;
        }

        public TypeConvertion(List<ValueType> input, ValueType res)
        {
            from = new List<ValueType>[] { input };
            to = new ValueType[] { res };
        }

        public TypeConvertion(string s, int IOcount)
        {
            IOcount++;
            List<List<ValueType>> flist = new List<List<ValueType>>();
            List<ValueType> tlist = new List<ValueType>();
            List<ValueType> inputVals = new List<ValueType>();
            for (int i = 0; i < s.Length; i++)
            {
                ValueType vt;
                switch (s[i])
                {
                    case 'I':
                        vt = new ValueType(VT.Cint); break;
                    case 'D':
                        vt = new ValueType(VT.Cdouble); break;
                    case 'B':
                        vt = new ValueType(VT.Cboolean); break;
                    case 'C':
                        vt = new ValueType(VT.Cchar); break;
                    case 'S':
                        vt = new ValueType(VT.Cstring); break;
                    case '_':
                        vt = new ValueType(VT.Cvoid); break;
                    case 'A':
                        vt = new ValueType(VT.Cadress); break;
                    case 'V':
                        vt = new ValueType(VT.Cvoid); break;
                    default:
                        vt = new ValueType(VT.Cunknown); break;
                }
                if (i % IOcount == IOcount - 1)
                    tlist.Add(vt);
                else
                {
                    if (i % IOcount < IOcount - 2)
                        inputVals.Add(vt);
                    else
                    {
                        inputVals.Add(vt);
                        flist.Add(inputVals.ToArray().ToList());
                        inputVals.Clear();
                    }
                }
            }
            from = flist.ToArray<List<ValueType>>();
            to = tlist.ToArray();
        }
    }
}
