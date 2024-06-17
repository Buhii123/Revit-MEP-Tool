using AppCustom.Apppanel;
using AppCustom.Library;
using AppCustom.Test;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media; // for the graphics
using adWin = Autodesk.Windows;
using AppCustom.Controller;
using AppCustom.Views;
using Autodesk.Revit.DB.Events;
using AppCustom.ViewModels;
using AppCustom.Commands;
using System.Windows.Forms;
using Autodesk.Revit.DB.ExtensibleStorage;
using AppCustom.StoreExible;

namespace AppCustom
{
    public class AAppMain : IExternalApplication
    {
     
        public Result OnStartup(UIControlledApplication application)
        {
          
            string tabName = "Buhii Custom";
            application.CreateRibbonTab(tabName);
            CreatePanelUIApp tienit = new CreatePanelUIApp(application, "Tool Buhii", tabName);
            tienit.CreateApp(
                   typeof(LoadFamily)
                );
            CreatePanelUIApp pipe = new CreatePanelUIApp(application, "Tool Pipe", tabName);
            pipe.CreateApp(
                   typeof(PipeInsulatiton),
                   typeof(Rotate3D),
                   typeof(MepToolPipe)
                );
            CreatePanelUIApp Mep = new CreatePanelUIApp(application, "Tool Duct", tabName);
            Mep.CreateApp(
                   typeof(ToolDcut),
                   typeof(MepToolDcut)
            );

            // define an image brush



            RibbonPanel panels = application.CreateRibbonPanel(tabName, "MyPanel");

            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData(
                "MyButtonasdasss",
                "Click Mes",
                path,
                "Namespace.MyCommand");
            

            PushButton pushButton = panels.AddItem(buttonData) as PushButton;
            pushButton.ToolTip = "This is a tooltip";    
            pushButton.Enabled = false;




            // define an image brush
            adWin.RibbonControl ribbon = adWin.ComponentManager.Ribbon;

           

            ImageBrush picBrush = new ImageBrush();
            picBrush.AlignmentX = AlignmentX.Left;
            picBrush.AlignmentY = AlignmentY.Top;
            picBrush.Stretch = Stretch.None;
            picBrush.TileMode = TileMode.FlipXY;
            #region
            // define a linear brush from top to bottom

            //LinearGradientBrush gradientBrush
            //  = new LinearGradientBrush();

            //gradientBrush.StartPoint
            //  = new System.Windows.Point(0, 0);

            //gradientBrush.EndPoint
            //  = new System.Windows.Point(0, 1);

            //gradientBrush.GradientStops.Add(
            //  new GradientStop(Colors.White, 0.0));

            //gradientBrush.GradientStops.Add(
            //  new GradientStop(Colors.Blue, 1));
            //ribbon.FontSize = 15;

            // define a solid color brush
            #endregion
            var customColorsolidBrush = (System.Windows.Media.Color)ColorConverter.ConvertFromString("#868651");

            SolidColorBrush solidBrush = new SolidColorBrush(customColorsolidBrush);

            ribbon.FontSize = 15;

            // iterate through the tabs and their panels

            foreach (adWin.RibbonTab tab in ribbon.Tabs)
            {
                foreach (adWin.RibbonPanel panel in tab.Panels)
                {
                    panel.CustomPanelTitleBarBackground
                      = solidBrush;

                    panel.CustomPanelBackground
                      = picBrush; 

                }
            }
           // [Guid("BA44139A-9F40-4EC4-BDBB-3866AB5C2E30")]
           
            LoadFamilyExternalCommand loadfamily = new LoadFamilyExternalCommand(); 
            ExternalEvent externalEvent = ExternalEvent.Create(loadfamily);
            DockablePaneId paneId = new DockablePaneId(GuidIDLoadFamily.SchemaGUID);
            ViewLoadFamily provider = new ViewLoadFamily(loadfamily, externalEvent)
            {
                
            };
            application.RegisterDockablePane(paneId, "Load Famliy", provider);
            return Result.Succeeded;
        }
      
        public Result OnShutdown(UIControlledApplication application)
        {

            DisnableID disnable = new DisnableID();
            ExternalEvent externalEvent = ExternalEvent.Create(disnable);
            externalEvent.Raise();
            return Result.Succeeded;
        }
   
     
    }
  
}
