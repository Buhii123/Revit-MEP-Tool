using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Library
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ButtonRvAttribute : Attribute
    {
        public void CreateAddPushButtonData(Type type, RibbonPanel ribbonPanel)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                var methodAttributes = method.GetCustomAttributes(typeof(PushButtonDataUIAttribute), false);
                if (methodAttributes.Length > 0)
                {

                    var firstAttr = (PushButtonDataUIAttribute)methodAttributes[0];
                    ribbonPanel.AddItem(firstAttr.CreatePushButtonData());
                }
            }

        }
    }
}
