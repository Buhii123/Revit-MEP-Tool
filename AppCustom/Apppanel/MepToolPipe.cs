using AppCustom.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Apppanel
{
    [PulldownButton("Pipe Down/Up")]
    public class MepToolPipe
    {
        [PushButtonDataUI("Up Pipe45", "Commands.UpPipe45Command", LinkImage = "pipe (1).png", ToolTip = "Up Pipe 45")]
        public void UpPipe45()
        {

        }
        [PushButtonDataUI("Up Pipe90", "Commands.UpPipe90Command", LinkImage = "pipe (1).png", ToolTip = "Down Pipe 45 ")]
        public void UpPipe90()
        {

        }
        [PushButtonDataUI("Down Pipe45", "Commands.DownPipe45Command", LinkImage = "pipe (1).png", ToolTip = "Down Pipe 45 ")]
        public void DownPipe45()
        {

        }
        [PushButtonDataUI("Down Pipe90", "Commands.DownPipe90Command", LinkImage = "pipe (1).png", ToolTip = "Down Pipe 45 ")]
        public void DownPipe90()
        {

        }
       
       
    }
   
}
