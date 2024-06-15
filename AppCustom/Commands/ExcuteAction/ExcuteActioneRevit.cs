using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppCustom.Commands.ExcuteAction
{
    internal static class ExcuteActioneRevit
    {
        public static void ExcuteDownUpDuct(Document doc,IList<Reference> pointsRef, double offset,bool up)
        {
            var elementId = pointsRef.First().ElementId;
            var point1 = pointsRef[0].GlobalPoint;
            var point2 = pointsRef[1].GlobalPoint;
            Duct duct = doc.GetElement(elementId) as Duct;
            XYZ ductDirection = (duct.Location as LocationCurve).Curve.GetEndPoint(1) - (duct.Location as LocationCurve).Curve.GetEndPoint(0);
            XYZ selectedPointsDirection = point2 - point1;
            double dotProduct = ductDirection.DotProduct(selectedPointsDirection);

            using (Transaction trans = new Transaction(doc, "Split and Move Duct"))
            {
                trans.Start();
                ElementId run1 = null;
                ElementId run2 = null;
                Duct duct3 = null;
                Duct middleDuct = null;
                var getSystem = doc.GetElement(duct.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM).AsElementId()) as MEPSystemType;
                if (dotProduct > 0)
                {
                    run1 = MechanicalUtils.BreakCurve(doc, elementId, point1);
                    run2 = MechanicalUtils.BreakCurve(doc, elementId, point2);
                    duct3 = doc.GetElement(run1) as Duct;
                    middleDuct = doc.GetElement(run2) as Duct;
                }
                else
                {
                    run1 = MechanicalUtils.BreakCurve(doc, elementId, point1);
                    run2 = MechanicalUtils.BreakCurve(doc, run1, point2);
                    duct3 = doc.GetElement(run2) as Duct;
                    middleDuct = doc.GetElement(run1) as Duct;
                }
                // Break the duct at the first point

                Duct duct1 = doc.GetElement(elementId) as Duct;

                // Get the middle duct segment
                ElementId levelId = ((MEPCurve)doc.GetElement(elementId)).ReferenceLevel.Id;
                ElementId ductTypeId = duct.DuctType.Id;
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
                Duct newDuct = Duct.Create(doc, ductTypeId, levelId, con1, con2);
                newDuct.get_Parameter(BuiltInParameter.RBS_DUCT_SYSTEM_TYPE_PARAM).Set(getSystem.Id);
                // Move the middle duct segment
                if (up) ElementTransformUtils.MoveElement(doc, newDuct.Id, moveVector);
                else ElementTransformUtils.MoveElement(doc, newDuct.Id, -moveVector);
                CalculateRevit.DeleteDuct(doc, middleDuct);

                //test
                Duct newDuct1 = CalculateRevit.NewDuct(doc, duct1, newDuct);
                CalculateRevit.CreateElbowFittingBetweenDucts(doc, newDuct, newDuct1);
                CalculateRevit.CreateElbowFittingBetweenDucts(doc, newDuct1, duct1);

                Duct newDuct2 = CalculateRevit.NewDuct(doc, newDuct, duct3);
                CalculateRevit.CreateElbowFittingBetweenDucts(doc, newDuct2, duct3);
                CalculateRevit.CreateElbowFittingBetweenDucts(doc, newDuct2, newDuct);
                trans.Commit();
            }
        }
    }
}
