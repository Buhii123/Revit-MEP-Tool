using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppCustom.Library
{
    public  class CreatePanelUIApp
    {
       public RibbonPanel Ribbon { get; }

       public CreatePanelUIApp(UIControlledApplication application, string ribbonName, string tab=null) 
        {
            Ribbon = tab == null ? application.CreateRibbonPanel(ribbonName) : application.CreateRibbonPanel(tab, ribbonName);

        }   
     
        public void CreateApp(params Type[] list)
        {
            foreach (var item in list)
            {
                var classAttributes = item.GetCustomAttributes(false).FirstOrDefault();
                switch (classAttributes)
                {
                    case PulldownButtonAttribute pulldown:
                        foreach (PulldownButtonAttribute attr in item.GetCustomAttributes(typeof(PulldownButtonAttribute), false))
                        {
                            PulldownButton pulldownButton = Ribbon.AddItem(attr.pulldownButtonData) as PulldownButton;
                            attr.CreateAddPushButtonData(item, pulldownButton);
                        }
                        break;

                    case SplitButtonAttribute split:
                        foreach (SplitButtonAttribute attr in item.GetCustomAttributes(typeof(SplitButtonAttribute), false))
                        {
                            SplitButton splitButton = Ribbon.AddItem(attr.splitButtonData) as SplitButton;
                            attr.CreateAddPushButtonData(item, splitButton);
                        }
                        break;

                    case ButtonRvAttribute button:
                        foreach (ButtonRvAttribute attr in item.GetCustomAttributes(typeof(ButtonRvAttribute), false))
                        {
                            attr.CreateAddPushButtonData(item, Ribbon);
                        }
                        break;

                    case StackedButtonRvAttribute stackedButton:
                        foreach (StackedButtonRvAttribute attr in item.GetCustomAttributes(typeof(StackedButtonRvAttribute), false))
                        {
                            attr.CreateAddPushButtonData(item, Ribbon);
                        }
                        break;
                }
            }
        }
    }
}
