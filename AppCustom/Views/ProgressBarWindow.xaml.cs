using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ProgressBarWindow.xaml
    /// </summary>
    public partial class ProgressBarWindow :Window
    {
        public ProgressBarWindow()
        {
            InitializeComponent();
        }
        public void UpdateProgress(int value, int max)
        {
            ProgressWindow.Value = value;
            ProgressText.Text = $"{(int)((double)value / max * 100)}%";
        }

        public void SetMaximum(int max)
        {
            ProgressWindow.Maximum = max;
        }
    }
}

