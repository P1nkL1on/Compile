using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    struct ASTFunctionCall : IOperation
    {

        int functionCallNumber;
        List<IOperation> arguments;

        public ASTFunctionCall(string s)
        {
            string ErrString = "";

            if (s.IndexOf("(") < 0)
                throw new Exception(s + " is not a function!");
            string approximateFuncName = s.Substring(0, s.IndexOf("("));
            bool foundAnalog = false; int i = 0;
            //define required types
            string incomeValuesString = MISC.getIn(s, s.IndexOf('('));

            List<ValueType> callingTypes;
            arguments = new List<IOperation>();

            if (incomeValuesString.Length > 0)
            {
                callingTypes = new List<ValueType>();
                List<string> incomeValues = MISC.splitBy(incomeValuesString, ',');
                for (int df = 0; df < incomeValues.Count; df++)
                {
                    arguments.Add(BinaryOperation.ParseFrom(incomeValues[df]));
                    callingTypes.Add(arguments[arguments.Count - 1].returnTypes());
                }
            }
            else
            {
                callingTypes = new List<ValueType>();
                //callingTypes.Add(ValueType.Cvoid);
            }

            i = 0;
            while (!foundAnalog && i < ASTTree.funcs.Count)
            {
                bool nameSame = (ASTTree.funcs[i].getName == approximateFuncName);
                int haveTypes = ASTTree.funcs[i].returnTypesList().Count,
                    callTypes = callingTypes.Count;

                if (nameSame && haveTypes == callTypes)
                {
                    IOperation[] children = new IOperation[arguments.Count];
                    for (int j = 0; j < arguments.Count; j++)
                        children[j] = arguments[j];
                    if (callingTypes.Count != 0)
                    {
                        try
                        {
                            //ValueType returnType = MISC.CheckTypeCorrect(null, ASTTree.funcs[i].tpcv, ref children);
                            ValueType returnType = TypeConverter.TryConvert( ASTTree.funcs[i].tpcv, ref children);
                            arguments = children.ToList();
                            foundAnalog = true;
                            break;
                        }
                        catch (Exception e) { ErrString += e.Message + "\n"; };
                    }
                    else
                    {
                        if (nameSame && callTypes == 0 && callTypes == haveTypes)
                        { foundAnalog = true; break; }
                    }
                }
                //else
                //{
                //    if (nameSame)
                //    {
                //        foundAnalog = true; break;
                //    }
                //}
                //// if same name then check correct of all types including
                //if (nameSame)
                //{
                //    foundAnalog = true;
                //    List<ValueType> requiredArgTypes = ASTTree.funcs[i].returnTypesList();
                //    if (requiredArgTypes.Count == callingTypes.Count)
                //    {
                //        for (int j = 0; j < callingTypes.Count; j++)
                //            if (callingTypes[j] != requiredArgTypes[j])
                //                foundAnalog = false;    // не совпадает тип соответствующих аргументов
                //    }
                //    else
                //        foundAnalog = false;    // не совпадает количество параметров
                //}
                i++;
            }
            // declare
            functionCallNumber = i;

            //make bug
            if (!foundAnalog)
            {
                if (ErrString == "")
                    throw new Exception("Function with this name/arguments was never declared!");
                else
                    throw new Exception(ErrString);
            }
        }

        public void Trace(int depth)
        {
            //Console.WriteLine(String.Format("{0}{1}  #{3}[{2}]", MISC.tabs(depth), ASTTree.funcs[functionCallNumber].getName,
            //                  ASTTree.funcs[functionCallNumber].returnTypes().ToString(), functionCallNumber));
            Console.Write(MISC.tabs(depth));
            MISC.ConsoleWrite(ASTTree.funcs[functionCallNumber].getName + " #" + functionCallNumber, ConsoleColor.Cyan);
            MISC.ConsoleWriteLine(" -> " + ASTTree.funcs[functionCallNumber].returnTypes().ToString().Substring(1), ConsoleColor.DarkGreen);

            for (int i = 0; i < arguments.Count; i++)
            {
                if (i == arguments.Count - 1)
                    MISC.finish = true;
                arguments[i].Trace(depth + 1);
            }
        }
        public ValueType returnTypes()
        {
            return ASTTree.funcs[functionCallNumber].returnTypes();
        }


    }
}
