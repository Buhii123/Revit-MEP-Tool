using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppCustom.Views
{
    /// <summary>
    /// Interaction logic for ViewSettingDownUp.xaml
    /// </summary>
    public partial class ViewSettingDownUp : Window
    {
        public ViewSettingDownUp()
        {
            InitializeComponent();
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); // Regex to match non-numeric input

        private void OffsetTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private void OffsetTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (IsTextNumeric(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }


        private static bool IsTextNumeric(string text)
        {
            return _regex.IsMatch(text);
        }
     
    }
}
