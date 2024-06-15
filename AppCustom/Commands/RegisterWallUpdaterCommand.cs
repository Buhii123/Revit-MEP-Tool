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
    public class RegisterWallUpdaterCommand : IExternalCommand
    {

        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            AddInId appId = uiApp.ActiveAddInId;
            WallUpdater updater = new WallUpdater(appId);

            // Kiểm tra nếu Updater đã được đăng ký
            if (!UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId()))
            {
                UpdaterRegistry.RegisterUpdater(updater);

                // Đăng ký sự kiện thêm tường
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
                UpdaterRegistry.AddTrigger(
                    updater.GetUpdaterId(),
                    filter,
                    Element.GetChangeTypeElementAddition());
            }

            return Result.Succeeded;
        }
    }
}
