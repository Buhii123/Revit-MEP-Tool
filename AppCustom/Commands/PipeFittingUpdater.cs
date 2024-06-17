using AppCustom.StoreExible;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppCustom.Commands
{
    internal class PipeFittingUpdater : IUpdater
    {
        static AddInId appId;
        static UpdaterId updaterId;

        public PipeFittingUpdater(AddInId id)
        {
            appId = id;
            updaterId = new UpdaterId(appId, GuiIDPipeFitting.SchemaGUID);
        }

        public void Execute(UpdaterData data)
        {
           
                Document doc = data.GetDocument();
                var fittings = data.GetAddedElementIds();
                foreach (var ft in fittings)
                {
                    try
                    {
                        FamilyInstance familyIntance = doc.GetElement(ft) as FamilyInstance;
                        ElementId ce = CalculateRevit.GetConnectedPipeId(familyIntance,doc);
                        if (ce != null)
                        {

                          var typeInsu = CalculateRevit.GetPipeInsulationInfo(ce, doc);
                          var thicknessInsu = CalculateRevit.GetThickneesPipeInsulationInfo(ce, doc);
                          PipeInsulation.Create(doc, ft, typeInsu, thicknessInsu);
                        }
                    }
                  
                    catch(Exception ex) {
                   // MessageBox.Show(ex.Message);    
                    continue;
                }   

                    // Xử lý phần tử Piftting mới được thêm vào

                }
       
           
        }  
        
        public UpdaterId GetUpdaterId()
        {
            return updaterId;
        }

       




        public string GetAdditionalInformation()
        {
            return "Adds insulation to pipes and notifies the user when a new pipe is added.";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.MEPSystems;
        }

        

        public string GetUpdaterName()
        {
            return "Pipe Insulation Updater";
        }
    }
}
