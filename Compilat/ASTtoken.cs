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
        }
        public ASTvalue(string s)
        {
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
                    { this.valType = new ValueType(VT.Cchar); this.data = (object)(s[1]); clr = ConsoleColor.DarkCyan; }
                    else
                    { throw new Exception("Char can not be more than 1 symbol"); }
                }
                else
                {
                    // detect string
                    if (s.IndexOf('\"') == 0 && s.LastIndexOf('\"') == s.Length - 1)
                    { this.valType = new ValueType(VT.Cstring); this.data = (object)(s.Substring(1, s.Length - 2)); clr = ConsoleColor.DarkCyan; }
                    else
                    {
                        if (s.ToLower() == "true" || s.ToLower() == "false")
                        {
                            this.valType = new ValueType(VT.Cboolean); this.data = (object)((s.ToLower() == "true"));
                            clr = ConsoleColor.Gray;
                        }
                        else
                        {
                            // finally trying to find variable
                            string varName = s;
                            int found = -1;
                            ASTvariable foundedVar = new ASTvariable(new ValueType(VT.Cunknown), "NONE", 0);

                            for (int i = 0; i < ASTTree.variables.Count; i++)
                                if (ASTTree.variables[i].name == varName && MISC.isVariableAvailable(i))
                                { foundedVar = ASTTree.variables[i]; found = i; break; }

                            if (found < 0)
                                throw new Exception("Used a variable \"" + varName + "\", that was never defined in this context!");
                            else
                            {
                                this.valType = foundedVar.getValueType;
                                throw new Exception("GetAddr_" + found);
                            }
                        }
                    }
                }
            }
            ASTTree.tokens.Add(this);
        }

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
    }

    public class ASTvariable : IASTtoken
    {

        ValueType valType;
        public string name;

        int adress;
        string localSpace;

        public ASTvariable()
        {
            this.valType = new ValueType(VT.Cunknown);
            this.name = "-";
            this.adress = -1;
            this.localSpace = string.Join("/", MISC.nowParsing.ToArray());
        }
        public ASTvariable(ValueType vt, string name, int level)
        {
            this.valType = vt;
            this.name = name;
            // check variable name with function collision
            for (int i = 0; i < ASTTree.funcs.Count; i++)
                if (name == ASTTree.funcs[i].getName)
                    throw new Exception("Variable \""+name+"\" can not conflict with function : "+ ASTTree.funcs[i].getArgsString);
            //
            this.adress = ASTTree.variables.Count;
            this.localSpace = string.Join("/", MISC.nowParsing.ToArray());
        }

        public virtual void Trace(int depth)
        {
            //Console.WriteLine(String.Format("{0}${1}   [{2}]", MISC.tabs(depth), name, valType.ToString()));
            Console.Write(MISC.tabs(depth));
            MISC.ConsoleWrite(pointerMuch(valType.pointerLevel), ConsoleColor.Red);
            MISC.ConsoleWrite(name, ConsoleColor.Green);
            MISC.ConsoleWriteLine("\t[" + getValueType.ToString() + "]", ConsoleColor.DarkGreen);
        }
        public virtual void TraceMore(int depth)
        {
            MISC.ConsoleWrite(String.Format("\t{0}\t\t{1}\t\t", pointerMuch(valType.pointerLevel) + name, getValueType.ToString().Substring(1)), ConsoleColor.DarkGreen);
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
    }
}
