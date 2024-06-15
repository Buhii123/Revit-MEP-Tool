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
    internal class ShowFamilyDockCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Lấy Dockable Pane và hiển thị nó
            DockablePaneId paneId = new DockablePaneId(new Guid("BA44139A-9F40-4EC4-BDBB-3866AB5C2E30"));
            DockablePane pane = commandData.Application.GetDockablePane(paneId);
            pane.Show();
            return Result.Succeeded;
        }
    }
}
