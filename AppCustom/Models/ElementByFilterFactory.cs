using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Models
{
    public class ElementByFilterFactory
    {
        private Parameter _parameter;
        private System.Windows.Media.Color _color;

        public ElementByFilterFactory(Parameter parameter, System.Windows.Media.Color color)
        {
            _parameter = parameter;
            _color = color;
        }

        public List<ElementByFilter> Create(List<Element> elements, string value)
        {
            int red = _color.R;
            int green = _color.G;
            int blue = _color.B;

            return elements
                .Where(e => e?.LookupParameter(_parameter?.Definition?.Name)?.AsValueString() == value)
                .Select(e => new ElementByFilter(e.Name, e.Id, new Autodesk.Revit.DB.Color((byte)red, (byte)green, (byte)blue)))
                .ToList();
        }
    }
}
