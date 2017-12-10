using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public interface ICommand : IAstNode
    {

    }


    public interface IOperation : ICommand
    {
        ValueType returnTypes();
    }

    public abstract class MonoOperation : IOperation
    {
        protected string operationString = "???";
        protected ValueType returnType;
        protected TypeConvertion atArg;
        protected IOperation a;   // pointer
        public virtual void Trace(int depth)
        {
            Console.Write(MISC.tabs(depth)); MISC.ConsoleWrite(operationString, ConsoleColor.Yellow); MISC.ConsoleWriteLine(" [" + returnTypes().ToString() + "]", ConsoleColor.DarkGreen);

            MISC.finish = true;
            //if (a != null)
            a.Trace(depth + 1);
            //else
            //    Console.WriteLine(MISC.tabs(depth + 1) + " NULL");
        }
        public static IOperation ParseFrom(string s)
        {
            if (s.IndexOf('{') == 0 && s.LastIndexOf('}') == s.Length - 1)
                return new StructureDefine(MISC.getIn(s, 0));
            if (s.IndexOf('(') == 0 && s.LastIndexOf(')') == s.Length - 1)
                return BinaryOperation.ParseFrom(MISC.breakBrackets(s));


            if (s.Length > 2 && s.IndexOf("--") == s.Length - 2)
                return new Dscr(ParseFrom(s.Substring(0, s.Length - 2)));

            if (s.Length > 2 && s.IndexOf("++") == s.Length - 2)
                return new Incr(ParseFrom(s.Substring(0, s.Length - 2)));

            if (s.IndexOf("-") == 0)
                return new Mins(ParseFrom(s.Substring(1, s.Length - 1)));

            if (s.IndexOf("!") == 0)
                return new Nega(ParseFrom(s.Substring(1, s.Length - 1)));

            if (s.IndexOf("&") == 0)
            {
                IOperation gettingAdressOf = ParseFrom(s.Substring(1, s.Length - 1));

                return new Adrs(gettingAdressOf);
            }

            try
            {
                return new ASTFunctionCall(s);
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("is not a function") < 0)
                    throw new Exception(e.Message);
            }

            int varType = Math.Max((s.IndexOf("int") >= 0) ? 2 : -1, Math.Max((s.IndexOf("double") >= 0) ? 5 : -1, Math.Max((s.IndexOf("char") >= 0) ? 3 : -1,
                Math.Max((s.IndexOf("string") >= 0) ? 5 : -1, (s.IndexOf("bool") >= 0) ? 3 : -1))));

            if (varType >= 0)
            {
                s = s.Insert(varType + 1, "$");
                return new Define(s);
                //string firstPart = s.Substring(0, s.IndexOf("$"));  // int&a,b
                //bool multipleDefines = false;
                //while (s.IndexOf(',') > 0)
                //{
                //    int at = s.IndexOf(',');
                //    s = s.Substring(0, s.IndexOf(',')) + ';' + firstPart + s.Substring(s.IndexOf(',') + 1);
                //    multipleDefines = true;
                //}
                //if (!multipleDefines)
                //    return new Define(s);
                //else
                //{
                //    s = s.Remove(s.IndexOf('$'), 1);
                //    throw new Exception("#MDS:" + s);
                //}
            }

            if (s.IndexOf("*") == 0)
            {
                IOperation pointTo = ParseFrom(s.Substring(1, s.Length - 1));
                return new GetValByAdress(pointTo, (pointTo).returnTypes());
                throw new Exception("Invalid pointer selected!");
            }
            if (s.LastIndexOf("]") == s.Length - 1 && s.IndexOf("[") > 0)
            {

                //IOperation pointTo = ;
                //return 
                //    new GetValByAdress(new Summ(pointTo, 
                //        BinaryOperation.ParseFrom(MISC.getIn(s, s.IndexOf('[')))),
                //        (pointTo).returnTypes());
                //throw new Exception("Invalid pointer selected!");


                string sContainBrackets = s.Substring(s.IndexOf("["));
                List<string> getedBrs = MISC.splitByQuad(sContainBrackets);

                IOperation resOper = ParseFrom(s.Substring(0, s.IndexOf('[')));

                for (int o = 0; o < getedBrs.Count; o++)
                {
                    IOperation currentBrsOp = BinaryOperation.ParseFrom(getedBrs[o]);
                    resOper = new GetValByAdress(new Summ(resOper, currentBrsOp), resOper.returnTypes());
                }
                return resOper;
            }
            //f (s.IndexOf('(') == 0 && s.LastIndexOf(')') == s.Length - 1)
            //return ParseFrom(MISC.breakBrackets(s));



            try
            {
                return new ASTvalue(s);
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("GetAddr") == 0)
                {
                    int newAdress = int.Parse(e.Message.Split('_')[1]);
                    return new GetValByAdress(new ASTvalue(new ValueType(VT.Cadress), (object)newAdress),
                                              ASTTree.variables[newAdress].getValueType);
                }
                throw new Exception(e.Message);
            }
        }

        public virtual ValueType returnTypes()
        {
            return returnType;
        }
    }

    public abstract class BinaryOperation : IOperation
    {
        protected string operationString = "???";
        protected ValueType returnType;
        // acceptable left and right types
        static int lastIndex = -1;
        protected IOperation a;   // pointer
        protected IOperation b;   // pointer
        public virtual void Trace(int depth)
        {
            Console.Write(MISC.tabs(depth)); MISC.ConsoleWrite(operationString, ConsoleColor.Yellow); MISC.ConsoleWriteLine("   " + returnType.ToString(), ConsoleColor.DarkGreen);
            a.Trace(depth + 1);
            MISC.finish = true;
            b.Trace(depth + 1);
        }

        static bool onLevel(string s, string symbols, int level)
        {
            lastIndex = onLevel(s, symbols, level, 0);
            return (lastIndex > -1);
        }

        static int onLevel(string s, string symbols, int level, int from)
        {
            if (s.IndexOf(symbols) < 0)
                return -1;
            if (symbols == "*")
            { int n = 0; }
            int nowLevel = 0,
                pos = s.IndexOf(symbols, from);
            if (pos == -1)
                return -1;

            for (int i = 0, ll = 0; i < s.Length; i++)
            {
                if (s[i] == '(' || s[i] == '[')
                    ll++;
                if (s[i] == ')' || s[i] == ']')
                    ll--;

                if (i > from && i + symbols.Length <= s.Length && ll == 0 && s.Substring(i, symbols.Length) == symbols)
                { pos = i; break; }
            }


            if (pos == -1 || pos >= s.Length - 1)
                return -1;
            // if it is a part of complex symbol then try further
            if (symbols.Length == 1 && pos < s.Length - 1 && pos > 0)
            {
                // kost for <= >=
                if (symbols == "=" && s[pos - 1] == '<' || s[pos - 1] == '>')
                    return onLevel(s, symbols, level, pos + 1);

                switch (s[pos + 1])
                {
                    // == += -= *= /= <= >= in case of =
                    case '=':
                        if (symbols == "=" || symbols == "+" || symbols == "-" || symbols == "*" || symbols == "/" || symbols == "<" || symbols == ">")
                            return onLevel(s, symbols, level, pos + 1);
                        break;
                    // ++
                    case '+':
                        if (symbols == "+")
                            return onLevel(s, symbols, level, pos + 1);
                        break;
                    // --
                    case '-':
                        if (symbols == "-")
                            return onLevel(s, symbols, level, pos + 1);
                        break;
                    default:
                        break;
                }
                if ((s[pos - 1] == s[pos] || s[pos + 1] == s[pos]) && (s[pos] == '=' || s[pos] == '-' || s[pos] == '+'))
                    return onLevel(s, symbols, level, pos + 1);
            }

            for (int i = 0; i < pos; i++)
            {
                if (s[i] == '(' || s[i] == '[')
                    nowLevel++;
                if (s[i] == ')' || s[i] == ']')
                    nowLevel--;
            }
            return (nowLevel == level) ? pos : -1;
        }

        /*for (int i = 0; i < s.Length + 1 - symbols.Length; i++)
            {
                if (s[i] == leftBracket)
                    nowLevel++;
                if (s[i] == rightBracket)
                    nowLevel--;
                if (nowLevel == 0 && s.Substring(i, symbols.Length) == symbols)
                    pos = i;
            }*/

        public static IOperation ParseFrom(string s)
        {
            try
            {
                string left = "", right = "";
                if (s.IndexOf("return") == 0)
                { return new Ret(ParseFrom(s.Substring(6))); }

                //___________
                int varType = Math.Max((s.IndexOf("int") >= 0) ? 2 : -1, Math.Max((s.IndexOf("double") >= 0) ? 5 : -1, Math.Max((s.IndexOf("char") >= 0) ? 3 : -1,
                Math.Max((s.IndexOf("string") >= 0) ? 5 : -1, (s.IndexOf("bool") >= 0) ? 3 : -1))));
                if (varType > 0 && MISC.IndexOfOnLevel0(s, ",", 0) > 0)
                {
                    s = s.Insert(varType + 1, "$");
                    string firstPart = s.Substring(0, s.IndexOf("$"));  // int&a,b

                    int at;
                    do
                    {
                        at = MISC.IndexOfOnLevel0(s, ",", 0);
                        if (at >= 0) s = s.Substring(0, at) + ';' + firstPart + s.Substring(at + 1);
                    } while (at >= 0);

                    s = s.Remove(s.IndexOf('$'), 1);
                    throw new Exception("#MDS:" + s);
                }
                //_________________
                if (s.IndexOf('{') == 0 && s.LastIndexOf('}') == s.Length - 1)
                    return MonoOperation.ParseFrom(s);

                if (!onLevel(s, "==", 0) && !onLevel(s, "!=", 0) && onLevel(s, "=", 0))
                {
                    MISC.separate(s, "=", ref left, ref right, lastIndex);

                    IOperation rightOperation = ParseFrom(right);
                    if (left.Length > 0 && ("+-*/").IndexOf(left[left.Length - 1]) >= 0)
                    {

                        IOperation leftMinusOne = MonoOperation.ParseFrom(left.Remove(left.Length - 1));
                        switch (left[left.Length - 1])
                        {
                            case '+':
                                return new Assum(leftMinusOne, new Summ(leftMinusOne, rightOperation));
                            case '-':
                                return new Assum(leftMinusOne, new Diff(leftMinusOne, rightOperation));
                            case '*':
                                return new Assum(leftMinusOne, new Mult(leftMinusOne, rightOperation));
                            case '/':
                                return new Assum(leftMinusOne, new Qout(leftMinusOne, rightOperation));
                            default:
                                break;
                        }
                    }

                    IOperation leftOperation = MonoOperation.ParseFrom(left);

                    if ((rightOperation as StructureDefine) == null)
                        return new Assum(leftOperation, rightOperation);
                    else
                    {
                        Assum needUPdatedAssum = new Assum(leftOperation, rightOperation);
                        needUPdatedAssum.requiredUpdate = "structdefineinfor";
                        return needUPdatedAssum;
                    }
                }

                if (onLevel(s, "==", 0))
                {
                    MISC.separate(s, "==", ref left, ref right, lastIndex);
                    return new Equal(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "!=", 0))
                {
                    MISC.separate(s, "!=", ref left, ref right, lastIndex);
                    return new Uneq(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "<=", 0))
                {
                    MISC.separate(s, "<=", ref left, ref right, lastIndex);
                    return new LsEq(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, ">=", 0))
                {
                    MISC.separate(s, ">=", ref left, ref right, lastIndex);
                    return new MrEq(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "<", 0))
                {
                    MISC.separate(s, "<", ref left, ref right, lastIndex);
                    return new Less(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, ">", 0))
                {
                    MISC.separate(s, ">", ref left, ref right, lastIndex);
                    return new More(ParseFrom(left), ParseFrom(right));
                }

                if (onLevel(s, "+", 0))
                {
                    MISC.separate(s, "+", ref left, ref right, lastIndex);
                    return new Summ(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "-", 0))
                {
                    MISC.separate(s, "-", ref left, ref right, lastIndex);
                    if (left.Length == 0)
                        return MonoOperation.ParseFrom("-" + right);
                    return new Diff(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "*", 0) && s.IndexOf("*") > 0)
                {
                    MISC.separate(s, "*", ref left, ref right, lastIndex);
                    // can be initialization of pointer!
                    if (new ValueType(VT.Cunknown) == Define.detectType(left))
                        return new Mult(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "/", 0))
                {
                    MISC.separate(s, "/", ref left, ref right, lastIndex);
                    return new Qout(ParseFrom(left), ParseFrom(right));
                }

                if (onLevel(s, "||", 0))
                {
                    MISC.separate(s, "||", ref left, ref right, lastIndex);
                    return new ORS(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "|", 0))
                {
                    MISC.separate(s, "|", ref left, ref right, lastIndex);
                    return new OR(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "&&", 0))
                {
                    MISC.separate(s, "&&", ref left, ref right, lastIndex);
                    return new ANDS(ParseFrom(left), ParseFrom(right));
                }
                if (onLevel(s, "&", 0) && s.IndexOf("&") > 0)
                {
                    MISC.separate(s, "&", ref left, ref right, lastIndex);
                    return new AND(ParseFrom(left), ParseFrom(right));
                }


                if (s.IndexOf('(') == 0 && s.LastIndexOf(')') == s.Length - 1)
                    return ParseFrom(MISC.breakBrackets(s));
                try
                {
                    return UniqOperation.ParseFrom(s);
                }
                catch (Exception e)
                {
                    return MonoOperation.ParseFrom(s);
                }
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("#MDS:") != 0)
                    throw new Exception("Can not parse \"" + s + "\"\n" + e.Message);
                else
                    throw new Exception(e.Message);
            }

        }
        public ValueType returnTypes()
        {
            return returnType;
        }
    }
}
/*
           if (symbols == "=" || symbols == "+" || symbols == "-" || symbols == "<" || symbols == ">")
           {
               for (int i = 1; i < s.Length - 1; i++)
                   if (((s[i] == '=' || s[i] == '<' || s[i] == '>') && s[i - 1] != '=' && s[i + 1] != '=')
                       || ((s[i] == '+' || s[i] == '-') && s[i + 1] != '=' && s[i - 1] != '+' && s[i - 1] != '-' && s[i + 1] != '+' && s[i + 1] != '-'))
                   { pos = i; break; }
           }
           else
           {
               pos = s.IndexOf(symbols);   // if it is double symbol or somthing else
           }*/
