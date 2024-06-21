using AppCustom.Asset;
using AppCustom.Controller;
using AppCustom.ExternelEventHandler;
using AppCustom.Storage;
using AppCustom.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;
using AppCustom.ViewModels;
using System.Windows;
using UIFramework;
using Autodesk.Revit.UI.Selection;
using AppCustom.StoreExible;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Structure;
using System.Windows.Controls;
using Microsoft.Office.Core;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Select a point on a pipe
            Reference pickedRef = uidoc.Selection.PickObject(ObjectType.PointOnElement, "Select a point on a pipe");
            XYZ pickedPoint = pickedRef.GlobalPoint;
            Element pickedElement = doc.GetElement(pickedRef.ElementId);
           
            if (!(pickedElement is Pipe))
            {
                message = "Selected element is not a pipe.";
                return Result.Failed;
            }

            Pipe selectedPipe = pickedElement as Pipe;

            // Find the nearest connector
            Connector nearestConnector = GetNearestConnector(selectedPipe, pickedPoint);
            XYZ nearestConnectorpoint = nearestConnector.Origin;

            if (nearestConnector == null)
            {
                message = "No connector found on the selected pipe.";
                return Result.Failed;
            }
            double lengh = CalculateRevit.GetDiameterPipe(selectedPipe);
            // Calculate the new pipe's position and direction
            XYZ newPipeEndPoint = CalculateNewPipeEndPoint(nearestConnector, lengh);
            double radian = 45 * Math.PI / 180;
            if (!CalculateRevit.IsSameDirectionPipe(selectedPipe, pickedPoint, nearestConnectorpoint)) radian = -45 * Math.PI / 180;

            // Start transaction to create new pipe
            using (Transaction trans = new Transaction(doc, "Create Pipe at 45 degree"))
            {
                trans.Start();


                Pipe newPipe = Pipe.Create(doc, selectedPipe.PipeType.Id, selectedPipe.LevelId, nearestConnector, newPipeEndPoint);        
                UnconectNewPipe(doc, newPipe, selectedPipe);
                Line axis = GetAxis(doc, nearestConnector, selectedPipe);
                ElementTransformUtils.RotateElement(doc, newPipe.Id, axis, radian);
                CalculateRevit.CreateElbowFittingBetweenPipe(doc, newPipe, selectedPipe);
                PlumbingUtils.PlaceCapOnOpenEnds(doc, newPipe.Id, new ElementId(12346798));
 
                trans.Commit();
            }

            return Result.Succeeded;
        }
        private void UnconectNewPipe(Document doc, Pipe newPipe, Pipe  selectedPipe)
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
        private Line GetAxis(Document doc,Connector nearestConnector, Pipe pipe)
        {
            XYZ start = nearestConnector.Origin;
            LocationCurve pipeCurve = pipe.Location as LocationCurve;
            XYZ pipeDirection = (pipeCurve.Curve.GetEndPoint(1) - pipeCurve.Curve.GetEndPoint(0)).Normalize();
            XYZ zAxis = XYZ.BasisZ;
            XYZ crossProduct = pipeDirection.CrossProduct(zAxis);
            double lineLength = 10.0;
            XYZ lineEnd = start + crossProduct.Multiply(lineLength);

            return Line.CreateBound(start, lineEnd);
        }  
        private Connector GetNearestConnector(Pipe pipe, XYZ point)
        {
            Connector nearestConnector = null;
            double minDistance = double.MaxValue;

            ConnectorSet connectors = pipe.ConnectorManager.Connectors;
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
        private XYZ CalculateNewPipeEndPoint(Connector connector, double length)
        {
            XYZ direction = connector.CoordinateSystem.BasisZ.Normalize();
            XYZ newPoint = connector.Origin + direction * length;
            return newPoint;
        }

        
      
    }
}
