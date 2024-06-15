using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppCustom.Models
{
    public class GruopElement
    {

        public string ValueParameter { get; set; }
        public List<ElementByFilter> Elements { get; set; } = new List<ElementByFilter>();
        public SolidColorBrush Background { get; set; }
        public int Count { get; set; }
        public GruopElement() { }
    }
}
