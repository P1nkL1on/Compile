using System;
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
            // 
            LLVM.AddToCode(";While\n");
            LLVM.AddToCode(String.Format("{0}br label %Whilecond{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber));
            LLVM.AddToCode(String.Format("{0}Whilecond{1}:\n", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber));
            string condLine = condition.getTrueEqual().ToLLVM(depth);
            LLVM.AddToCode(String.Format("{0}%cond{1} = icmp {2}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber, condLine));
            LLVM.AddToCode(String.Format("{0}br i1 %cond{1}, label %Whileaction{1}, label %Whilecont{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber));

            LLVM.AddToCode(String.Format("{0}Whileaction{1}:\n", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber));
            LLVM.AddToCode(actions.ToLLVM(depth));
            LLVM.AddToCode(String.Format("{0}br label %Whilecond{1}\n", MISC.tabsLLVM(depth), GlobalOperatorNumber));
            LLVM.AddToCode(String.Format("{0}Whilecont{1}:", MISC.tabsLLVM(depth - 1), GlobalOperatorNumber));
            return "";
        }
    }
}
