using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppCustom.Commands.DownDuctCommand;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class TestDirection : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Prompt the user to select two points on the duct
                var pointsRef = uidoc.Selection.PickObjects(ObjectType.PointOnElement, "Select two points on the duct");

                // Ensure exactly two points are selected
                if (pointsRef.Count != 2)
                {
                    message = "Please select exactly two points.";
                    return Result.Failed;
                }


                // Extract element and point information
                var elementId = pointsRef.First().ElementId;
                var point1 = pointsRef[0].GlobalPoint;
                var point2 = pointsRef[1].GlobalPoint;
                if(pointsRef[0].ElementId != pointsRef[1].ElementId)
                {
                    message = "Please select two points on the same duct.";
                    return Result.Failed;
                }
                Duct duct = doc.GetElement(elementId) as Duct;
                XYZ ductDirection = (duct.Location as LocationCurve).Curve.GetEndPoint(1) - (duct.Location as LocationCurve).Curve.GetEndPoint(0);
                XYZ selectedPointsDirection = point2 - point1;
                double dotProduct = ductDirection.DotProduct(selectedPointsDirection);
                // Check if both points are on the same element
                
        
                using (Transaction trans = new Transaction(doc, "Split and Move Duct"))
                {
                    trans.Start();
                    ElementId run1 =null;
                    ElementId run2 = null;
                    Duct duct3 = null;
                    // Break the duct at the first point
                    if (dotProduct > 0)
                    {
                         run1 = MechanicalUtils.BreakCurve(doc, elementId, point1);
                         run2 = MechanicalUtils.BreakCurve(doc, elementId, point2);
                         duct3 = doc.GetElement(run1) as Duct;
                    }
                    else
                    {
                        run1 = MechanicalUtils.BreakCurve(doc, elementId, point1);
                        run2 = MechanicalUtils.BreakCurve(doc, run1, point2);
                        duct3 = doc.GetElement(run2) as Duct;
                    }              
                                  
                    Duct duct1 = doc.GetElement(elementId) as Duct;
                    
                    List<ElementId> elementIds = new List<ElementId>();

                    elementIds.Add(duct1.Id);
                    elementIds.Add(duct3.Id);
                    uidoc.Selection.SetElementIds(elementIds);
                    uidoc.ShowElements(elementIds);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}
