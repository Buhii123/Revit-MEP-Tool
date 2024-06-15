using AppCustom.Controller;
using AppCustom.Views;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.ExternelEventHandler
{
    public class TestEvent : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            //ViewSetting view = new ViewSetting();
            //view.Mainview.Show();
            TaskDialog.Show("asda", "test");
            //testviewlist rs = new testviewlist();  
            //rs.Mainview.ShowDialog();
        }

        public string GetName()
        {
            return "test";
        }
    }
}
