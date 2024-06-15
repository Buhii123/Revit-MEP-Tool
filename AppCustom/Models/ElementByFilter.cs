using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Models
{
    public class ElementByFilter
    {
        public string Name { get; set; }
        public ElementId Id { get; set; }
        public Autodesk.Revit.DB.Color Color { get; set; }
        public ElementByFilter(string name, ElementId id, Autodesk.Revit.DB.Color color)
        {
            Name = name;
            Id = id;
            Color = color;
        }
    }
}
