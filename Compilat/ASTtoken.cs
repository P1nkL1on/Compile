using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public interface IASTtoken : IOperation
    {
        void Trace(int depth);
        void TraceMore(int depth);

        ValueType getValueType { get; }
        Object getValue { get; }
        ValueType returnTypes();

        int getPointerLevel { get; }
    }

    public class ASTvalue : IASTtoken
    {
        ValueType valType;
        Object data;
        ConsoleColor clr;
        string LLVMname;
        string array_type;

        public ASTvalue(ValueType vt, Object data)
        {
            this.valType = vt;
            this.data = data;
            ASTTree.tokens.Add(this);
            clr = ConsoleColor.DarkCyan;
            if (vt == VT.Cchar || vt == VT.Cstring) clr = ConsoleColor.DarkCyan;
            if (vt == VT.Cint || vt == VT.Cdouble) clr = ConsoleColor.DarkCyan;
            if (vt == VT.Cboolean) clr = ConsoleColor.Gray;
            if (vt == VT.Cadress) clr = ConsoleColor.DarkGray;
            if (vt == VT.Cboolean && vt.pointerLevel == 0)
            { LLVMname = ((bool)data).ToString(); }
        }
        public ASTvalue(string s, bool calledFromDefined)
        {
            LLVMname = "?LLVMNAME?";
            string nums = "-1234567890.";
            bool isnum = true;
            int numPoints = 0;
            // cheking if is a nubmer
            for (int i = 0; i < s.Length; i++)
            {
                if (nums.IndexOf(s[i]) < 0) { isnum = false; break; }
                if (s[i] == '.') { numPoints++; if (numPoints > 1) { isnum = false; break; } }
                if (i > 0 && s[i] == '-') { isnum = false; break; }
            }
            // calculate a number
            if (isnum)
            {
                if (numPoints == 0) { this.valType = new ValueType(VT.Cint); this.data = (object)(int.Parse(s)); clr = ConsoleColor.DarkCyan; }
                else { this.valType = new ValueType(VT.Cdouble); this.data = (object)(double.Parse(s.Replace('.', ','))); clr = ConsoleColor.DarkCyan; }
            }
            else
            {
                // detect char
                if (s.IndexOf('\'') == 0 && s.LastIndexOf('\'') == s.Length - 1)
                {
                    if (s.Length == 3 || (s.Length == 4 && s[1] == '\\'))
                    { this.valType = new ValueType(VT.Cchar); this.data = (object)(s[1]); LLVMname = s[1] + ""; clr = ConsoleColor.DarkCyan; }
                    else
                    { throw new Exception("Char can not be more than 1 symbol"); }
                }
                else
                {
                    // detect string
                    if (s.IndexOf('\"') == 0 && s.LastIndexOf('\"') == s.Length - 1)
                    {
                        this.valType = new ValueType(VT.Cstring);

                        string STR = s.Substring(1, s.Length - 2);
                        this.data = (object)(STR);
                        clr = ConsoleColor.DarkCyan;

                        string STRLLVM = STR.Replace("\\r", "").Replace("\\n", "\\0A") + "\\00";

                        int leng = STRLLVM.Length;
                        for (int sk = 0; sk < STRLLVM.Length - 1; sk++)
                            if (STRLLVM[sk] == '\\')
                                leng -= 2;
                        LLVM.AddToCode(String.Format("@str{0} = private unnamed_addr constant [{1} x i8] c\"{2}\"\n", LLVM.globalVars, leng, STRLLVM));
                        this.array_type = "[" + leng + " x i8]";
                        LLVMname = "str" + LLVM.globalVars;
                        LLVM.globalVars++;
                    }
                    else
                    {
                        if (s.ToLower() == "true" || s.ToLower() == "false")
                        {
                            this.valType = new ValueType(VT.Cboolean); this.data = (object)((s.ToLower() == "true"));
                            clr = ConsoleColor.Gray; LLVMname = s.ToLower();
                        }
                        else
                        {
                            // finally trying to find variable
                            string varName = s;
                            int found = -1;
                            ASTvariable foundedVar = new ASTvariable(new ValueType(VT.Cunknown), "NONE", -1, new AdressType(-1, VAT.Unknown));

                            for (int i = 0; i < ASTTree.variables.Count; i++)
                                if (ASTTree.variables[i].name == varName && MISC.isVariableAvailable(i, calledFromDefined))
                                { foundedVar = ASTTree.variables[i]; found = i; break; }

                            if (found < 0)
                                throw new Exception("Used a variable \"" + varName + "\", that was never defined in this context!");
                            else
                            {
                                if (!calledFromDefined)
                                    foundedVar.everUsed++;
                                this.valType = foundedVar.getValueType;
                                throw new Exception("GetAddr_" + found);
                            }
                        }
                    }
                }
            }
            ASTTree.tokens.Add(this);
        }
        public virtual string ToLLVM(int depth)
        {

            //return String.Format("{0}",
            //    (returnTypes() == new ValueType(VT.Cdouble) ?
            //    (data.ToString() + ((data.ToString().IndexOf(",") < 0) ? "," : "")).Replace(',', '.').PadRight(6, '0')    // недостающие нули в записи с плавающей запятой
            //    : data.ToString()));
            switch (returnTypes().rootType)
            {
                case VT.Cdouble:
                    string res = (data.ToString().Replace(',', '.'));
                    if (res.IndexOf('.') < 0) res += ".0";
                    return res;
                case VT.Cint:
                    return data.ToString();
                case VT.Cstring:
                    return String.Format("getelementptr ({0}, {0}* @{1}, i64 0, i64 0)", array_type, LLVMname);//"@" + LLVMname;
                case VT.Cchar:
                    return ((int)(LLVMname[0])).ToString();
                case VT.Cboolean:
                    return (LLVMname.ToLower() == "true") ? "1" : "0";
                default:
                    return "???";
            }
        }
        //public virtual string ToLLVMwtType()
        //{
        //    return String.Format("{0} {1}", returnTypes().ToLLVM(), ToLLVM());
        //}
        public void Trace(int depth)
        {
            string br = "";
            if (this.getValueType == VT.Cstring) br = "\"";
            if (this.getValueType == VT.Cchar) br = "\'";
            if (this.getValueType == VT.Cadress) br = "#";
            if (data == null)
            {
                //Console.WriteLine(String.Format("{0}{1}", MISC.tabs(depth), "null"));
                Console.Write(MISC.tabs(depth));
                MISC.ConsoleWriteLine("null", ConsoleColor.Red);
            }
            else
            {
                //Console.WriteLine(String.Format("{0}{1}", MISC.tabs(depth), (br + data.ToString() + br)));
                Console.Write(MISC.tabs(depth));
                MISC.ConsoleWriteLine((br + data.ToString() + br), clr);
            }
        }
        public void TraceMore(int depth)
        {
            string br = "";
            if (this.getValueType == VT.Cstring) br = "\"";
            if (this.getValueType == VT.Cchar) br = "\'";
            if (data == null)
                MISC.ConsoleWriteLine(String.Format("\tnull\t\t[{0}]", valType.ToString()), clr);
            else
                MISC.ConsoleWriteLine(String.Format("\t{0}\t\t[{1}]", (br + data.ToString() + br), valType.ToString()), clr);
        }

        public ValueType getValueType { get { return valType; } }
        public Object getValue { get { return data; } }
        public ValueType returnTypes() { return valType; }

        public int getPointerLevel { get { return 0; } }

        public override string ToString()
        {
            return valType.ToString();
        }
    }

    public enum VAT
    {
        Local = 3,
        Global = 1,
        Parameter = 2,
        Unknown = 0
    }
    public struct AdressType
    {
        public int adressId;
        public VAT typ;
        public AdressType(int id, VAT typ)
        {
            this.adressId = id; this.typ = typ;
        }
        public override string ToString()
        {
            string res = "";
            switch (typ)
            {
                case VAT.Global:
                    res += "GLBL";
                    break;
                case VAT.Local:
                    res += " LCL";
                    break;
                case VAT.Parameter:
                    res += " PRM";
                    break;
                default:
                    res += "????";
                    break;
            }
            return res += " " + adressId;
        }
    }

    public class ASTvariable : IASTtoken
    {

        ValueType valType;
        public string name;

        public AdressType adress;
        string localSpace;

        public int everUsed;
        public int reloadedTimes;

        public ASTvariable()
        {
            reloadedTimes = 0;
            this.valType = new ValueType(VT.Cunknown);
            this.name = "-";
            this.adress = new AdressType(-1, VAT.Unknown);
            this.localSpace = string.Join("/", MISC.nowParsing.ToArray());
            everUsed = 0;
        }
        public ASTvariable(ValueType vt, string name, int level, AdressType adress)
        {
            reloadedTimes = 0;
            this.valType = vt;
            this.name = name;
            // check variable name with function collision
            for (int i = 0; i < ASTTree.funcs.Count; i++)
                if (name == ASTTree.funcs[i].getName)
                    throw new Exception("Variable \"" + name + "\" can not conflict with function : " + ASTTree.funcs[i].getArgsString);
            //
            this.adress = adress;//ASTTree.variables.Count;
            this.localSpace = string.Join("/", MISC.nowParsing.ToArray());
            everUsed = 0;
        }
        public string ToLLVM(int depth)
        {
            return String.Format(ToLLVM());
        }

        public virtual void Trace(int depth)
        {
            Console.Write(MISC.tabs(depth));
            MISC.ConsoleWrite(pointerMuch(valType.pointerLevel), ConsoleColor.Red);
            MISC.ConsoleWrite(name, ConsoleColor.Green);
            MISC.ConsoleWriteLine(" (" + getValueType.ToString().Substring(1) + "" + adress + ")", ConsoleColor.DarkGreen);

        }
        public virtual string ToLLVM()
        {
            return ((adress.typ == VAT.Global && reloadedTimes == 0) ? "@" : "%") + ((reloadedTimes > 0) ? "$" + reloadedTimes : "") + "_" + adress.adressId + name;
        }
        public virtual void TraceMore(int depth)
        {
            MISC.ConsoleWrite(String.Format("{2}\t{0}\t\t{1}\t\t{3}\t",
                pointerMuch(valType.pointerLevel) + name, getValueType.ToString().Substring(1), adress, everUsed), ConsoleColor.DarkGreen);
            MISC.ConsoleWriteLine(localSpace, ConsoleColor.DarkMagenta);
        }

        public virtual ValueType getValueType
        { get { return valType; } }

        public Object getValue
        { get { return null; } }

        public virtual ValueType returnTypes()
        { return getValueType; }

        public int getPointerLevel { get { return valType.pointerLevel; } }

        string pointerMuch(int x)
        {
            string res = "";
            for (int i = 0; i < x; i++)
            {
                res += "*";
            }
            return res;
        }

        public override string ToString()
        {
            return valType.ToString() + " " + name;
        }
    }
}
