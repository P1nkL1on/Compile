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
        public static List<IASTtoken> tokens = new List<IASTtoken>();
        public static List<ASTvariable> variables = new List<ASTvariable>();
        public static CommandOrder GlobalVars = new CommandOrder();

        public static ConsoleColor clr = ConsoleColor.Black;

        public void Trace()
        {

            if (funcs.Count <= 0)
                return;

            clr = ConsoleColor.Black;
            Console.WriteLine("\nTokens:");
            for (int i = 0; i < tokens.Count; i++)
                tokens[i].TraceMore(0);

            Console.WriteLine("\nVariables");
            for (int i = 0; i < variables.Count; i++)
                variables[i].TraceMore(0);


            Console.WriteLine("\nFunctions:");
            for (int i = 0; i < funcs.Count; i++)
            {
                if (funcs[i] != null)
                    MISC.ConsoleWriteLine(String.Format("  {0}: {1}", funcs[i].getName, funcs[i].getArgsString), ConsoleColor.Green);
                else
                    MISC.ConsoleWriteLine(String.Format("  {0}:", "null"), ConsoleColor.DarkGreen);
            }

            Console.WriteLine();
            if (GlobalVars.CommandCount > 0)
            {
                Console.Write(MISC.tabs(0));
                MISC.ConsoleWriteLine("Global vars:", ConsoleColor.Black, ConsoleColor.Cyan);
                GlobalVars.Trace(1);
            }
            for (int i = 0; i < funcs.Count; i++)
                if (funcs[i] != null)
                {
                    clr = (funcs[i].getName.ToLower() == "main") ? ConsoleColor.DarkRed : ConsoleColor.DarkGreen;
                    funcs[i].Trace(0);
                }
        }
        static List<char> brStack = new List<char>();
        void ClearTree()
        {
            funcs = new List<ASTFunction>();
            tokens = new List<IASTtoken>();
            variables = new List<ASTvariable>();
            MISC.ClearStack();
            GlobalVars = new CommandOrder();
        }

        public ASTTree(string s)
        {
            string sTrim = "";
            ClearTree();

            sTrim = FuncTrimmer(s); // remove last ^
            original = s;
            string[] funcParseMaterial = sTrim.Split('^');
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
        }

        static string FuncTrimmer(string s)
        {
            string res = "";
            int brL = 0, isStr = 0, isCmt = 0, isChar = 0;

            for (int i = 0; i < s.Length; i++)
            {
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
