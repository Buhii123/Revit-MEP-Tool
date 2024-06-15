using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppCustom.Commands
{
    
    public class PipeInsulationUpdater : IUpdater
    {
        //static AddInId appId;
        //static UpdaterId updaterId;
        //string _nameInsul;
        //double _thicknessFeet;
        //private List<GetInfoCheckInsulationPipe> _checkInsulations { get; } 
        //private static HashSet<ElementId> _processedPipes = new HashSet<ElementId>();
        static AddInId appId;
        static UpdaterId updaterId;
        private List<GetInfoCheckInsulationPipe> _checkInsulations { get; }
        private static HashSet<ElementId> _processedPipes = new HashSet<ElementId>();


        public PipeInsulationUpdater(AddInId id,List<GetInfoCheckInsulationPipe> getinfo)
        {
            appId = id;
            updaterId = new UpdaterId(appId, new Guid("737F262B-62DF-4B19-A7AA-B3F21E77445D"));
            this._checkInsulations = getinfo;   
        }

      

        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
            var newPipeIds = data.GetAddedElementIds().Where(id => !_processedPipes.Contains(id)).ToList();

            foreach (ElementId id in newPipeIds)
            {
                try
                {
                    Pipe pipe = doc.GetElement(id) as Pipe;
                    if (pipe == null) continue;

                    CalculateRevit.ProcessCheckPipe(doc, pipe, _checkInsulations);

                    _processedPipes.Add(id);
                }
                catch
                {
                    continue;
                }
            }
        }







        public string GetAdditionalInformation()
        {
            return "Adds insulation to pipes and notifies the user when a new pipe is added.";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.MEPSystems;
        }

        public UpdaterId GetUpdaterId()
        {
            return updaterId;
        }

        public string GetUpdaterName()
        {
            return "Pipe Insulation Updater";
        }
    }
}
