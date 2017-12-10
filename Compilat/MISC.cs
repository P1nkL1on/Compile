using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Compilat
{
    public class MISC
    {

        public static void ClearStack()
        {

            availableConvertation = new List<Tuple<ValueType, ValueType>>();
            nowParsing = new List<string>();
            levelVariables = new List<List<int>>();
            isSlide = new List<bool>();
            nowOpen = "│ ";
            lastDepth = 0;
            finish = false;
            rmColomn = false;

        }
        public static void ConsoleWrite(string S, ConsoleColor clr)
        {
            Console.ForegroundColor = clr;
            Console.Write(S);
            Console.ForegroundColor = ConsoleColor.Black;
        }
        public static void ConsoleWriteLine(string S, ConsoleColor clr)
        {
            Console.ForegroundColor = clr;
            Console.WriteLine(S);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void ConsoleWrite(string S, ConsoleColor clr, ConsoleColor Bclr)
        {
            Console.ForegroundColor = clr;
            Console.BackgroundColor = Bclr;
            Console.Write(S);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void ConsoleWriteLine(string S, ConsoleColor clr, ConsoleColor Bclr)
        {
            Console.ForegroundColor = clr;
            Console.BackgroundColor = Bclr;
            Console.WriteLine(S);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static List<Tuple<ValueType, ValueType>> availableConvertation = new List<Tuple<ValueType, ValueType>>();

        public static ValueType CheckTypeCorrect(IOperation o, TypeConvertion accept, ref IOperation[] parts)
        {
            try
            {
                ValueType[] inputValueTypes = new ValueType[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                    //if (parts[i].returnTypes().pointerLevel == 0)
                    inputValueTypes[i] = parts[i].returnTypes();
                //else
                //    inputValueTypes[i] = new ValueType(VT.Cadress);

                //if ((parts[i] as GetValByAdress) != null)
                //{
                //    ValueType vt = (parts[i] as GetValByAdress).pointerType;
                //    int n = 0;
                //}
                if ((o as Assum) != null && parts[0].returnTypes() == parts[1].returnTypes())
                    return parts[0].returnTypes();

                ValueType res = CheckType(accept, inputValueTypes);

                return res;
            }
            catch (Exception e)
            {
                string getErr = e.Message;
                string[] splitedTypes = getErr.Split('_');
                for (int i = 0; i < splitedTypes.Length; i++)
                    if (splitedTypes[i] != "-")
                    {
                        string parseFrom = splitedTypes[i].Substring(splitedTypes[i].IndexOf(", "),
                            splitedTypes[i].IndexOf(")") - splitedTypes[i].IndexOf(", ")).ToLower().Remove(0, 3);
                        ValueType convertType = new ValueType(Define.detectType(parseFrom));
                        parts[i] = new Conv(parts[i], convertType);
                    }
                try
                {
                    ValueType[] inputValueTypes = new ValueType[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                        inputValueTypes[i] = parts[i].returnTypes();

                    ValueType res = CheckType(accept, inputValueTypes);
                    return res;
                }
                catch (Exception r)
                {
                    throw new Exception("Can not apply convertation chain!");
                    return new ValueType(VT.Cunknown);
                }
            }
        }

        public static ValueType CheckType(TypeConvertion accept, params ValueType[] hadTypes)
        {
            // I D
            // IIB DDB CCB

            for (int i = 0; i < accept.from.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < accept.from[i].Count; j++)
                    if (hadTypes[j] != accept.from[i][j])
                        found = false;
                if (found)
                    return accept.to[i];
            }
            // we can found some kostils
            if (availableConvertation.Count == 0)
            {
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cboolean), new ValueType(VT.Cint)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cint), new ValueType(VT.Cdouble)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cint), new ValueType(VT.Cstring)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cint), new ValueType(VT.Cboolean)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cdouble), new ValueType(VT.Cstring)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cdouble), new ValueType(VT.Cboolean)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cchar), new ValueType(VT.Cstring)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cchar), new ValueType(VT.Cboolean)));
                availableConvertation.Add(new Tuple<ValueType, ValueType>(new ValueType(VT.Cchar), new ValueType(VT.Cint)));
            }

            // I -> D
            // checking in cyccle
            bool convertionFound = false;
            int[] convertion = new int[hadTypes.Length];
            for (int i = 0; i < convertion.Length; i++)
                convertion[i] = -1;                     // -1 -1 -1 -1 -1 -1 for each variable in signature

            for (int i = 0; i < accept.from.Length; i++)
            {
                bool foundAcceptance = true;
                for (int j = 0; j < hadTypes.Length; j++)
                {
                    bool geted = false;
                    if (hadTypes[j] != accept.from[i][j])
                        for (int k = 0; k < availableConvertation.Count; k++)
                        {
                            if (hadTypes[j] == availableConvertation[k].Item1
                                && accept.from[i][j] == availableConvertation[k].Item2)
                            { geted = true; convertion[j] = k; break; }
                        }
                    else
                        geted = true;
                    // if geted then we have conversion for a current parameter
                    if (!geted)
                        foundAcceptance = false;
                }
                if (foundAcceptance)
                {
                    int n = 0;
                    convertionFound = true;
                    break;
                }
            }
            //
            if (convertionFound)
            {
                string ConvertNeed = "";
                for (int i = 0; i < convertion.Length; i++)
                    if (convertion[i] >= 0)
                        ConvertNeed += "" + availableConvertation[convertion[i]] + "_";
                    else
                        ConvertNeed += "-_";
                throw new Exception(ConvertNeed.Remove(ConvertNeed.Length - 1));
            }
            //
            //return ValueType.Unknown;
            throw new Exception("DID NOT FOUND");
        }

        public static List<string> nowParsing = new List<string>();
        static List<List<int>> levelVariables = new List<List<int>>();

        public static void GoDeep(string parseFolder)
        {
            nowParsing.Add(parseFolder);
            levelVariables.Add(new List<int>());
            DrawIerch();
        }


        public static void GoBack()
        {
            string func = nowParsing[nowParsing.Count - 1];
            if (func.IndexOf("FUNCTION") == 0)
                if (func.IndexOf("$C") > 0 && func.IndexOf("$Cvoid") < 0 && func.IndexOf("R#") < 0)
                    throw new Exception("No return in non-void function \"" + func.Substring(9, func.IndexOf("$", 9) - 9) + "\"!");

            nowParsing.RemoveAt(nowParsing.Count - 1);
            levelVariables.RemoveAt(levelVariables.Count - 1);
            DrawIerch();
        }
        public static void pushVariable(int variableNumber)
        {
            if (levelVariables.Count == 0)
                levelVariables.Add(new List<int>());
            levelVariables[levelVariables.Count - 1].Add(variableNumber);
            DrawIerch();

            return;
        }
        public static bool isVariableAvailable(int variableNumber)
        {
            for (int i = 0; i < levelVariables.Count; i++)
                for (int j = 0; j < levelVariables[i].Count; j++)
                    if (levelVariables[i][j] == variableNumber)
                        return true;
            return false;
        }

        public static bool isLast(string ss)
        {
            return (isNowIn(ss) > 0/*== nowParsing.Count - 1*/);
        }
        public static string lastFunction()
        {
            for (int i = nowParsing.Count - 1; i >= 0; i--)
                if (nowParsing[i].IndexOf("FUNCTION") == 0)
                    return nowParsing[i];
            return "NONE";
        }
        public static void addReturnToLastFunction()
        {
            for (int i = nowParsing.Count - 1; i >= 0; i--)
                if (nowParsing[i].IndexOf("FUNCTION") == 0)
                {
                    if (nowParsing[i].IndexOf("R#") == -1)
                        nowParsing[i] += "R#";
                    return;
                }
        }
        public static int isNowIn(string ss)
        {
            for (int i = nowParsing.Count - 1; i >= 0; i--)
                if (nowParsing[i].IndexOf(ss) >= 0)
                    return i;
            return -1;
        }
        static void DrawIerch()
        {
            return;
            Console.Clear();

            for (int i = 0; i < nowParsing.Count; i++)
            {
                Console.Write("\n/" + nowParsing[i] + " :: ");
                for (int j = 0; j < levelVariables[i].Count; j++)
                    Console.Write(" " + levelVariables[i][j]);
            }
            Console.ReadKey();//Thread.Sleep(1500);
        }

        public static bool CompareFunctionSignature(ASTFunction f1, ASTFunction f2)
        {
            List<ValueType> lvt1 = f1.returnTypesList();
            List<ValueType> lvt2 = f2.returnTypesList();
            string nam1 = f1.getName, nam2 = f2.getName;
            if (nam1 == nam2 && lvt1.Count == lvt2.Count)
            {
                for (int i = 0; i < lvt1.Count; i++)
                    if (lvt1[i] != lvt2[i])
                        return false;
            }
            else
                return false;
            return true;
        }

        // correct tab problems
        static List<bool> isSlide = new List<bool>();
        static string nowOpen = "│ ";
        static int lastDepth = 0;
        public static bool finish = false;
        static bool rmColomn = false;

        public static string tabs(int depth)
        {
            bool lastChild = finish;
            if (depth != lastDepth)
            {
                if (depth > lastDepth)
                {
                    isSlide.Add(!rmColomn);
                    string nowOpen2 = "";
                    for (int i = 0; i < nowOpen.Length / 2; i++)
                        nowOpen2 += ((isSlide[i]) ? "│ " : "  ");

                    nowOpen = nowOpen2 + ((lastChild) ? "└ " : "├ ");
                }
                if (depth < lastDepth)
                    for (int i = 0; i < lastDepth - depth; i++)
                    { nowOpen = nowOpen.Remove(nowOpen.Length - 4) + ((lastChild) ? "└ " : "├ "); isSlide.RemoveAt(isSlide.Count - 1); }

                lastDepth = depth;
            }
            else
            {
                if (lastChild) nowOpen = nowOpen.Remove(nowOpen.Length - 2) + "└ ";
            }

            rmColomn = (nowOpen[nowOpen.Length - 2] == '└');
            finish = false;
            return nowOpen;
        }
        public static void separate(string S, string separator, ref string leftpart, ref string rightpart, int separatorIndex)
        {
            leftpart = ""; rightpart = "";
            int pos = separatorIndex;
            if (pos == -1)
            { leftpart = S; return; }

            for (int i = 0; i < S.Length; i++)
            {
                if (i < pos)
                    leftpart += S[i];
                if (i >= pos + separator.Length)
                    rightpart += S[i];
            }

            return;
        }
        public static string breakBrackets(string s)
        {
            if (s[0] == '(' && s[s.Length - 1] == ')')
                return s.Substring(1, s.Length - 2);
            return s;
        }

        public static List<string> splitBy(string S, params char[] seps)
        {
            List<string> res = new List<string>();
            res.Add("");
            int founded = 0, level = 0;
            for (int i = -1; i < S.Length;
                i++, level += (i < S.Length) ? ((S[i] == '(' || S[i] == '{' || S[i] == '[') ? 1 : (S[i] == ')' || S[i] == '}' || S[i] == ']') ? -1 : 0) : 0)
                if (i >= 0)
                {
                    for (int j = 0; j < seps.Length; j++)   // count separate
                        if (S[i] == seps[j] && level == 0)
                        { founded++; res.Add(""); }
                        else
                            res[founded] += S[i];
                }
            List<string> res2 = new List<string>(); // delete zero long
            for (int i = 0; i < res.Count; i++)
                if (res[i].Length > 0)
                    res2.Add(res[i]);

            return res2;
        }

        public static List<string> splitByQuad(string s)
        {
            int level = 0;
            string current = "";
            List<string> res = new List<string>();

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '[') { level++; if (level == 1) continue; }
                if (s[i] == ']') { level--; if (level == 0) { res.Add(current); current = ""; continue;}  }
                if (s[i] == ',' && level == 1) { res.Add(current); current = ""; continue; }
                if (level > 0) current += s[i];

            }
            return res;
        }

        public static string StringFirstLetters(string s, int much, bool addPoints)
        {
            string res;
            try
            {
                res = s.Substring(0, much);
            }
            catch
            {
                res = s;
            }
            return (addPoints) ? res + "..." : res;
        }

        public static string getIn(string S, int pos)
        {

            char c = S[pos], c2 = ' ';
            if (c != '(' && c != '{' && c != '[')
                return S;
            if (c == '(') c2 = ')';
            if (c == '{') c2 = '}';
            if (c == '[') c2 = ']';

            int nowLevel = 0;
            string res = "";
            for (int i = pos; i < S.Length; i++)
            {
                if (S[i] == c2) { nowLevel--; if (nowLevel == 0) return res; }
                if (nowLevel >= 1) res += S[i];
                if (S[i] == c) nowLevel++;
            }
            return res; // get operand or commands
        }

        public static int IndexOfOnLevel0(string S, string subS, int from)
        {
            int pos = S.IndexOf(subS, from);
            if (pos < 0)
                return -1;
            if (pos == 0)
                return 0;

            int level = 0;
            for (int i = 0; i < S.Length; i++)
            {
                level += (S[i] == '(' || S[i] == '{' || S[i] == '[') ? 1 : ((S[i] == ')' || S[i] == '}' || S[i] == ']') ? -1 : 0);
                if ((level == 0 || (level == 1 && (subS == "(" || subS == "{" || subS == "["))) && S.Substring(i).IndexOf(subS) == 0)
                    return i;
            }
            return -1; //(level == 0) ? pos :
        }
    }
}
