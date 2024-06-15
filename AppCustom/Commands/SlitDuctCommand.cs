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
    [Regeneration(RegenerationOption.Manual)]
    public class SlitDuctCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication UIAPP = commandData.Application;
            UIDocument UIDOC = UIAPP.ActiveUIDocument;
            Document DOC = UIDOC.Document;

            if (DOC.IsFamilyDocument)
            {
                TaskDialog.Show("Warrning", "Không thể Chạy Tool trên Family!");
                return Result.Cancelled;
            }
            try
            {
                #region Code here!

                ControlViewSlitDucts Mainwindow = new ControlViewSlitDucts(UIAPP);
                Mainwindow.MainView.ShowDialog();

                
                #endregion
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Erro", "Lỗi: " + ex.Message);
                return Result.Cancelled;
            }


            return Result.Succeeded;
        }
       
    }
}
