using AppCustom.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Apppanel
{
    [SplitButton("PipeInsu", "Insulation")]
    public class PipeInsulatiton
    {
        [PushButtonDataUI("Pipe Insulation", "Commands.PipeInsulationUpdaterCommand", LinkImage = "pipe (1).png", ToolTip = "Pipe Insulation")]

        public void PipeInsulationUpdater()
        {

        }
        [PushButtonDataUI("All Pipes Insulation", "Commands.AllPipeInsulationCommand", LinkImage = "pipe (1).png", ToolTip = "Un Pipe Insulation")]
        public void All()
        {

        }

       

    }
}
