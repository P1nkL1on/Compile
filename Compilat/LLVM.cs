using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public static class LLVM
    {
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
            string kids = String.Format("{0}, {1}\n", a.ToLLVM(depth), b.ToLLVM(depth));
            string add = String.Format("{3}%tmp{0} = {4} {1} {2}", MISC.LLVMtmpNumber, returnType.ToLLVM(), kids, MISC.tabsLLVM(depth), keyword);
            LLVM.CurrentCode += add;
            return "%tmp" + (MISC.LLVMtmpNumber++);
        }

        public static string BinaryEqualToLLVM(int depth, string keyword, IOperation a, IOperation b, ValueType returnType)
        {
            string kids = String.Format("{0}, {1}\n", a.ToLLVM(depth), b.ToLLVM(depth));
            return String.Format("{3} {1} {2}", MISC.LLVMtmpNumber, a.returnTypes().ToLLVM(), kids, keyword);
        }

        public static void AddToCode(string what)
        {
            string A = what;
            currentCode += A;
        }
    }
}
