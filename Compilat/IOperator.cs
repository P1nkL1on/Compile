﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{

    public abstract class IOperator : ICommand
    {
        protected int GlobalOperatorNumber;
        public Equal condition;   // if (...){}
        protected List<CommandOrder> actions;  // {....} {....} {....} {....} {....}
        // for IF there are only 2, for SWITCH is infinite + DEFAULT

        public virtual void Trace(int depth)
        {
            Console.WriteLine(MISC.tabs(depth) + "Default operator trace");
        }
        public virtual string ToLLVM(int depth)
        {
            return String.Format("{0} ...", MISC.tabsLLVM(depth));
        }
    }

    public class OperatorZone : IOperator
    {
        public OperatorZone(string parseActions)
        {
            condition = new Equal(new ASTvalue(new ValueType(VT.Cboolean), (object)true), new ASTvalue(new ValueType(VT.Cboolean), (object)true));
            actions = new List<CommandOrder>();
            MISC.GoDeep("OZONE");
            actions.Add(new CommandOrder(parseActions, ';'));
            MISC.GoBack();
        }
        public override void Trace(int depth)
        {
            Console.WriteLine(String.Format("{0}BLOCK", MISC.tabs(depth)));
            for (int i = 0; i < actions.Count; i++)
            {
                if (i == actions.Count - 1)
                    MISC.finish = true;
                actions[i].Trace(depth + 1);
            }
        }
    }

    public class OperatorIf : IOperator
    {
        public OperatorIf(string parseCondition, string parseActions, string parseElseAction)
        {
            condition = new Equal(BinaryOperation.ParseFrom(parseCondition), new ASTvalue(new ValueType(VT.Cboolean), (object)true));
            actions = new List<CommandOrder>();
            actions.Add(new CommandOrder());
            actions.Add(new CommandOrder());

            MISC.GoDeep("IFTHEN");
            actions[0].MergeWith(new CommandOrder(parseActions, ';'));
            MISC.GoBack();
            if (parseElseAction.Length > 0)
            {
                MISC.GoDeep("IFELSE");
                actions[1].MergeWith(new CommandOrder(parseElseAction, ';'));
                MISC.GoBack();
            }
            GlobalOperatorNumber = ++MISC.GlobalOperatorNumber;
        }

        public override void Trace(int depth)
        {
            Console.WriteLine(String.Format("{0}IF", MISC.tabs(depth)));
            condition.Trace(depth + 1);
            if (actions[1].CommandCount == 0)
                MISC.finish = true;
            Console.WriteLine(String.Format("{0}THEN", MISC.tabs(depth + 1)));
            MISC.finish = true;
            actions[0].Trace(depth + 2);
            if (actions[1].CommandCount > 0)
            {
                MISC.finish = true;
                Console.WriteLine(String.Format("{0}ELSE", MISC.tabs(depth + 1)));
                MISC.finish = true;
                actions[1].Trace(depth + 2);
            }
        }

        public override string ToLLVM(int depth)
        {
            //
            LLVM.AddToCode(String.Format("{0}If{1}:\n", MISC.tabsLLVM(depth), GlobalOperatorNumber));
            string condLine = condition.getTrueEqual().ToLLVM(depth + 1);
            LLVM.AddToCode(String.Format("{0}%cond{1} = icmp {2}\n", MISC.tabsLLVM(depth + 1), GlobalOperatorNumber, condLine));
            LLVM.AddToCode(String.Format("{0}br i1 %cond{1}, label %Then{1}, label %Else{1}\n", MISC.tabsLLVM(depth + 1), GlobalOperatorNumber));
            LLVM.AddToCode(String.Format("{0}Then{1}:\n", MISC.tabsLLVM(depth), GlobalOperatorNumber));
            LLVM.AddToCode(actions[0].ToLLVM(depth + 1));
            LLVM.AddToCode(String.Format("{0}Else{1}:\n", MISC.tabsLLVM(depth), GlobalOperatorNumber));
            LLVM.AddToCode(actions[1].ToLLVM(depth + 1));
            return "";

        }
    }

}
