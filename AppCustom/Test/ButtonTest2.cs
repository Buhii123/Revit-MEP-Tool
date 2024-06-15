using AppCustom.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Test
{
    [ButtonRvAttribute]
    public class ButtonTest2
    {
        [PushButtonDataUI("Reload Command", "Commands.MyCommand", ToolTip = "sadasdzxc")]
        public void DataTest1()
        {
        }
    }
}
