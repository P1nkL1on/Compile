using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public class ASTFunction : IOperation
    {
        // int func (double a, double y, double z, bool XX){....with only those variables.... return int();}
        string name;
        ValueType retType;
        List<Define> input;
        CommandOrder actions;
        //
        public TypeConvertion tpcv;
        //
        public ASTFunction(string S)
        {
            //TypeConvertion tpcv = new TypeConvertion("IIBDDBDIBIDBCCB", 2);
            string s = S.Substring(0, S.IndexOf('('));
                   //tpcvString = "";
            List<ValueType> vtList = new List<ValueType>();


            int varType = Math.Max((s.IndexOf("int") >= 0) ? 2 : -1, Math.Max((s.IndexOf("double") >= 0) ? 5 : -1, Math.Max((s.IndexOf("char") >= 0) ? 3 : -1,
                Math.Max((s.IndexOf("string") >= 0) ? 5 : -1, Math.Max((s.IndexOf("bool") >= 0) ? 3 : -1, (s.IndexOf("void") >= 0) ? 3 : -1)))));
            if (varType >= 0)
            {
                varType++;
                string[] type_name = new
                    string[] { s.Substring(0, varType), s.Substring(varType, s.Length - varType) };//s.Split(s[varType + 1]);
                name = type_name[1];
                int returnPointerLevel = 0;
                while (name[0] == '*') { returnPointerLevel++; name = name.Substring(1); }

                if (name.Length == 0) throw new Exception("Invalid function name!");
                

                // !
                retType=  new ValueType(Define.detectType(type_name[0]), returnPointerLevel);
                // try to parse signature and actions
                List<string> vars = MISC.splitBy(MISC.getIn(S, S.IndexOf('(')), ',');
                input = new List<Define>();
                MISC.GoDeep("FDEFINED");

                for (int i = 0; i < vars.Count; i++)
                {
                    input.Add((Define)MonoOperation.ParseFrom(vars[i]));
                    vtList.Add((input[input.Count - 1] as Define).returnTypes());
                    //tpcvString += vars[i][0].ToString().ToUpper();
                }
                //tpcvString += returnTypes().ToString()[1].ToString().ToUpper();
                tpcv = new TypeConvertion(vtList, retType);
                


                // check name uniq!
                //bool foundFunc = false;
                for (int i = 0; i < ASTTree.funcs.Count; i++)
                    if (ASTTree.funcs[i].actions.CommandCount > 0 && MISC.CompareFunctionSignature(ASTTree.funcs[i], this))
                        throw new Exception("Can not redefine a function \""+name+" : "+this.getArgsString+"\"!");

                if (S.IndexOf('{') >= 0)
                {
                    try
                    {
                        MISC.GoDeep("FUNCTION$" + name + "$" + returnTypes());
                        string actionCode = MISC.getIn(S, S.IndexOf('{'));
                        actions = new CommandOrder(actionCode, ';');
                        MISC.GoBack();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Problem in function \""+name+"\"\n" + e.Message);
                    }
                }
                else
                    actions = new CommandOrder();

                MISC.GoBack();
                return;
            }
            // check contain of Return function
            throw new Exception("Can not parse a function\t " + MISC.StringFirstLetters(S, 20, true));
        }
        //
        public ValueType returnTypes()
        {
            return retType;
        }
        //
        public void Trace(int depth)
        {
            Console.Write(MISC.tabs(depth));
            MISC.ConsoleWriteLine(String.Format("FUNCTION \"{0}\"", this.name), ConsoleColor.Black, ConsoleColor.Cyan);
            Console.Write(MISC.tabs(depth + 1));
            MISC.ConsoleWriteLine("<<", ConsoleColor.Cyan);

            for (int i = 0; i < input.Count; i++) { if (i == input.Count - 1)MISC.finish = true; input[i].Trace(depth + 2); }

            Console.Write(MISC.tabs(depth + 1));
            MISC.ConsoleWriteLine(">>", ConsoleColor.Cyan);
            MISC.finish = true;
            Console.Write(MISC.tabs(depth + 2));
            MISC.ConsoleWriteLine(returnTypes().ToString(), ConsoleColor.Cyan);
            MISC.finish = true;
            actions.Trace(depth + 1);
        }
        //
        public string getName
        {
            get { return name; }
        }

        public string getArgsString
        {
            get
            {
                string res = tpcv.ToString();
                return res.Substring(0, res.Length - 1);
            }
        }
        public List<ValueType> returnTypesList()
        {
            List<ValueType> res = new List<ValueType>();
            for (int i = 0; i < this.input.Count; i++)
                res.Add(this.input[i].returnTypes());
            return res;
        }
        public int CommandCount
        {
            get { return actions.CommandCount; }
        }
    }
}
