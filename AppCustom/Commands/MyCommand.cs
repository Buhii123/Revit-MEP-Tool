using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class MyCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Đường dẫn đến DLL cần tải
            string assemblyPath = @"C: \Users\Admin\AppData\Roaming\Autodesk\Revit\Addins\2023\AppCustom.dll";
        
            // Tạo một AppDomain mới
            AppDomain appDomain = AppDomain.CreateDomain("NewAppDomain");

            // Tải và sử dụng assembly trong AppDomain mới
            appDomain.DoCallBack(() =>
            {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                foreach (Type type in assembly.GetTypes())
                {
                    TaskDialog.Show("Loaded Type", type.FullName);
                }
            });

            // Dỡ bỏ AppDomain
            AppDomain.Unload(appDomain);

            TaskDialog.Show("Info", "Assembly unloaded.");
            return Result.Succeeded;
        }
    }
}
