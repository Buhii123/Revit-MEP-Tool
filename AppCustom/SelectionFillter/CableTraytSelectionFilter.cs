using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI.Selection;

namespace AppCustom
{
    public class CableTraytSelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element elem)
        {
            return elem is CableTray;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
