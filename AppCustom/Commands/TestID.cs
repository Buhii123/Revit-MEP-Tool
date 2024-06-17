using AppCustom.Test;
using AppCustom.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class TestID : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var pointsRef = uidoc.Selection.PickObject(ObjectType.Element, "Select two points on the duct");
            var ass = doc.GetElement(pointsRef);
            
            return Result.Succeeded;
        }
    }
}
