using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppCustom.Models
{
    public class GroupElementBuilder
    {
        private GruopElement _gruopElement;
        public GroupElementBuilder()
        {
            _gruopElement = new GruopElement();
        }
        public GroupElementBuilder SetValueParameter(string value)
        {
            _gruopElement.ValueParameter = value;
            return this;
        }
        public GroupElementBuilder SetBackground(System.Windows.Media.Color wpfColor)
        {
            _gruopElement.Background = new SolidColorBrush(wpfColor);
            return this;
        }
        public GroupElementBuilder SetElements(List<Element> elements, Parameter parameter)
        {
            var elementFactory = new ElementByFilterFactory(parameter, _gruopElement.Background.Color);
            _gruopElement.Elements.AddRange(elementFactory.Create(elements, _gruopElement.ValueParameter));
            _gruopElement.Count = _gruopElement.Elements.Count;
            return this;
        }
        public GruopElement Build()
        {
            return _gruopElement;
        }
    }
}
