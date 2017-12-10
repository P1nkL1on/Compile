using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    class UniqOperation : IOperation
    {
        protected string operationString = "???";
        protected ValueType returnType;
        public virtual void Trace(int depth)
        {
            Console.WriteLine(MISC.tabs(depth) + operationString);
        }
        public static IOperation ParseFrom(string s)
        {
            if (s.IndexOf ("break") == 0)
                return (new Brk());
            if (s.IndexOf("continue") == 0)
                return (new Cnt());
            if (s.IndexOf("return") == 0)
                return (new Ret(BinaryOperation.ParseFrom(s.Substring(6))));
            throw new Exception("Can not parse uniq keyword:\t " + MISC.StringFirstLetters(s, 20, true));
        }

        public ValueType returnTypes()
        {
            return returnType;
        }
    }
    class Ret : UniqOperation
    {
        protected IOperation a;
        public Ret(IOperation returnable)
        {
            if (MISC.isNowIn("FUNCTION") == -1)
                throw new Exception("Return can be used only inside functions!");
            a = returnable;
            returnType = a.returnTypes();
            operationString = "RETURN  [" + returnTypes()+ "]";
            // checking a base type
            string lf = MISC.lastFunction();
            if (lf.IndexOf("$") < 0)
                throw new Exception("Incorrect external function");
            string funcType = lf.Substring(lf.LastIndexOf('$') + 1),
                   retType = returnType.ToString();
            if (funcType.IndexOf("R#") >= 0)
                funcType = funcType.Remove(funcType.IndexOf("R#"));

            if ( funcType != retType)
                throw new Exception("Incorrect return type! Required <" + funcType.ToString().Substring(1)+">, but recieved <" + retType.ToString().Substring(1)+">");
            // if everything is good than add a ret to last function
            MISC.addReturnToLastFunction();
        }
        public override void Trace(int depth)
        {
            
            Console.WriteLine(MISC.tabs(depth) + operationString);
            MISC.finish = true;
            a.Trace(depth + 1);
        }
    }
    class Cnt : UniqOperation
    {
        public Cnt()
        {
            if (!(MISC.isLast("FOR")))
                throw new Exception("Continue can be used only in FOR!");
            operationString = "CONTINUE";
            returnType = new ValueType(VT.Cvoid);
        }
    }
    class Brk : UniqOperation
    {
        public Brk()
        {
            if (!MISC.isLast("WHILE") && !(MISC.isLast("FOR")))
                throw new Exception("Break can be used only in cycles!");
            operationString = "BREAK";
            returnType = new ValueType(VT.Cvoid);
        }
    }
}
