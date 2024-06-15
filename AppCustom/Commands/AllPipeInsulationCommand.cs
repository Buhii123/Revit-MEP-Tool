using AppCustom.Storage;
using AppCustom.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class AllPipeInsulationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var collectorPipes = new FilteredElementCollector(doc)
                            .OfClass(typeof(Pipe))
                            .OfCategory(BuiltInCategory.OST_PipeCurves).ToList();
            var fittingCollector = new FilteredElementCollector(doc)
                            .OfClass(typeof(FamilyInstance))
                            .OfCategory(BuiltInCategory.OST_PipeFitting).ToList();

            var infoItems = InfoItemsStorage.GetInfoItems(doc);

            if (infoItems == null || infoItems.Count == 0)
            {
                TaskDialog.Show("InfoItems", "Chưa Setting!");
                return Result.Cancelled;
            }

            int totalCount = collectorPipes.Count + fittingCollector.Count;
            int currentCount = 0;
            ProgressBarWindow progressBarWindow = new ProgressBarWindow();
            progressBarWindow.SetMaximum(totalCount);
            progressBarWindow.Show();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (TransactionGroup transactionGroup = new TransactionGroup(doc, "Modify Pipe Insulation"))
            {
                transactionGroup.Start();

          
                TransactionMethod.TranTransactionRun(() =>
                {
                    foreach (FamilyInstance pipef in fittingCollector)
                    {
                        CalculateRevit.RemoveInsulationPipeFitting(doc, pipef);
                        currentCount++;
                        progressBarWindow.Dispatcher.Invoke(() => {
                            progressBarWindow.UpdateProgress(currentCount, totalCount);
                        }, DispatcherPriority.Background);
                    }
                },doc, "Remove Insulation from Fittings");
                                      
                TransactionMethod.TranTransactionRun(() =>
                    {
                        foreach (Pipe pipe in collectorPipes)
                        {
                            CalculateRevit.RemoveInsulationPipe(doc, pipe);
                            CalculateRevit.ProcessCheckPipe(doc, pipe, infoItems);
                            currentCount++;
                            progressBarWindow.Dispatcher.Invoke(() => {
                                progressBarWindow.UpdateProgress(currentCount, totalCount);
                            }, DispatcherPriority.Background);
                        }
                    }, doc, "Remove and Add Insulation to Pipes");

                transactionGroup.Assimilate();
            }

            stopwatch.Stop();
            progressBarWindow.Close();

            TaskDialog.Show("Thành Công!", "Hoàn Thành Tiến Trình");

            return Result.Succeeded;
        }

    }
}
