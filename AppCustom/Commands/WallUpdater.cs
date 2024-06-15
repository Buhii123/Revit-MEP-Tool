using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    public class WallUpdater : IUpdater
    {
        static AddInId appId;
        static UpdaterId updaterId;
        
        public WallUpdater(AddInId id)
        {
            appId = id;
            updaterId = new UpdaterId(appId, new Guid("6D0ED2BB-C985-41B9-AD7D-74995A7B8189"));
        }

        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
            foreach (ElementId id in data.GetAddedElementIds())
            {
                int a = 0;
                Element element = doc.GetElement(id);
                if (element is Wall)
                {
                    //using (Transaction trans = new Transaction(doc, "Add Pipe Insulationsss"))
                    //{
                    //    trans.Start();
                    //   
                    //    trans.Commit();

                    //}
                        Parameter lengthParam = element.LookupParameter("Length");
                    if (lengthParam != null && !lengthParam.IsReadOnly)
                    {
                       
                            using (Transaction trans = new Transaction(doc, "Update Wall Length"))
                             {
                              trans.Start();
                              lengthParam.Set(100); // Ví dụ: đặt chiều dài của tường là 100 đơn vị
                            
                              trans.Commit();
                          }
                        }
                                    a++;
                }
                TaskDialog.Show("Wall Added", $"A new wall has been added with ID: {a}");
            }
        }

        public string GetAdditionalInformation()
        {
            return "Updates wall parameters when a new wall is added.";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Annotations;
        }

        public UpdaterId GetUpdaterId()
        {
            return updaterId;
        }

        public string GetUpdaterName()
        {
            return "Wall Updater";
        }
    }
}
