using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    public class GetInfoCheckInsulationPipe
    {
        public string PipeType { get; set; }
        public string SytemPipe { get; set; }
        public string InsulationType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string thickness { get; set; }


    }
}
