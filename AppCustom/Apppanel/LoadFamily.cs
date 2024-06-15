using AppCustom.Library;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Apppanel
{
    [ButtonRvAttribute]
    public class LoadFamily
    {
        [PushButtonDataUI("Setting \nPanel", "Commands.TestColorpanel", LinkImage = "icons8-setting-32.png", ToolTip = "Un Pipe Insulation")]
        public void coler()
        {

        }
        [PushButtonDataUI("Load \nFamily", "Commands.ShowFamilyDockCommand", LinkImage = "pipe (1).png",ToolTip = "sadasdzxc")]
        public void LoadFamilys()
        {
        }

        [PushButtonDataUI("Color \nFillter", "Commands.SetColorByFillterCommand", LinkImage = "icons8-color-32 (1).png", ToolTip = "sadasdzxc")]
        public void ColorFillter()
        {
        }

  

        [PushButtonDataUI("Reload \nCommand", "Commands.Command1", ToolTip = "sadasdzxc")]
        public void DataTest1()
        {
        }
        
    }
}
