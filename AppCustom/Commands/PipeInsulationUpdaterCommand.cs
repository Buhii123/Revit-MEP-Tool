using AppCustom.Controller;
using AppCustom.Storage;
using AppCustom.Views;
using Autodesk.Revit.Attributes;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class PipeInsulationUpdaterCommand : IExternalCommand
    {
       
        public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
        {
            
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument; 
            Document doc= uidoc.Document;
            if (doc.IsFamilyDocument)
            {
                TaskDialog.Show("Warrning", "Không thể Dùng Tool!");
                return Result.Cancelled;
            }

            ViewSettingInsulation rs = new ViewSettingInsulation(doc);
            rs.ShowDialog();



            if(rs.infoItems == null)
            {
                return Result.Cancelled;
            }


            // Save infoItems
            InfoItemsStorage.SaveInfoItems(doc, rs.infoItems.ToList());


            AddInId appId = uiApp.ActiveAddInId;
            UpdaterId updaterId = new UpdaterId(appId, new Guid("737F262B-62DF-4B19-A7AA-B3F21E77445D"));
            // Check if the updater is registered
            if (UpdaterRegistry.IsUpdaterRegistered(updaterId))
            {
                // Unregister the updater
                UpdaterRegistry.UnregisterUpdater(updaterId);

            }
            //Load All [Guid("58868B42-DEF6-48DB-9561-4B583549E2A6")]
       

            //end
            PipeInsulationUpdater updater = new PipeInsulationUpdater(appId, rs.infoItems.ToList());
            if (!UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId()))
            {
                UpdaterRegistry.RegisterUpdater(updater);
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
                UpdaterRegistry.AddTrigger(
                    updater.GetUpdaterId(),
                    filter,
                    Element.GetChangeTypeElementAddition());
            }

            return Result.Succeeded;
        }
    }
}
