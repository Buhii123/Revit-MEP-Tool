using AppCustom.StoreExible;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB.Mechanical;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class UpPipe90Command : IExternalCommand
    {

        private Guid SchemaGUID = ExtensibleStorageSettingPipe.SchemaGUID;
        private string FieldName = ExtensibleStorageSettingPipe.FieldName;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Schema schema = Schema.Lookup(SchemaGUID);
            int intValue = schema != null ? ExtensibleStorageSettingPipe.GetStoreExibleOffsetValue(doc, FieldName, SchemaGUID) : 500;
            double offset = intValue / 304.8;

            try
            {
                var pointsRef = uidoc.Selection.PickObjects(ObjectType.PointOnElement, new PipeSelectionFilter(), "Select two points on the Pipe");
                if (pointsRef.Count != 2)
                {
                    message = "Please select exactly two points.";
                    return Result.Failed;
                }

                var elementId = pointsRef.First().ElementId;
                var point1 = pointsRef[0].GlobalPoint;
                var point2 = pointsRef[1].GlobalPoint;

                if (pointsRef[0].ElementId != pointsRef[1].ElementId)
                {
                    message = "Please select two points on the same pipe.";
                    return Result.Failed;
                }

                Pipe pipe = doc.GetElement(elementId) as Pipe;
                XYZ pipeDirection = (pipe.Location as LocationCurve).Curve.GetEndPoint(1) - (pipe.Location as LocationCurve).Curve.GetEndPoint(0);
                XYZ selectedPointsDirection = point2 - point1;
                double dotProduct = pipeDirection.DotProduct(selectedPointsDirection);
               
                using (Transaction trans = new Transaction(doc, "Split and Move Up Pipe"))
                {
                    trans.Start();
                    var getSystem = doc.GetElement(pipe.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsElementId()) as MEPSystemType;
                    ElementId firstSplitId = PlumbingUtils.BreakCurve(doc, elementId, point1);
                    ElementId secondSplitId = dotProduct > 0 ? PlumbingUtils.BreakCurve(doc, elementId, point2) : PlumbingUtils.BreakCurve(doc, firstSplitId, point2);

                    Pipe firstPipe = dotProduct > 0 ? doc.GetElement(firstSplitId) as Pipe : doc.GetElement(secondSplitId) as Pipe;
                    Pipe middlePipe = dotProduct > 0 ? doc.GetElement(secondSplitId) as Pipe : doc.GetElement(firstSplitId) as Pipe;

                    ConnectorSet connectors = middlePipe.ConnectorManager.Connectors;
                    var connectorList = connectors.Cast<Connector>().Take(2).ToList();

                    Connector con1 = connectorList.OrderBy(c => c.Origin.DistanceTo(dotProduct > 0 ? point1 : point2)).First();
                    Connector con2 = connectorList.OrderBy(c => c.Origin.DistanceTo(dotProduct > 0 ? point2 : point1)).First();

                    XYZ moveVector = new XYZ(0, 0, offset);

                    Pipe newPipe = Pipe.Create(doc, pipe.PipeType.Id, ((MEPCurve)pipe).ReferenceLevel.Id, con1, con2);
                    newPipe.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).Set(getSystem.Id);
                    ElementTransformUtils.MoveElement(doc, newPipe.Id, moveVector);

                    CalculateRevit.DeletePipe(doc, middlePipe);

                    Pipe newPipe1 = CalculateRevit.NewPipe(doc, pipe, newPipe);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newPipe, newPipe1);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newPipe1, pipe);

                    Pipe newPipe2 = CalculateRevit.NewPipe(doc, newPipe, firstPipe);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newPipe2, firstPipe);
                    CalculateRevit.CreateElbowFittingBetweenPipe(doc, newPipe2, newPipe);

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
