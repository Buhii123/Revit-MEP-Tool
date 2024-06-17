using AppCustom.Asset;
using AppCustom.Controller;
using AppCustom.ExternelEventHandler;
using AppCustom.Storage;
using AppCustom.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;
using AppCustom.ViewModels;
using System.Windows;
using UIFramework;
using Autodesk.Revit.UI.Selection;
using AppCustom.StoreExible;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            AddInId appIdF = uiApp.ActiveAddInId;
            UpdaterId updaterIdF = new UpdaterId(appIdF, GuiIDPipeFitting.SchemaGUID);
            PipeFittingUpdater updater = new PipeFittingUpdater(appIdF);
            if (UpdaterRegistry.IsUpdaterRegistered(updaterIdF))
            {
                // Unregister the updater
                UpdaterRegistry.UnregisterUpdater(updaterIdF);

            }
            if (!UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId()))
            {
                UpdaterRegistry.RegisterUpdater(updater);
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_PipeFitting);
                UpdaterRegistry.AddTrigger(
                    updater.GetUpdaterId(),
                    filter,
                    Element.GetChangeTypeElementAddition());
            }
            return Result.Succeeded;
         
        }
        public static string GetPath()
        {
            return typeof(Command1).Namespace + "." + nameof(Command1);
        }
    }
}
