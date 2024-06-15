using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Library
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StackedButtonRvAttribute : Attribute
    {
        public void CreateAddPushButtonData(Type type, RibbonPanel ribbonPanel)
        {
            var methods = type.GetMethods();
            var listAttr = methods
                .Select(method => method.GetCustomAttribute<PushButtonDataUIAttribute>())
                .Where(attr => attr != null)
                .ToList();

            if (listAttr.Count == 2)
          
                ribbonPanel.AddStackedItems(listAttr[0].CreatePushButtonData(), listAttr[1].CreatePushButtonData());

            else if (listAttr.Count >= 3)
           
                ribbonPanel.AddStackedItems(listAttr[0].CreatePushButtonData(), listAttr[1].CreatePushButtonData(), listAttr[2].CreatePushButtonData());
          


        }
    }
}
