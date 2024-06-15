using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.ExternelEventHandler
{
    public class RotateFittingRightEventHandler : IExternalEventHandler
    {
        public Line direc { get; set; }
        public ElementId fittingId { get; set; }
        public double coner { get; set;}   


        public RotateFittingRightEventHandler() 
        {

        }
        public void Execute(UIApplication app)
        {
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction tran = new Transaction(doc, "Rotate fitting"))
            {
                tran.Start();
                doc.GetElement(fittingId).Location.Rotate(direc, -coner);
                tran.Commit();
            }
        }

        public string GetName()
        {
            return " Rotate Fitting Right";
        }
    }
}
