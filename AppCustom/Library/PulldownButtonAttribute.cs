using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppCustom.Library
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PulldownButtonAttribute : Attribute
    {

        public PulldownButtonData pulldownButtonData { get; }
        public PulldownButtonAttribute(string Name) 
        {
            Guid newGuid = Guid.NewGuid();
            string Unique=newGuid.ToString();   
            pulldownButtonData = new PulldownButtonData(Unique, Name);
    
        }
        public void CreateAddPushButtonData(Type type, PulldownButton pulldown) 
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                var methodAttributes = method.GetCustomAttributes(typeof(PushButtonDataUIAttribute), false);
                foreach (PushButtonDataUIAttribute attr in methodAttributes)
                {
                    pulldown.AddPushButton(attr.CreatePushButtonData());
                }
            }

        }



    }
}
