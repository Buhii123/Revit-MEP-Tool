using Autodesk.Revit.DB.Mechanical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Models
{
    public class DuctTypeModel
    {
        public string NameDuctType { get; set; }
        public List<Duct> ListDuct { get; set; } = new List<Duct>();
        public DuctTypeModel(DuctType ducttype, List<Duct> duct)
        {
            NameDuctType = ducttype.Name;
            foreach (Duct d in duct)
            {
                if (d.Name == ducttype.Name)
                {
                    ListDuct.Add(d);
                }
            }
        }
        public DuctTypeModel(List<Duct> duct)
        {
            NameDuctType = "All Duct in View";
            ListDuct = duct;
        }
    }
}
