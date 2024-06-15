using AppCustom.Test;
using AppCustom.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
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
            var collectorPipes = new FilteredElementCollector(doc)
                           .OfClass(typeof(Pipe))
                           .OfCategory(BuiltInCategory.OST_PipeCurves).ToList();
            int bar= 0;


            int totalCount = collectorPipes.Count;
            int currentCount = 0;
            //ProgressBarWindow progressBarWindow = new ProgressBarWindow();
            //progressBarWindow.ProgressWindow.Maximum = collectorPipes.Count;
            //progressBarWindow.Show();
            //foreach (Pipe pipe in collectorPipes) 
            //{
            //    using (Transaction tx = new Transaction(doc, "Remove and add Insulation"))
            //    {
            //        tx.Start();
            //        CalculateRevit.RemoveInsulationPipe(doc, pipe);
            //        tx.Commit();
            //    }
            //    currentCount++;
            //    progressBarWindow.ProgressWindow.Dispatcher.Invoke(() => { progressBarWindow.ProgressWindow.Value= currentCount; }, DispatcherPriority.Background);
            //    //currentCount++;
            //    //progressBarWindow.UpdateProgressBar(currentCount * 100);
            //}
            return Result.Succeeded;
        }
    }
}
