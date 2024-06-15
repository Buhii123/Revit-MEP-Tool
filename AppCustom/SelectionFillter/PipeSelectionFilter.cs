using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Plumbing;

namespace AppCustom
{
    public class PipeSelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element elem)
        {
            return elem is Pipe;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
