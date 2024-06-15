using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Library
{
    interface IRibbonItem
    {

        string LinkImage { get; set; }
        string ToolTipImage { get; set; }
        string LongDescription { get; set; }
        string ToolTip { get; set; }
    }
}
