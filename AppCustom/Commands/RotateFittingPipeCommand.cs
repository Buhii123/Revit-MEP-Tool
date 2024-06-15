using AppCustom.Controller;
using AppCustom.ExternelEventHandler;
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
    public class RotateFittingPipeCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;
            using(TransactionGroup tranG = new TransactionGroup(doc)) 
            {
                tranG.Start("Rotate Pipe Fitting");
                ICollection<ElementId> selectionFitting = uidoc.Selection.GetElementIds();
                if (selectionFitting == null)
                {
                    TaskDialog.Show("Warning", "Selected Fiting");
                    return Result.Failed;
                }
                var pickDirection = uidoc.Selection.PickObject(ObjectType.Element, new PipeSelectionFilter(), "Select Pipe");
                Pipe pipe = doc.GetElement(pickDirection) as Pipe;
                var pipeCurve = ((LocationCurve)pipe.Location).Curve as Line;


                ViewRotateFittingPipe view = new ViewRotateFittingPipe(doc, selectionFitting, pipeCurve);
                view.Mainview.ShowDialog();
                tranG.Assimilate();
            }
            return Result.Succeeded;
           

        }
    }
}
