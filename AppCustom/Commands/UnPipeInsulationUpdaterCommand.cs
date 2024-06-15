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
    public class UnPipeInsulationUpdaterCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            AddInId appId = uiApp.ActiveAddInId;

            // Sử dụng appId để tạo UpdaterId
            UpdaterId updaterId = new UpdaterId(appId, new Guid("737F262B-62DF-4B19-A7AA-B3F21E77445D"));

            // Check if the updater is registered
            if (UpdaterRegistry.IsUpdaterRegistered(updaterId))
            {
                // Unregister the updater
                UpdaterRegistry.UnregisterUpdater(updaterId);
                TaskDialog.Show("Thông báo", "Pipe Insulation đã được hủy thành công.");
            }
            else
            {
                TaskDialog.Show("Thông báo", "Updater chưa được đăng ký.");
            }

            return Result.Succeeded;
        }
    }
}
