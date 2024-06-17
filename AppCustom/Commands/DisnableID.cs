using AppCustom.Storage;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
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
    public class DisnableID : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            UIDocument activeUiDocument = app.ActiveUIDocument;
            Document doc = activeUiDocument.Document;


            try
            {
                using (Transaction trans = new Transaction(doc, "Clear Extensible Storage"))
                {
                    trans.Start();

                    // Truy cập vào ProjectInfo
                    ProjectInfo projectInfo = doc.ProjectInformation;

                    if (projectInfo != null)
                    {
                        // Lấy Schema dựa trên GUID
                        Schema schema = Schema.Lookup(InfoItemsStorage.schemaGuid);
                        if (schema != null)
                        {
                            // Kiểm tra nếu Schema tồn tại trong ProjectInfo, sau đó xóa nó
                            if (projectInfo.GetEntity(schema) != null)
                            {
                                projectInfo.DeleteEntity(schema);
                            }
                        }
                    }

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }

        }
        public string GetName() => nameof(DisnableID);
    }
}
