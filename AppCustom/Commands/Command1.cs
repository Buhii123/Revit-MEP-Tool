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
            try
            {
          
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
          //  return Result.Succeeded;
        }
        public static string GetPath()
        {
            return typeof(Command1).Namespace + "." + nameof(Command1);
        }
    }
}
