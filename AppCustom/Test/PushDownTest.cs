using AppCustom.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Test
{
    [PulldownButton("Test2")]
    public class PushDownTest
    {

        [PushButtonDataUI("Test3", "sdasdxzc4", ToolTip = "sadasdzxc")]
        public void DataTest1()
        {

        }
        [PushButtonDataUI("Test2", "sdasdxzc3", ToolTip = "sadasdzxc")]
        public void DataTest2()
        {

        }
        [PushButtonDataUI("Test1", "sdasdxzc2", ToolTip = "sadasdzxc")]
        public void DataTest3()
        {

        }
    }
}
