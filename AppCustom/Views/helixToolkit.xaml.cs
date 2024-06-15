using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using HelixToolkit.Wpf;

namespace AppCustom.Views
{
    public partial class HelixToolkitWindow : Window
    {
        private UIDocument _uidoc;
        private Document _doc;

        public HelixToolkitWindow(UIDocument uidoc)
        {
            InitializeComponent();
            _uidoc = uidoc;
            _doc = _uidoc.Document;
           
        }

    }
}
