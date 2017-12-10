using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public interface IAstNode
    {
        void Trace(int depth);
    }
}
