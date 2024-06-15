using AppCustom.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Apppanel
{
    [ButtonRvAttribute]
    public class Rotate3D
    {
        [PushButtonDataUI("Rotate \n3D", "Commands.RotateFittingPipeCommand", LinkImage = "pipe (1).png", ToolTip = "Pipe Insulation")]
        public void PipeInsulationUpdater()
        {

        }
        [PushButtonDataUI("Setting \nOffset", "Commands.SettingDownUPPipeCommand", LinkImage = "pipe (1).png", ToolTip = "Setting Offset")]
        public void Setting()
        {

        }
    }
}
