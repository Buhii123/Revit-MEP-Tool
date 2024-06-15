using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Models
{
    public class ElementFillter : IListItem<Element>
    {
        private FilteredElementCollector _collector;
        public ElementFillter(FilteredElementCollector collector)
        {
            this._collector = collector;
        }
        public List<Element> ListItems()
        {
            return _collector
                .WhereElementIsNotElementType()
                .Where(e => e.Category != null)
                .ToList();
        }
    }
}
