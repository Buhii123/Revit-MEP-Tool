using AppCustom.Controller;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)] 
    public class SetColorByFillterCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

    
            if (doc.IsFamilyDocument)
            {
                TaskDialog.Show("Warrning", "Không thể Dùng Tool!");
                return Result.Cancelled;
            }
            ControllerViewColor main = new ControllerViewColor(doc, uidoc);
            main.Mainview.ShowDialog();

            return Result.Succeeded;
        }
    }
}
