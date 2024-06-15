using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Models
{
    public class SetOptionColor : OverrideGraphicSettings
    {
        public SetOptionColor(Document doc, Autodesk.Revit.DB.Color color)
        {
            var getallFillRegions = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).ToElements().ToList().FirstOrDefault();

            //this.SetProjectionLineColor(color);

            this.SetSurfaceForegroundPatternColor(color);

            this.SetSurfaceBackgroundPatternColor(color);
            this.SetSurfaceBackgroundPatternId(getallFillRegions.Id);
            //this.SetSurfaceForegroundPatternVisible(true);

            //this.SetSurfaceTransparency(35);

            this.SetCutLineColor(color);
        }

    }
}
