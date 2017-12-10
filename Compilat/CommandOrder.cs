using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public class CommandOrder : ICommand
    {
        // there can be any ASI doing one by one
        protected List<ICommand> commands;



        public CommandOrder(params ICommand[] cmnds)
        {
            commands = cmnds.ToList<ICommand>();
            CheckEmptyCommandOrders();
        }
        public CommandOrder()
        {
            commands = new List<ICommand>();
        }
        public CommandOrder(String S, char sep)
        {
            commands = new List<ICommand>();

            List<string> commandArr = (sep == ';') ? commandSplitter(S) : MISC.splitBy(S, sep);

            for (int i = 0; i < commandArr.Count; i++)
            {
                try
                {
                    commands.Add(ParseCommand2(commandArr[i]));
                } catch (Exception e)
                {
                    if (e.Message.IndexOf("#MDS:") == 0)
                    {
                        string parseAdDefines = e.Message.Substring(5);
                        List<string> addedDefines = commandSplitter(parseAdDefines);
                        for (int j = 0; j < addedDefines.Count; j++) commands.Add(ParseCommand2(addedDefines[j]));  // inta;intb;intc;  -> 3 commands
                    } else throw e; // any another bug
                        
                }
            }

            CheckEmptyCommandOrders();
        }

        void CheckEmptyCommandOrders()
        {
            for (int i = 0; i < commands.Count; i++)
                if ((commands[i] as CommandOrder) != null && ((commands[i] as CommandOrder).CommandCount == 0))
                { commands.RemoveAt(i); i--; }
        }

        List<string> commandSplitter(string S)
        {
            List<string> res = new List<string>();
            List<char> brs = new List<char>();
            string current = "";
            int doC = 0;

            for (int i = 0; i < S.Length; i++)
            {
                if ((brs.Count == 0 || brs.Last() != '\"') && (S[i] == '{' || S[i] == '(' || S[i] == '\"'))
                    brs.Add(S[i]);
                else
                    if (brs.Count > 0 && ((brs.Last() == '\"' && S[i] == '\"') || (brs.Last() == '{' && S[i] == '}')
                        || (brs.Last() == '(' && S[i] == ')')))
                        brs.RemoveAt(brs.Count - 1);

                if (brs.Count == 0 && (S[i] == ';' || (S[i] == '}' && !(doC == 3 && S.Substring(i + 1).IndexOf("while") == 0))))
                {
                    if (S[i] == '}')
                        current += S[i];
                    if (!(i < S.Length - 6 && S.Substring(i + 1, 4) == "else"))
                    {
                        if (current.Length > 0)
                            res.Add(current);
                        current = "";
                    }
                }
                else
                {
                    current += S[i];
                    if (doC < 3)
                    {
                        if (S[i] == ("do{")[doC])
                            doC++;
                        else
                            doC = 0;
                    }
                }
            }
            if (current.Length > 0)
                res.Add(current);
            return res;
        }

        public ICommand ParseCommand2(String S)
        {
            int s1 = MISC.IndexOfOnLevel0(S, "(", 0),
                s2 = MISC.IndexOfOnLevel0(S, ")", 0),
                p1 = MISC.IndexOfOnLevel0(S, "{", 0),
                p2 = MISC.IndexOfOnLevel0(S, "}", 0);

            if (s2 < s1 || p2 < p1)
                throw new Exception("Command contains incorrect brackets:\t" + MISC.StringFirstLetters(S, 20, true));

            if (p1 == 0 && p2 == S.Length - 1)
            {
                MISC.GoDeep("Block");
                CommandOrder co = new CommandOrder(MISC.getIn(S, 0), ';');
                MISC.GoBack();
                return co;
            }

            // Binary operation usuall
            if ((p1 < 0 || (MISC.IndexOfOnLevel0(S, "=", 0) > 0))
                && S.ToLower().IndexOf("if") != 0 && S.ToLower().IndexOf("for") != 0)
            {
                IOperation newBO = BinaryOperation.ParseFrom(S);
                if ((newBO as Assum) != null && (newBO as Assum).requiredUpdate != "none")
                {
                    string needUpdate = (newBO as Assum).requiredUpdate;
                    if (needUpdate.IndexOf("structdefineinfor") == 0)
                    {
                        string nam = (newBO as Assum).GetAssumableName;
                        if (nam == "-")
                            throw new Exception("What are you doing!?");
                        List<IOperation> values = (newBO as Assum).GetStructDefine();
                        List<ICommand> res = new List<ICommand>();
                        for (int i = 0; i < values.Count; i++)
                            res.Add(new Assum(BinaryOperation.ParseFrom(nam + "[" + i + "]"), values[i]));

                        return new CommandOrder(res.ToArray());
                    }
                }
                else
                    return newBO;
            }
            // _______________________
            if (s1 > 0)
            {
                string operatorFind = S.Remove(s1);
                // simple if
                if (operatorFind == "if")
                {
                    // we gonna parse IF from this shit!
                    string conditionParse = MISC.getIn(S, 2),
                           firstActionParse = S.Substring(s2 + 1);

                    int indElse = MISC.IndexOfOnLevel0(firstActionParse, "else", 0);
                    if (indElse > 0)
                    {
                        string secondActionParse = firstActionParse.Substring(indElse + 4);
                        firstActionParse = firstActionParse.Substring(0, indElse);

                        return new OperatorIf(conditionParse, firstActionParse, secondActionParse);
                    }
                    return new OperatorIf(conditionParse, firstActionParse, "");
                }
                // simple while
                if (operatorFind == "while")
                {

                    string conditionParse = MISC.getIn(S, 5),
                           iterationParse = S.Substring(s2 + 1);
                    return new CycleWhile(conditionParse, iterationParse, false);
                }
                // reverse while
                if (p1 == 2 && S.Remove(p1) == "do")
                {
                    int whilePos = MISC.IndexOfOnLevel0(S, "while", 0);

                    if (whilePos < -1)
                        throw new Exception("No while, but used \"do\"\t " + MISC.StringFirstLetters(S, 20, true));

                    string iterationParse = MISC.getIn(S, 2),
                           conditionParse = MISC.getIn(S, s1);

                    return new CycleWhile(conditionParse, iterationParse, true);
                }
                // FOR mazafaka
                if (operatorFind == "for")
                {
                    string partsParse = MISC.getIn(S, 3),
                           allOther = S.Substring(s2 + 1);
                    string[] spp = partsParse.Split(';');
                    if (spp.Length != 3)
                        throw new Exception("Invalid count of FOR-cycle condition parts\t " + MISC.StringFirstLetters(S, 20, true));

                    MISC.GoDeep("FOR");
                    this.MergeWith(new CommandOrder(spp[0], ','));
                    if (spp[1] == "") spp[1] = "true";  // condition
                    CommandOrder actions = new CommandOrder(allOther, ';'); actions.MergeWith(new CommandOrder(spp[2], ','));
                    

                    CycleFor cf = new CycleFor(spp[1], actions);
                    MISC.GoBack();

                    return cf;
                }
                throw new Exception("Can not parse a command\t " + MISC.StringFirstLetters(S, 20, true));
            }

            return new CommandOrder();
        }

        //public ICommand[] ParseCommand(String S)
        //{
        //    // here we get a 1 string between ;...;
        //    // it can be cycle or simple operation
        //    int p1 = MISC.IndexOfOnLevel0(S, "{", 0),
        //        p2 = MISC.IndexOfOnLevel0(S, "}", 0);

        //    #region 0Zone and array assume
        //    if (p1 == 0 && p2 == S.Length - 1)
        //    {
        //        return new ICommand[] { new OperatorZone(MISC.getIn(S, S.IndexOf('{'))) };
        //    }
        //    if ((S.IndexOf("{") < 0 || (MISC.IndexOfOnLevel0(S, "=", 0) > 0))
        //        && S.ToLower().IndexOf("if") != 0/* && S.ToLower().IndexOf("else") != 0*/)
        //    {
        //        IOperation newBO = BinaryOperation.ParseFrom(S);
        //        if ((newBO as Assum) != null && (newBO as Assum).requiredUpdate != "none")
        //        {
        //            string needUpdate = (newBO as Assum).requiredUpdate;
        //            if (needUpdate.IndexOf("structdefineinfor") == 0)
        //            {
        //                string nam = (newBO as Assum).GetAssumableName;
        //                if (nam == "-")
        //                    throw new Exception("What are you doing!?");
        //                List<IOperation> values = (newBO as Assum).GetStructDefine();
        //                List<ICommand> res = new List<ICommand>();
        //                for (int i = 0; i < values.Count; i++)
        //                    res.Add(new Assum(BinaryOperation.ParseFrom(nam + "[" + i + "]"), values[i]));

        //                return res.ToArray();
        //            }
        //        }
        //        else
        //            return new ICommand[] { newBO };
        //    }
        //    #endregion
        //    #region Cycles
        //    //try to parse while cycles
        //    if (S.ToLower().IndexOf("for") == 0)
        //    {
        //        string parseCondition = MISC.getIn(S, S.IndexOf('(')),
        //               parseAction = MISC.getIn(S, S.IndexOf('{'));

        //        string[] conditionParts = parseCondition.Split(';');
        //        if (conditionParts.Length != 3)
        //            throw new Exception("Invalid count of FOR-cycle condition parts");
        //        // first one - is simple commands of initialization

        //        MISC.GoDeep("FOR");
        //        if (conditionParts[0].Length > 0)
        //            this.MergeWith(new CommandOrder(conditionParts[0], ','));    // included
        //        if (conditionParts[1].Length <= 0)
        //            conditionParts[1] = "true";
        //        // parse commands

        //        CommandOrder actions = new CommandOrder(parseAction, ';');
        //        if (conditionParts[2].Length > 0)
        //            actions.MergeWith(new CommandOrder(conditionParts[2], ','));

        //        ICommand[] res = new ICommand[] { new CycleFor(conditionParts[1], actions) };
        //        MISC.GoBack();
        //        return res;
        //    }
        //    if (S.ToLower().IndexOf("while") == 0)
        //        return new ICommand[] { new CycleWhile(MISC.getIn(S, S.IndexOf('(')), MISC.getIn(S, S.IndexOf('{')), false) };
        //    if (S.ToLower().IndexOf("do") == 0)
        //        return new ICommand[] { new CycleWhile(MISC.getIn(S, S.IndexOf('(')), MISC.getIn(S, S.IndexOf('{')), true) };
        //    #endregion
        //    #region Operators
        //    if (S.ToLower().IndexOf("if") == 0)
        //    {
        //        int indexOfConditionRightBrakket = MISC.IndexOfOnLevel0(S, ")", 0);
        //        if (S.IndexOf("{") - 1 == indexOfConditionRightBrakket)
        //        {
        //            int pos1 = MISC.IndexOfOnLevel0(S, "}", 0),
        //                pos2 = MISC.IndexOfOnLevel0(S, "}", pos1 + 1),
        //                posElse = MISC.IndexOfOnLevel0(S, "}else{", 0);
        //            if (pos2 < pos1)
        //                return new ICommand[] { new OperatorIf(MISC.getIn(S, S.IndexOf('(')), MISC.getIn(S, S.IndexOf('{')), "") };
        //            else
        //                return new ICommand[] { new OperatorIf(MISC.getIn(S, S.IndexOf('(')), MISC.getIn(S, S.IndexOf('{')), MISC.getIn(S, S.LastIndexOf("{"))) };
        //        }
        //        else
        //        {
        //            int indexElse = MISC.IndexOfOnLevel0(S, "else", 0);
        //            if (indexElse < 0)
        //                return new ICommand[] { new OperatorIf(MISC.getIn(S, S.IndexOf('(')), S.Substring(indexOfConditionRightBrakket + 1), "") };
        //            else
        //                return new ICommand[]{new OperatorIf(MISC.getIn(S, S.IndexOf('(')),
        //                    S.Substring(indexOfConditionRightBrakket + 1, indexElse - indexOfConditionRightBrakket - 1),
        //                    S.Substring(indexElse + 4))};
        //        }
        //    }
        //    #endregion
        //    throw new Exception("Can not parse a command\t " +MISC.StringFirstLetters(S, 20, true));
        //    return null;
        //}

        public void MergeWith(CommandOrder another)
        {
            for (int i = 0; i < another.commands.Count; i++)
                commands.Add(another.commands[i]);
            // merge with            
        }

        public void Trace(int depth)
        {
            Console.WriteLine(String.Format("{0}" + ((commands.Count > 0) ? "C" : "Empty c") + "ommand order <{1}> :", MISC.tabs(depth), commands.Count));
            for (int i = 0; i < commands.Count; i++)
            {
                //if (commands.Count > 1)
                //    Console.WriteLine(String.Format("{0}#{1}", MISC.tabs(depth+1), i + 1));
                if (i == commands.Count - 1)
                    MISC.finish = true;
                commands[i].Trace(depth + 1);
            }
        }

        public int CommandCount
        {
            get { return commands.Count(); }
        }
    }
}
