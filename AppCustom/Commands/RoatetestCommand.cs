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

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class RoatetestCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var selectionFitting = uidoc.Selection.GetElementIds().ToList();
            if(selectionFitting == null)
            {
                TaskDialog.Show("Warning","Selected Fiting");
                return Result.Failed;
            }
            var pickDirection = uidoc.Selection.PickObject(ObjectType.Element,new PipeSelectionFilter(), "Select Pipe");
            Pipe pipe = doc.GetElement(pickDirection) as Pipe;
            var pipeCurve = ((LocationCurve)pipe.Location).Curve as Line;

            using (Transaction tran = new Transaction(doc,"Rotate fitting")) 
            { 
                tran.Start();

                doc.GetElement(selectionFitting[0]).Location.Rotate(pipeCurve,-45);
                tran.Commit();  
            }
            return Result.Succeeded;
        }
    }
}
