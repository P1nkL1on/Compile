using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public static class LLVM
    {
        public static int globalVars = 0;
        static string currentCode = "";
        public static string CurrentCode
        {
            get { return currentCode; }
            set
            {
                currentCode = value;
                //Console.WriteLine("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _\n"); MISC.ConsoleWriteLine(currentCode, ConsoleColor.DarkGreen);
            }
        }

        public static string BinaryToLLVM(int depth, string keyword, IOperation a, IOperation b, ValueType returnType)
        {
            
            //int curNumber = MISC.LLVMtmpNumber + 1;
            MISC.LLVMtmpNumber++;
            int num = MISC.LLVMtmpNumber;
            string kids = String.Format("{0}, {1}\n", a.ToLLVM(depth), b.ToLLVM(depth)); MISC.LLVMtmpNumber++;
            string add = String.Format("{3}%tmp{0} = {4} {1} {2}", num, returnType.ToLLVM(), kids, MISC.tabsLLVM(depth), keyword);
            LLVM.CurrentCode += add;
            return "%tmp" + (num);
        }


        public static string BinaryEqualToLLVM(int depth, string keyword, IOperation a, IOperation b, ValueType returnType)
        {
            if (keyword != "and" && keyword != "or")
                keyword = "icmp " + keyword;
            MISC.LLVMtmpNumber++;
            int num = MISC.LLVMtmpNumber;
            string kids = String.Format("{0}, {1}\n", a.ToLLVM(depth), b.ToLLVM(depth));
            LLVM.AddToCode( String.Format("{4}%tmp{0} = {3} {1} {2}", num, a.returnTypes().ToLLVM(), kids, keyword, MISC.tabsLLVM(depth)));
            return "%tmp" + num;
        }
        public static string TypeConvertLLVM(int depth, string keyword, IOperation what, ValueType ty2)
        {
            MISC.LLVMtmpNumber++;
            int num = MISC.LLVMtmpNumber;
            string boy = what.returnTypes().ToLLVM() + " " + what.ToLLVM(depth);
            LLVM.AddToCode(String.Format("{0}%tmp{1} = {2} {3} to {4}\n", MISC.tabsLLVM(depth), num, keyword, boy, ty2.ToLLVM() ));
            return "%tmp" + num;
        }

        public static string ParamToLLVM(int depth, string keyword, ValueType returnType, params IOperation[] ops)
        {
            MISC.LLVMtmpNumber++;
            int num = MISC.LLVMtmpNumber;
            string kids = "";//first.ToLLVM(depth)+((ops.Length > 0)?", " : "");// = String.Format("{0}, {1}\n", a.ToLLVM(depth), b.ToLLVM(depth));
            for (int i = 0; i < ops.Length; i++, kids += ((i < ops.Length ) ? ", " : ""))
                kids += ops[i].returnTypes().ToLLVM() + " " + ops[i].ToLLVM(depth);

            return String.Format("{2}{0}{3}{5} {4}\n", (keyword.Length > 0)? " " :"", num, keyword, returnType.ToLLVM(), kids,(keyword.Length > 0)? "," :"" );

        }

        public static void AddToCode(string what)
        {
            string A = what;
            currentCode += A;
        }
        public static string Load(ValueType vtLoad, string from)
        {
            return String.Format("load {0}, {1}, align {2}", vtLoad.TypeOfPointerToThis().ToLLVM(), from, MISC.SyzeOf(vtLoad.TypeOfPointerToThis()));
        }
        public static string CommandOrderQueueCode = "";
        public static List<ASTvariable> varisReload = new List<ASTvariable>();
    }
}
