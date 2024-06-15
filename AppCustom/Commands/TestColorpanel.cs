using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media; // for the graphics
using adWin = Autodesk.Windows;
namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class TestColorpanel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            adWin.RibbonControl ribbon = adWin.ComponentManager.Ribbon;

            // Open the color dialog to pick a color
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Convert the selected color to System.Windows.Media.Color
                var selectedColor = System.Windows.Media.Color.FromArgb(
                    colorDialog.Color.A,
                    colorDialog.Color.R,
                    colorDialog.Color.G,
                    colorDialog.Color.B
                );
                SolidColorBrush solidBrush = new SolidColorBrush(selectedColor);

                // define a border brush
                var customColorborderBrush = (System.Windows.Media.Color)ColorConverter.ConvertFromString("#0B0B0A");
                SolidColorBrush borderBrush = new SolidColorBrush(customColorborderBrush);

                ribbon.FontSize = 15;

                foreach (adWin.RibbonTab tab in ribbon.Tabs)
                {
                    foreach (adWin.RibbonPanel panel in tab.Panels)
                    {
                        panel.CustomPanelTitleBarBackground = solidBrush;
                        panel.CustomSlideOutPanelBackground = borderBrush;
                        panel.HighlightPanelTitleBar = true;
                    }
                }
            }
            // terst

            //// Tạo cửa sổ tùy chỉnh
            //Window window = new Window
            //{
            //    Title = "My Custom Dialog",
            //    Width = 300,
            //    Height = 200,
            //    Background = new SolidColorBrush(Colors.LightGray)
            //};

            //// Tạo stack panel
            //StackPanel stackPanel = new StackPanel();

            //// Tạo text block
            //TextBlock textBlock = new TextBlock
            //{
            //    Text = "Hello, this is a custom dialog!",
            //    Margin = new Thickness(10)
            //};
            //stackPanel.Children.Add(textBlock);

            //// Tạo nút đóng cửa sổ
            //Button closeButton = new Button
            //{
            //    Content = "Close",
            //    Margin = new Thickness(10)
            //};
            //closeButton.Click += (s, e) => window.Close();
            //stackPanel.Children.Add(closeButton);

            //// Đặt stack panel vào cửa sổ
            //window.Content = stackPanel;

            //// Hiển thị cửa sổ
            //window.ShowDialog();

            //// re


            return Result.Succeeded;
        }
    }
}
