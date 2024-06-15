using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Library
{
    interface IPushButtonData
    {
        string Name { get; }
        string text { get; }   
        string AssemblyName { get;}
        string className { get; }  
    }
}
