using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Mechanical;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CapOpenEndCommand : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Select a point on a pipe
                Reference pickedRef = uidoc.Selection.PickObject(ObjectType.PointOnElement, new PipeSelectionFilter(), "Select a point on a pipe");
                XYZ pickedPoint = pickedRef.GlobalPoint;
                Pipe selectedPipe = doc.GetElement(pickedRef.ElementId) as Pipe;

                if (selectedPipe == null)
                {
                    message = "Selected element is not a pipe.";
                    return Result.Failed;
                }

                // Find the nearest connector
                Connector nearestConnector = selectedPipe.ConnectorManager.Connectors
                    .Cast<Connector>()
                    .OrderBy(c => c.Origin.DistanceTo(pickedPoint))
                    .FirstOrDefault();

                if (nearestConnector == null)
                {
                    message = "No connector found on the selected pipe.";
                    return Result.Failed;
                }

                double length = CalculateRevit.GetDiameterPipe(selectedPipe);
                XYZ newPipeEndPoint = nearestConnector.Origin + nearestConnector.CoordinateSystem.BasisZ.Normalize() * length;
                double radian = 45 * Math.PI / 180;
                if (!CalculateRevit.IsSameDirectionPipe(selectedPipe, pickedPoint, nearestConnector.Origin)) radian = -45 * Math.PI / 180;

                using (Transaction trans = new Transaction(doc, "Create Pipe at 45 degree"))
                {
                    trans.Start();
                    ElementId levelId = ((MEPCurve)selectedPipe).ReferenceLevel.Id;
                    Pipe newPipe = Pipe.Create(doc, selectedPipe.PipeType.Id, levelId, nearestConnector, newPipeEndPoint);
                    UnconnectNewPipe(doc, newPipe, selectedPipe);
                    Line axis = GetAxisWithBasisZ(nearestConnector, selectedPipe);
                    ElementTransformUtils.RotateElement(doc, newPipe.Id, axis, radian);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newPipe, selectedPipe);
                    PlumbingUtils.PlaceCapOnOpenEnds(doc, newPipe.Id, new ElementId(12346798));

                    trans.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
        private void UnconnectNewPipe(Document doc, Pipe newPipe, Pipe selectedPipe)
        {
            ConnectorManager connectorManager1 = newPipe.ConnectorManager;
            ConnectorManager connectorManager2 = selectedPipe.ConnectorManager;

            // Duyệt qua các Connector của newPipe
            foreach (Connector connector1 in connectorManager1.Connectors)
            {
                // Duyệt qua các Connector của selectedPipe
                foreach (Connector connector2 in connectorManager2.Connectors)
                {
                    // Kiểm tra xem hai Connector có cùng tọa độ và hướng không
                    if (connector1.Origin.IsAlmostEqualTo(connector2.Origin)
                       )
                    {
                        // Ngắt kết nối giữa hai Connector
                        connector1.DisconnectFrom(connector2);
                    }
                }
            }

        }

        private Line GetAxisWithBasisZ(Connector nearestConnector, Pipe pipe)
        {
            XYZ start = nearestConnector.Origin;
            XYZ pipeDirection = ((pipe.Location as LocationCurve).Curve.GetEndPoint(1) - (pipe.Location as LocationCurve).Curve.GetEndPoint(0)).Normalize();
            XYZ crossProduct = pipeDirection.CrossProduct(XYZ.BasisZ);
            double lineLength = 10.0;
            XYZ lineEnd = start + crossProduct.Multiply(lineLength);

            return Line.CreateBound(start, lineEnd);
        }
     
    }
}
