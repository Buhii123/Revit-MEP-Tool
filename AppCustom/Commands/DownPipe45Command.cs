using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCustom.StoreExible;
using Autodesk.Revit.DB.Plumbing;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class DownPipe45Command : IExternalCommand
    {
        private Guid SchemaGUID = ExtensibleStorageSettingPipe.SchemaGUID;
        private string FieldName = ExtensibleStorageSettingPipe.FieldName;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Schema schema = Schema.Lookup(SchemaGUID);
            int intValue = 500;
            if (schema != null)
            {
                intValue = ExtensibleStorageSettingPipe.GetStoreExibleOffsetValue(doc, FieldName, SchemaGUID);
            }

            double offset = Convert.ToDouble(intValue) / 304.8;


            try
            {
                // Prompt the user to select two points on the duct
                var pointsRef = uidoc.Selection.PickObjects(ObjectType.PointOnElement, new PipeSelectionFilter(), "Select two points on the Pipe");

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

                // Check if both points are on the same element
                if (pointsRef[0].ElementId != pointsRef[1].ElementId)
                {
                    message = "Please select two points on the same duct.";
                    return Result.Failed;
                }
                //Check Direction
                Pipe duct = doc.GetElement(elementId) as Pipe;
                XYZ ductDirection = (duct.Location as LocationCurve).Curve.GetEndPoint(1) - (duct.Location as LocationCurve).Curve.GetEndPoint(0);
                XYZ selectedPointsDirection = point2 - point1;
                double dotProduct = ductDirection.DotProduct(selectedPointsDirection);

                using (Transaction trans = new Transaction(doc, "Split and Move Pipe"))
                {
                    trans.Start();
                    ElementId run1 = null;
                    ElementId run2 = null;
                    Pipe duct3 = null;
                    Pipe middleDuct = null;
                    ElementId levelId = ((MEPCurve)doc.GetElement(elementId)).ReferenceLevel.Id;
                    ElementId ductTypeId = duct.PipeType.Id;
                    var getSystem = doc.GetElement(duct.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsElementId()) as MEPSystemType;
                    if (dotProduct > 0)
                    {
                        run1 = PlumbingUtils.BreakCurve(doc, elementId, point1);
                        run2 = PlumbingUtils.BreakCurve(doc, elementId, point2);
                        duct3 = doc.GetElement(run1) as Pipe;
                        middleDuct = doc.GetElement(run2) as Pipe;
                    }
                    else
                    {
                        run1 = PlumbingUtils.BreakCurve(doc, elementId, point1);
                        run2 = PlumbingUtils.BreakCurve(doc, run1, point2);
                        duct3 = doc.GetElement(run2) as Pipe;
                        middleDuct = doc.GetElement(run1) as Pipe;
                    }
                    // Break the duct at the first point

                    Pipe duct1 = doc.GetElement(elementId) as Pipe;

                    // Get the middle duct segment

                    // NewDuct
                    ConnectorSet connectors = middleDuct.ConnectorManager.Connectors;


                    var connectorList = connectors.Cast<Connector>().Take(2).ToList();
                    // Ensure connectors are ordered from start to end
                    Connector con1 = connectorList.OrderBy(c => c.Origin.DistanceTo(point1)).First();
                    Connector con2 = connectorList.OrderBy(c => c.Origin.DistanceTo(point2)).First();

                    XYZ startPoint = con1.Origin;
                    XYZ endPoint = con2.Origin;


                    XYZ direction = (endPoint - startPoint).Normalize();
                    XYZ newStartPoint = startPoint + direction * offset;
                    XYZ newEndPoint = endPoint - direction * offset;
                    // Calculate the move vector based on the original position and move distance

                    XYZ moveVector = new XYZ(0, 0, offset);
                    con1.Origin = newStartPoint;
                    con2.Origin = newEndPoint;
                    Pipe newDuct = Pipe.Create(doc, ductTypeId, levelId, con1, con2);
                    newDuct.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).Set(getSystem.Id);
                    // Move the middle duct segment
                    ElementTransformUtils.MoveElement(doc, newDuct.Id, -moveVector);
                    CalculateRevit.DeletePipe(doc, middleDuct);

                    //test
                    Pipe newDuct1 = CalculateRevit.NewPipe(doc, duct1, newDuct);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newDuct, newDuct1);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newDuct1, duct1);

                    Pipe newDuct2 = CalculateRevit.NewPipe(doc, newDuct, duct3);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newDuct2, duct3);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newDuct2, newDuct);
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
