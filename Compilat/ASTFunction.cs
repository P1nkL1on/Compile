﻿using System;
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
        public bool declareOnly;
        //
        public int infiniteParamsAfter;
        public int LLVMnumber = -1;
        //
        public int ParamCount { get { return input.Count; } }
        static Define GetDefineFromString(string s)
        {
            int varType = Math.Max((s.IndexOf("int") == 0) ? 2 : -1, Math.Max((s.IndexOf("double") == 0) ? 5 : -1, Math.Max((s.IndexOf("char") == 0) ? 3 : -1,
                Math.Max((s.IndexOf("string") == 0) ? 5 : -1, (s.IndexOf("bool") == 0) ? 3 : -1))));

            if (varType >= 0)
            {
                s = s.Insert(varType + 1, "$");
                return new Define(s, true);
            }
            throw new Exception("Incorrect signature!");
        }
        public ASTFunction(string S)
        {
            infiniteParamsAfter = -1;
            declareOnly = false;
            //TypeConvertion tpcv = new TypeConvertion("IIBDDBDIBIDBCCB", 2);
            string s = S.Substring(0, S.IndexOf('('));
            List<ValueType> vtList = new List<ValueType>();

            int varType = Math.Max((s.IndexOf("int") == 0) ? 2 : -1, Math.Max((s.IndexOf("double") == 0) ? 5 : -1, Math.Max((s.IndexOf("char") == 0) ? 3 : -1,
                Math.Max((s.IndexOf("string") == 0) ? 5 : -1, Math.Max((s.IndexOf("bool") == 0) ? 3 : -1, (s.IndexOf("void") == 0) ? 3 : -1)))));
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
                retType = new ValueType(Define.detectType(type_name[0]), returnPointerLevel);
                // try to parse signature and actions
                List<string> vars = MISC.splitBy(MISC.getIn(S, S.IndexOf('(')), ',');
                input = new List<Define>();
                MISC.GoDeep("FDEFINED");
                MISC.ChangeAdressType(VAT.Parameter);

                for (int i = 0; i < vars.Count; i++)
                {
                    if (vars[i] != "...")
                    {
                        if (infiniteParamsAfter >= 0)
                            throw new Exception("Can not defined more arguments after ... !");
                        input.Add(GetDefineFromString(vars[i]));
                        vtList.Add((input[input.Count - 1] as Define).returnTypes());
                    }
                    else
                        infiniteParamsAfter = i;
                }
                //tpcvString += returnTypes().ToString()[1].ToString().ToUpper();
                tpcv = new TypeConvertion(vtList, retType);



                // check name uniq!
                //bool foundFunc = false;
                for (int i = 0; i < ASTTree.funcs.Count; i++)
                    if (ASTTree.funcs[i].actions.CommandCount > 0 && MISC.CompareFunctionSignature(ASTTree.funcs[i], this))
                        throw new Exception("Can not redefine a function \"" + name + " : " + this.getArgsString + "\"!");

                if (S.IndexOf('{') >= 0)
                {
                    try
                    {
                        MISC.GoDeep("FUNCTION$" + name + "$" + returnTypes());
                        MISC.ChangeAdressType(VAT.Local);
                        string actionCode = MISC.getIn(S, S.IndexOf('{'));
                        actions = new CommandOrder(actionCode, ';');
                        if (!MISC.GoBack())
                            actions.MergeWith(new CommandOrder(new ICommand[] { new Ret() }));
                        MISC.ChangeAdressType(VAT.Global);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Problem in function \"" + name + "\"\n" + e.Message);
                    }
                }
                else
                {
                    actions = new CommandOrder();
                    declareOnly = true;
                }
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
        public virtual string ToLLVM(int depth)
        {
            string param = "";
            int funcNumber = depth; depth = 0;
            for (int i = 0; i < input.Count; i++)
                param += input[i].returnTypes().ToLLVM() + ((!declareOnly) ? " %" + input[i].varName : "") + ((i < input.Count - 1) ? ", " : "");
            if (infiniteParamsAfter >= 0)
                param += ", ...";
            LLVM.AddToCode(String.Format("; {0} {1}\n", getName, getArgsString));
            if (!declareOnly)
            {
                LLVM.AddToCode(String.Format("{0}define {1} @{2}({3}) #{4} ", MISC.tabsLLVM(depth), retType.ToLLVM(), getName, param, funcNumber) + "{\n" /*+ MISC.tabsLLVM(depth) + "entry:\n"*/);//  + code + "}";
                // add used global variables
                foreach (ASTvariable vari in ASTTree.GlobalVarsVars)
                {
                    vari.reloadedTimes++;
                    LLVM.AddToCode(String.Format("{0}{1} = load {2}, {3} {4}\n", MISC.tabsLLVM(depth + 1), vari.ToLLVM(), vari.returnTypes().ToLLVM(), vari.returnTypes().TypeOfPointerToThis().ToLLVM(),"@" +  MISC.RemoveCall(vari.ToLLVM()).Substring(1)));
                }
                LLVM.AddToCode(actions.ToLLVM(depth + 1));
                return MISC.tabsLLVM(depth) + "}";
            }
            return String.Format("{0}declare {1} @{2}({3}) #{4}", MISC.tabsLLVM(depth), retType.ToLLVM(), getName, param, funcNumber);
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
        public string returnListLLVMCall()
        {
            string res = "";
            for (int i = 0; i < input.Count; i++)
                res += input[i].returnTypes().ToLLVM() + ", ";
            return (infiniteParamsAfter > 0) ? res + "..." : res.Remove(res.Length - 2);
        }
        public int CommandCount
        {
            get { return actions.CommandCount; }
        }
    }
}
