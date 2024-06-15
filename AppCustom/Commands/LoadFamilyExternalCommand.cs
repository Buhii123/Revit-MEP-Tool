using AppCustom.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class  LoadFamilyExternalCommand : IExternalEventHandler
    {
        public string familyPath { get; set; }
        public string familyName { get; set; }
        public void Execute(UIApplication app)
        {
            UIDocument activeUiDocument = app.ActiveUIDocument;
            Document doc = activeUiDocument.Document;
            Family family = null;
            bool loadSuccess = false;
            if(doc.IsFamilyDocument) {
                System.Windows.MessageBox.Show("No Suport For Family Document!");
                return;
            }
            TransactionMethod.TranTransactionRun(() =>
            {
                loadSuccess= doc.LoadFamily(familyPath + @"\" + familyName, out family);
            }, doc, "Load Family");

            if (loadSuccess && family != null) System.Windows.MessageBox.Show("Load Success!");

            else System.Windows.MessageBox.Show("Load Fail! The Family might be from a higher version.");
        

        }
        public string GetName() => nameof(LoadFamilyExternalCommand);
    }
}
