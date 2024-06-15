using AppCustom.Asset;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Library
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PushButtonDataUIAttribute: Attribute, IRibbonItem, IPushButtonData
    {
        public string LinkImage { get ; set ; }
        public string ToolTipImage { get ; set ; }
        public string LongDescription { get ; set ; }
        public string ToolTip { get; set; }

        public string Name { get; }
        public string text { get; }
        public string AssemblyName => CoreAssembly.GetAssemblyLocation();
        public string className { get; }

        public PushButtonDataUIAttribute(string text, string className)
        {
            Guid newGuid = Guid.NewGuid();
            this.Name = newGuid.ToString();  
            this.text = text;   
            this.className = ResourceAssembly.GetNameNames()+ className; 
        }
        public PushButtonData CreatePushButtonData()
        {
            var buttonData = new PushButtonData(Name, text, AssemblyName, className);

            if (!string.IsNullOrEmpty(LinkImage))
            {

                var bitmapImage = ResourceImage.GetIcon(LinkImage);
                buttonData.LargeImage = bitmapImage;
                buttonData.Image = bitmapImage;
            }

            if (!string.IsNullOrEmpty(ToolTipImage))
            {

                var bitmapImage = ResourceImage.GetIcon(LinkImage);
                buttonData.ToolTipImage = bitmapImage;
            }

            if (!string.IsNullOrEmpty(LongDescription))
            {
                buttonData.LongDescription = LongDescription;
            }

            if (!string.IsNullOrEmpty(ToolTip))
            {
                buttonData.ToolTip = ToolTip;
            }

            return buttonData;
        }
    }
}
