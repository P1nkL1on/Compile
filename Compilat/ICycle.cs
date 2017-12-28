﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public abstract class ICycle : ICommand
    {
        protected int GlobalOperatorNumber;
        public Equal condition;   // if (...){}
        protected CommandOrder actions;  // if (){...}

        public virtual void Trace(int depth)
        {
            Console.WriteLine(MISC.tabs(depth) + "Default cycle trace");
        }
        public virtual string ToLLVM(int depth)
        {
            return String.Format("{0} ...", MISC.tabsLLVM(depth));
        }
    }

    public class CycleFor : ICycle
    {
        public CycleFor(string parseCondition, CommandOrder actions)
        {
            condition = new Equal(BinaryOperation.ParseFrom(parseCondition), new ASTvalue(new ValueType(VT.Cboolean), (object)true));
            this.actions = actions;
            GlobalOperatorNumber = ++MISC.GlobalOperatorNumber;
        }
        public override void Trace(int depth)
        {
            Console.WriteLine(String.Format("{0}FOR", MISC.tabs(depth)));

            //Console.WriteLine(String.Format("{0}WHILE", MISC.tabs(depth)));
            condition.Trace(depth + 1);

            MISC.finish = true; actions.Trace(depth + 1);
        }
        public override string ToLLVM(int depth)
        {
            return CycleWhile.ToLLVMVariative(depth, GlobalOperatorNumber, "For", false, condition, actions);
        }
    }
    public class CycleWhile : ICycle
    {
        bool doFirst;

        public CycleWhile(string parseCondition, CommandOrder actions, bool doFirst)
        {
            condition = new Equal(BinaryOperation.ParseFrom(parseCondition), new ASTvalue(new ValueType(VT.Cboolean), (object)true));
            this.actions = actions;
            this.doFirst = doFirst;
            GlobalOperatorNumber = ++MISC.GlobalOperatorNumber;
        }

        public CycleWhile(string parseCondition, string parseActions, bool doFirst)
        {
            condition = new Equal(BinaryOperation.ParseFrom(parseCondition), new ASTvalue(new ValueType(VT.Cboolean), (object)true));
            MISC.GoDeep("WHILE");
            actions = new CommandOrder(parseActions, ';');
            MISC.GoBack();
            this.doFirst = doFirst;
            GlobalOperatorNumber = ++MISC.GlobalOperatorNumber;
        }

        public override void Trace(int depth)
        {
            Console.WriteLine(String.Format("{0}WHILE", MISC.tabs(depth)));
            if (!doFirst)
            {
                condition.Trace(depth + 1);
                MISC.finish = true;
                actions.Trace(depth + 1);
            }
            else
            {
                actions.Trace(depth + 1);
                MISC.finish = true;
                condition.Trace(depth + 1);
            }
        }
        public override string ToLLVM(int depth)
        {
            
            return ToLLVMVariative(depth, GlobalOperatorNumber, "While", doFirst, condition, actions);
        }
        public static string ToLLVMVariative(int depth, int GlobalOperatorNumber, string type,  bool doBeforeWhile, Equal condition, CommandOrder actions )
        {
            LLVM.AddToCode(";" + type + "\n");
            // 
            if (!doBeforeWhile)
            {
                LLVM.AddToCode(String.Format("{0}br label %{2}cond{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, type));
                LLVM.AddToCode(String.Format("{0}{2}cond{1}:\n", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber, type));
                string condLine = condition.getTrueEqual().ToLLVM(depth);
                LLVM.AddToCode(String.Format("{0}%cond{1} = icmp {2}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, condLine));
                LLVM.AddToCode(String.Format("{0}br i1 %cond{1}, label %{2}action{1}, label %{2}cont{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, type));

                LLVM.AddToCode(String.Format("{0}{2}action{1}:\n", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber, type));
                LLVM.AddToCode(actions.ToLLVM(depth));
                LLVM.AddToCode(String.Format("{0}br label %{2}cond{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, type));
                LLVM.AddToCode(String.Format("{0}{2}cont{1}:", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber, type));
            }
            else
            {
                LLVM.AddToCode(String.Format("{0}{2}action{1}:\n", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber, type));
                LLVM.AddToCode(actions.ToLLVM(depth));

                LLVM.AddToCode(String.Format("{0}br label %{2}cond{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, type));
                LLVM.AddToCode(String.Format("{0}{2}cond{1}:\n", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber, type));
                string condLine = condition.getTrueEqual().ToLLVM(depth);
                LLVM.AddToCode(String.Format("{0}%cond{1} = icmp {2}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, condLine));
                LLVM.AddToCode(String.Format("{0}br i1 %cond{1}, label %{2}action{1}, label %{2}cont{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, type));

                LLVM.AddToCode(String.Format("{0}br label %{2}cond{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, type));
                LLVM.AddToCode(String.Format("{0}{2}cont{1}:", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber, type));
            }
            return "";
        }
    }
}
