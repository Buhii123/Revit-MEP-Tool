using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Library
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SplitButtonAttribute : Attribute
    {

        public SplitButtonData splitButtonData { get; }
        public SplitButtonAttribute(string Unique, string Name)
        {
            splitButtonData = new SplitButtonData(Unique, Name);

        }
        public void CreateAddPushButtonData(Type type, SplitButton Split)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                var methodAttributes = method.GetCustomAttributes(typeof(PushButtonDataUIAttribute), false);
                foreach (PushButtonDataUIAttribute attr in methodAttributes)
                {
                    Split.AddPushButton(attr.CreatePushButtonData());
                }
            }

        }
    }
}
