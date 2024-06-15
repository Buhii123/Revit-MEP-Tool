using AppCustom.StoreExible;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class DownCableTrayCommand : IExternalCommand
    {

        private Guid SchemaGUID = ExtensibleStorageSettingPipe.SchemaGUID; // You might need to change this to the appropriate GUID for CableTray
        private string FieldName = ExtensibleStorageSettingPipe.FieldName; // You might need to change this to the appropriate field name for CableTray
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //UIApplication uiApp = commandData.Application;
            //UIDocument uidoc = uiApp.ActiveUIDocument;
            //Document doc = uidoc.Document;

            //Schema schema = Schema.Lookup(SchemaGUID);
            //int intValue = 500;
            //if (schema != null)
            //{
            //    intValue = ExtensibleStorageSettingPipe.GetStoreExibleOffsetValue(doc, FieldName, SchemaGUID); // You might need to change this to the appropriate method for CableTray
            //}

            //double offset = Convert.ToDouble(intValue) / 304.8;

            //try
            //{
            //    // Prompt the user to select two points on the cable tray
            //    var pointsRef = uidoc.Selection.PickObjects(ObjectType.PointOnElement, new CableTraytSelectionFilter(), "Select two points on the Cable Tray");

            //    // Ensure exactly two points are selected
            //    if (pointsRef.Count != 2)
            //    {
            //        message = "Please select exactly two points.";
            //        return Result.Failed;
            //    }

            //    // Extract element and point information
            //    var elementId = pointsRef.First().ElementId;
            //    var point1 = pointsRef[0].GlobalPoint;
            //    var point2 = pointsRef[1].GlobalPoint;

            //    // Check if both points are on the same element
            //    if (pointsRef[0].ElementId != pointsRef[1].ElementId)
            //    {
            //        message = "Please select two points on the same cable tray.";
            //        return Result.Failed;
            //    }

            //    // Check Direction
            //    CableTray cableTray = doc.GetElement(elementId) as CableTray;
            //    XYZ cableTrayDirection = (cableTray.Location as LocationCurve).Curve.GetEndPoint(1) - (cableTray.Location as LocationCurve).Curve.GetEndPoint(0);
            //    XYZ selectedPointsDirection = point2 - point1;
            //    double dotProduct = cableTrayDirection.DotProduct(selectedPointsDirection);

            //    using (Transaction trans = new Transaction(doc, "Split and Move Cable Tray"))
            //    {
            //        trans.Start();
            //        ElementId run1 = null;
            //        ElementId run2 = null;
            //        CableTray cableTray3 = null;
            //        CableTray middleCableTray = null;
            //        if (dotProduct > 0)
            //        {
            //            run1 = ElectricalUtils.BreakCurve(doc, elementId, point1);
            //            run2 = ElectricalUtils.BreakCurve(doc, elementId, point2);
            //            cableTray3 = doc.GetElement(run1) as CableTray;
            //            middleCableTray = doc.GetElement(run2) as CableTray;
            //        }
            //        else
            //        {
            //            run1 = ElectricalUtils.BreakCurve(doc, elementId, point1);
            //            run2 = ElectricalUtils.BreakCurve(doc, run1, point2);
            //            cableTray3 = doc.GetElement(run2) as CableTray;
            //            middleCableTray = doc.GetElement(run1) as CableTray;
            //        }

            //        // Break the cable tray at the first point
            //        CableTray cableTray1 = doc.GetElement(elementId) as CableTray;

            //        // Get the middle cable tray segment
            //        ElementId levelId = ((MEPCurve)doc.GetElement(elementId)).ReferenceLevel.Id;
            //        ElementId cableTrayTypeId = cableTray.GetTypeId();
            //        // NewCableTray
            //        ConnectorSet connectors = middleCableTray.ConnectorManager.Connectors;

            //        var connectorList = connectors.Cast<Connector>().Take(2).ToList();
            //        // Ensure connectors are ordered from start to end
            //        Connector con1 = connectorList.OrderBy(c => c.Origin.DistanceTo(point1)).First();
            //        Connector con2 = connectorList.OrderBy(c => c.Origin.DistanceTo(point2)).First();

            //        XYZ startPoint = con1.Origin;
            //        XYZ endPoint = con2.Origin;

            //        XYZ direction = (endPoint - startPoint).Normalize();
            //        XYZ newStartPoint = startPoint + direction * offset;
            //        XYZ newEndPoint = endPoint - direction * offset;

            //        // Calculate the move vector based on the original position and move distance
            //        XYZ moveVector = new XYZ(0, 0, offset);
            //        con1.Origin = newStartPoint;
            //        con2.Origin = newEndPoint;
            //        CableTray newCableTray = CableTray.Create(doc, cableTrayTypeId, levelId, con1, con2);
            //        // Move the middle cable tray segment
            //        ElementTransformUtils.MoveElement(doc, newCableTray.Id, moveVector);
            //        CalculateRevit.DeleteCableTray(doc, middleCableTray);

            //        // Test
            //        CableTray newCableTray1 = CalculateRevit.NewCableTray(doc, cableTray1, newCableTray);
            //        CalculateRevit.CreateElbowFittingBetweenCableTray(doc, newCableTray, newCableTray1);
            //        CalculateRevit.CreateElbowFittingBetweenCableTray(doc, newCableTray1, cableTray1);

            //        CableTray newCableTray2 = CalculateRevit.NewCableTray(doc, newCableTray, cableTray3);
            //        CalculateRevit.CreateElbowFittingBetweenCableTray(doc, newCableTray2, cableTray3);
            //        CalculateRevit.CreateElbowFittingBetweenCableTray(doc, newCableTray2, newCableTray);

            //        trans.Commit();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    message = ex.Message;
            //    return Result.Failed;
            //}

            return Result.Succeeded;
        }
    }
}
