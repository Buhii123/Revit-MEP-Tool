using AppCustom.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom
{
    [StackedButtonRv]
    public class MepToolDcut
    {
        [PushButtonDataUI("Up Duct", "Commands.UpDuctCommand", LinkImage = "pipe (1).png", ToolTip = "Up Duc")]
        public void UpDuct()
        {

        }
        [PushButtonDataUI("Down Duct", "Commands.DownDuctCommand", LinkImage = "pipe (1).png", ToolTip = "Down Duct")]
        public void DownDuct()
        {

        }
       
       
    }
    [ButtonRvAttribute]
    public class ToolDcut
    {
        [PushButtonDataUI("Slit \n Ducts", "Commands.SlitDuctCommand", LinkImage = "pipe (1).png", ToolTip = "sadasdzxc")]
        public void Slit()
        {
        }
        [PushButtonDataUI("Setting\n Offset", "Commands.SettingDownUPDuctCommand", LinkImage = "pipe (1).png", ToolTip = "Setting Offset")]
        public void Setting()
        {

        }
      

    }
}
