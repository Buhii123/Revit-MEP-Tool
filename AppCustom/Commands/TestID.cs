using AppCustom.Test;
using AppCustom.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class TestID : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;
            // Chọn một điểm trên Pipe
            Reference pickedRef = uidoc.Selection.PickObject(ObjectType.PointOnElement, "Select a point on a pipe");
            if (pickedRef == null)
            {
                message = "No point selected.";
                return Result.Failed;
            }

            // Lấy Pipe từ Reference
            Pipe pipe = doc.GetElement(pickedRef) as Pipe;
            if (pipe == null)
            {
                message = "Selected element is not a Pipe.";
                return Result.Failed;
            }

            // Lấy điểm đã chọn
            XYZ selectedPoint = pickedRef.GlobalPoint;

            // Tìm connector gần nhất trên Pipe
            Connector nearestConnector = GetNearestConnector(pipe, selectedPoint);
            if (nearestConnector == null)
            {
                message = "No connectors found on the selected pipe.";
                return Result.Failed;
            }
            ////////////////////////////////////////////////
            XYZ start = nearestConnector.Origin;

            // Tính vector hướng của Pipe
            LocationCurve pipeCurve = pipe.Location as LocationCurve;
            XYZ pipeDirection = (pipeCurve.Curve.GetEndPoint(1) - pipeCurve.Curve.GetEndPoint(0)).Normalize();

            // Trục Z
            XYZ zAxis = XYZ.BasisZ;

            // Tính Cross Product của hướng Pipe và trục Z
            XYZ crossProduct = pipeDirection.CrossProduct(zAxis);

            // Độ dài của Model Line
            double lineLength = 10.0; // Độ dài của Line có thể điều chỉnh

            // Tạo điểm kết thúc cho Model Line mới từ Cross Product
            XYZ lineEnd = start + crossProduct.Multiply(lineLength);

            // Tạo Model Line
            using (Transaction trans = new Transaction(doc, "Create Model Line"))
            {
                trans.Start();

                // Lấy Plane từ mặt phẳng XY
                Plane plane = Plane.CreateByNormalAndOrigin(zAxis, start);
                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);

                // Tạo Model Line từ điểm bắt đầu đến điểm kết thúc
                doc.Create.NewModelCurve(Line.CreateBound(start, lineEnd), sketchPlane);

                trans.Commit();
            }

            return Result.Succeeded;
        }


        // Hàm tìm connector gần nhất
        private Connector GetNearestConnector(Pipe pipe, XYZ point)
        {
            ConnectorSet connectors = pipe.ConnectorManager.Connectors;
            Connector nearestConnector = null;
            double minDistance = double.MaxValue;

            foreach (Connector connector in connectors)
            {
                double distance = connector.Origin.DistanceTo(point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestConnector = connector;
                }
            }

            return nearestConnector;
        }
    }
}
