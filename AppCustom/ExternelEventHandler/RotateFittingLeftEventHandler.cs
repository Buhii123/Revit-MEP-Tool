using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppCustom.ExternelEventHandler
{
    public class RotateFittingLeftEventHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;
            var selectionFitting = uidoc.Selection.GetElementIds().ToList();
            if (selectionFitting == null)
            {
                TaskDialog.Show("Warning", "Selected Fiting");
                return;
            }
            var pickDirection = uidoc.Selection.PickObject(ObjectType.Element, new PipeSelectionFilter(), "Select Pipe");
            Pipe pipe = doc.GetElement(pickDirection) as Pipe;
            var pipeCurve = ((LocationCurve)pipe.Location).Curve as Line;

            using (Transaction tran = new Transaction(doc, "Rotate fitting"))
            {
                tran.Start();

                doc.GetElement(selectionFitting[0]).Location.Rotate(pipeCurve, 45);
                tran.Commit();
            }
            
        }

        public string GetName()
        {
            return " Rotate Fitting Left";
        }
    }
}
