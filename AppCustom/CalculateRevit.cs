using AppCustom.Commands;
using AppCustom.Compares;
using AppCustom.Views;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static Autodesk.Revit.DB.SpecTypeId;

namespace AppCustom
{
    public static class CalculateRevit
    {
        public static Connector GetClosestConnector(ConnectorSet connectors, XYZ point)
        {
            Connector closestConnector = null;
            double minDistance = double.MaxValue;

            foreach (Connector connector in connectors)
            {
                double distance = connector.Origin.DistanceTo(point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestConnector = connector;
                }
            }

            return closestConnector;
        }
        //Duct
        public static XYZ GetMidPoint(Duct Duct1, Duct Duct2) 
        {
            XYZ midPoint = (Duct1.Location as LocationCurve).Curve.GetEndPoint(1) +
                               (Duct2.Location as LocationCurve).Curve.GetEndPoint(0);
            return midPoint/2;
        }
        public static Duct NewDuct(Document doc,Duct duct1,Duct duct2)
        {
          
                XYZ mid = CalculateRevit.GetMidPoint(duct1, duct2);
                ConnectorSet duct1Connectors = duct1.ConnectorManager.Connectors;
                ConnectorSet newDuctConnectors = duct2.ConnectorManager.Connectors;
                Connector duct1End = CalculateRevit.GetClosestConnector(duct1Connectors, mid);
                Connector newDuctEnd = CalculateRevit.GetClosestConnector(newDuctConnectors, mid);
                ElementId levelId = ((MEPCurve)duct1).ReferenceLevel.Id;
                ElementId ductTypeId = duct1.DuctType.Id;
           
            
            return Duct.Create(doc, ductTypeId, levelId, duct1End, newDuctEnd);
        }
     
        public static void CreateElbowFittingBetweenDucts(Document doc, Duct duct1, Duct duct2)
        {
            // Lấy các Connector của duct1 và duct2
            ConnectorSet duct1Connectors = duct1.ConnectorManager.Connectors;
            ConnectorSet duct2Connectors = duct2.ConnectorManager.Connectors;

            // Sử dụng LINQ để tìm các cặp Connector trùng tọa độ
            var matchingConnectors = from Connector connector1 in duct1Connectors
                                     from Connector connector2 in duct2Connectors
                                     where connector1.Origin.IsAlmostEqualTo(connector2.Origin)
                                     select new { Connector1 = connector1, Connector2 = connector2 };

            // Lấy cặp Connector đầu tiên trùng tọa độ
            var firstMatch = matchingConnectors.FirstOrDefault();
          
                // Nếu tìm thấy cặp Connector trùng tọa độ, tạo Elbow Fitting
            if (firstMatch != null) doc.Create.NewElbowFitting(firstMatch.Connector1, firstMatch.Connector2);
          
        }
        public static void DeleteDuct(Document doc, Duct middleDuct)
        {
            ConnectorSet connectors = middleDuct.ConnectorManager.Connectors;
            foreach (Connector connector in connectors)
            {

                foreach (Connector refConnector in connector.AllRefs)
                {
                    if (refConnector.ConnectorType != ConnectorType.End)
                    {
                        connector.DisconnectFrom(refConnector);
                    }
                }
            }
            doc.Delete(middleDuct.Id);
        }
        public static int GetStoreExibleOffsetValueDuct(Document doc, string FieldName, Guid guid)
        {
            Schema schema = Schema.Lookup(guid);

            // Retrieving the data
            Element retrievedProjectInfo = new FilteredElementCollector(doc)
                .OfClass(typeof(ProjectInfo))
                .FirstOrDefault();

            if (retrievedProjectInfo != null)
            {
                Entity retrievedEntity = retrievedProjectInfo.GetEntity(schema);

                if (retrievedEntity.Schema != null)
                {

                    int myInt = retrievedEntity.Get<int>(FieldName);
                    return myInt;
                }
            }
            return 500;
        }

        //Pipe

        public static XYZ GetMidPointPipe(Pipe Pipe1, Pipe Pipe2)
        {
            XYZ midPoint = (Pipe1.Location as LocationCurve).Curve.GetEndPoint(1) +
                               (Pipe2.Location as LocationCurve).Curve.GetEndPoint(0);
            return midPoint / 2;
        }
        public static Pipe NewPipe(Document doc, Pipe Pipe1, Pipe Pipe2)
        {

            XYZ mid = CalculateRevit.GetMidPointPipe(Pipe1, Pipe2);
            ConnectorSet duct1Connectors = Pipe1.ConnectorManager.Connectors;
            ConnectorSet newDuctConnectors = Pipe2.ConnectorManager.Connectors;
            Connector duct1End = CalculateRevit.GetClosestConnector(duct1Connectors, mid);
            Connector newDuctEnd = CalculateRevit.GetClosestConnector(newDuctConnectors, mid);
            ElementId levelId = ((MEPCurve)Pipe1).ReferenceLevel.Id;
            ElementId ductTypeId = Pipe1.PipeType.Id;


            return Pipe.Create(doc, ductTypeId, levelId, duct1End, newDuctEnd);
        }
        public static Pipe NewPipeTest(Document doc,ElementId System, Pipe Pipe1, Pipe Pipe2)
        {

            XYZ mid = CalculateRevit.GetMidPointPipe(Pipe1, Pipe2);
            ConnectorSet duct1Connectors = Pipe1.ConnectorManager.Connectors;
            ConnectorSet newDuctConnectors = Pipe2.ConnectorManager.Connectors;
            Connector duct1End = CalculateRevit.GetClosestConnector(duct1Connectors, mid);
            Connector newDuctEnd = CalculateRevit.GetClosestConnector(newDuctConnectors, mid);
            ElementId levelId = ((MEPCurve)Pipe1).ReferenceLevel.Id;
            ElementId ductTypeId = Pipe1.PipeType.Id;


            return Pipe.Create(doc, System, ductTypeId, levelId, duct1End.Origin, newDuctEnd.Origin);
        }
        public static void CreateElbowFittingBetweenPipe(Document doc, Pipe duct1, Pipe duct2)
        {
            // Lấy các Connector của duct1 và duct2
            ConnectorSet duct1Connectors = duct1.ConnectorManager.Connectors;
            ConnectorSet duct2Connectors = duct2.ConnectorManager.Connectors;

            // Sử dụng LINQ để tìm các cặp Connector trùng tọa độ
            var matchingConnectors = from Connector connector1 in duct1Connectors
                                     from Connector connector2 in duct2Connectors
                                     where connector1.Origin.IsAlmostEqualTo(connector2.Origin)
                                     select new { Connector1 = connector1, Connector2 = connector2 };

            // Lấy cặp Connector đầu tiên trùng tọa độ
            var firstMatch = matchingConnectors.FirstOrDefault();

            // Nếu tìm thấy cặp Connector trùng tọa độ, tạo Elbow Fitting
            if (firstMatch != null) doc.Create.NewElbowFitting(firstMatch.Connector1, firstMatch.Connector2);

        }
        public static void DeletePipe(Document doc, Pipe middleDuct)
        {
            ConnectorSet connectors = middleDuct.ConnectorManager.Connectors;
            foreach (Connector connector in connectors)
            {

                foreach (Connector refConnector in connector.AllRefs)
                {
                    if (refConnector.ConnectorType != ConnectorType.End)
                    {
                        connector.DisconnectFrom(refConnector);
                    }
                }
            }
            doc.Delete(middleDuct.Id);
        }
        //Pipe Fitting
        public static ElementId GetPipeInsulationInfo(ElementId pipeId, Document doc)
        {
            Pipe pipe = doc.GetElement(pipeId) as Pipe;
            var valueInsupipe = pipe.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_TYPE).AsValueString();
            var INSULATION_TYPEID = CalculateRevit.GetPipeInsulationTypeId(doc, valueInsupipe);
            return INSULATION_TYPEID;
        }
        public static double GetThickneesPipeInsulationInfo(ElementId pipeId, Document doc)
        {
            Pipe pipe = doc.GetElement(pipeId) as Pipe;
            var valueInsupipe = pipe.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_THICKNESS).AsDouble();
            return valueInsupipe ;
        }
        public static ElementId GetConnectedPipeId(FamilyInstance pipeFitting,Document doc)
        {
            // Lấy MEPModel từ FamilyInstance
            MEPModel mepModel = pipeFitting.MEPModel;

            if (mepModel?.ConnectorManager?.Connectors != null)
            {
                // Sử dụng LINQ để tìm ElementId của Pipe được kết nối và có Insulation
                return mepModel.ConnectorManager.Connectors
                    .Cast<Connector>()
                    .SelectMany(connector => connector.AllRefs.Cast<Connector>())
                    .Select(connectedConnector => connectedConnector.Owner)
                    .OfType<Pipe>()
                    .FirstOrDefault(pipe => InsulationLiningBase.GetInsulationIds(doc, pipe.Id).Count > 0)?.Id;
            }

            return null;
            //// Lấy MEPModel từ FamilyInstance
            //MEPModel mepModel = pipeFitting.MEPModel;

            //if (mepModel?.ConnectorManager?.Connectors != null)
            //{
            //    // Sử dụng LINQ để tìm ElementId của Pipe được kết nối
            //    return mepModel.ConnectorManager.Connectors
            //        .Cast<Connector>()
            //        .SelectMany(connector => connector.AllRefs.Cast<Connector>())
            //        .Select(connectedConnector => connectedConnector.Owner)
            //        .OfType<Pipe>()
            //        .Select(pipe => pipe.Id)
            //        .FirstOrDefault();
            //}

            //return null;
        }

        //CableTray 
        public static XYZ GetMidPointCableTray(CableTray cableTray1, CableTray cableTray2)
        {
            XYZ midPoint = (cableTray1.Location as LocationCurve).Curve.GetEndPoint(1) +
                           (cableTray2.Location as LocationCurve).Curve.GetEndPoint(0);
            return midPoint / 2;
        }

        public static CableTray NewCableTray(Document doc, CableTray cableTray1, CableTray cableTray2)
        {
            XYZ mid = CalculateRevit.GetMidPointCableTray(cableTray1, cableTray2);
            ConnectorSet tray1Connectors = cableTray1.ConnectorManager.Connectors;
            ConnectorSet newTrayConnectors = cableTray2.ConnectorManager.Connectors;
            Connector tray1End = CalculateRevit.GetClosestConnector(tray1Connectors, mid);
            Connector newTrayEnd = CalculateRevit.GetClosestConnector(newTrayConnectors, mid);
            ElementId levelId = ((MEPCurve)cableTray1).ReferenceLevel.Id;
            ElementId trayTypeId = cableTray1.GetTypeId();

            XYZ startPoint = tray1End.Origin;
            XYZ endPoint = newTrayEnd.Origin;

            return CableTray.Create(doc, trayTypeId, startPoint, endPoint, levelId);
        }
        public static void CreateElbowFittingBetweenCableTray(Document doc, CableTray tray1, CableTray tray2)
        {
            ConnectorSet tray1Connectors = tray1.ConnectorManager.Connectors;
            ConnectorSet tray2Connectors = tray2.ConnectorManager.Connectors;

            var matchingConnectors = from Connector connector1 in tray1Connectors
                                     from Connector connector2 in tray2Connectors
                                     where connector1.Origin.IsAlmostEqualTo(connector2.Origin)
                                     select new { Connector1 = connector1, Connector2 = connector2 };

            var firstMatch = matchingConnectors.FirstOrDefault();

            if (firstMatch != null)
            {
                doc.Create.NewElbowFitting(firstMatch.Connector1, firstMatch.Connector2);
            }
        }

        public static void DeleteCableTray(Document doc, CableTray middleTray)
        {
            ConnectorSet connectors = middleTray.ConnectorManager.Connectors;
            foreach (Connector connector in connectors)
            {
                foreach (Connector refConnector in connector.AllRefs)
                {
                    if (refConnector.ConnectorType != ConnectorType.End)
                    {
                        connector.DisconnectFrom(refConnector);
                    }
                }
            }
            doc.Delete(middleTray.Id);
        }
        // Hàm mở rộng

        public static List<string> GetAllPipeTypeNames(Document doc)
        {
            // Lấy tất cả các PipeTypes từ Document
            var pipeTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(PipeType))
                .Cast<PipeType>();

            // Sử dụng LINQ để chọn tên của từng PipeType
            var pipeTypeNames = pipeTypes.Select(pt => pt.Name).ToList();

            return pipeTypeNames;
        }
        public static List<string> GetAllPipingSystems(Document doc)
        {
            // Lấy tất cả các hệ thống ống từ Document
            var pipingSystems = new FilteredElementCollector(doc)
                .OfClass(typeof(PipingSystemType))
                .Cast<PipingSystemType>();

            // Sử dụng LINQ để chọn tên của từng hệ thống ống
            var pipingSystemNames = pipingSystems.Select(ps => ps.Name).ToList();

            return pipingSystemNames;
        }
        public static List<string> GetAllInsulationPipeTypes(Document doc)
        {
            List<string> insulationPipeTypeNames = new List<string>();

            var collector = new FilteredElementCollector(doc)
                                                .OfClass(typeof(PipeInsulationType))
                                                .Cast<PipeInsulationType>();
            foreach (PipeInsulationType item in collector)
            {
                insulationPipeTypeNames.Add(item.Name);
            }

            return insulationPipeTypeNames;
        }


        //check
        public static bool IsPipeTypeMatched(this Pipe pipe,Document doc,string pipeTypeName)
        {


            // Lấy loại ống từ đối tượng ống
            ElementId pipeTypeId = pipe.GetTypeId();
            PipeType pipeType = doc.GetElement(pipeTypeId) as PipeType;
            // Nếu không thể lấy được loại ống từ đối tượng ống, trả về false
            if (pipeType == null)
            {
                return false;
            }
            // So sánh tên của loại ống với tên được cung cấp
            return pipeType.Name == pipeTypeName;
        }
        public static bool IsPipeInPipingSystem(this Pipe pipe,Document doc, string pipingSystemName)
        {
            // Lấy hệ thống ống mà đối tượng ống thuộc về
         
          
            // Kiểm tra nếu đối tượng ống không thuộc về bất kỳ hệ thống ống nào
            Parameter systemTypeParam = pipe.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM);
            if (systemTypeParam == null)
            {
                return false;
            }

            // So sánh tên của hệ thống ống mà đối tượng ống thuộc về với tên hệ thống ống được cung cấp
            return systemTypeParam.AsValueString() == pipingSystemName;
        }
        public static bool IsLengthPipe(this Pipe pipe, Document doc, string from, string to)
        {
            if (!double.TryParse(from, out double fromValue))
            {
                throw new ArgumentException("Invalid 'from' value. It must be a valid number.");
            }

            if (!double.TryParse(to, out double toValue))
            {
                throw new ArgumentException("Invalid 'to' value. It must be a valid number.");
            }

            // Lấy đường kính danh nghĩa của ống
            Parameter diameterParam = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_DIAMETER_PARAM);
            if (diameterParam == null || !diameterParam.HasValue)
            {
                // Không tìm thấy thông số đường kính hoặc thông số không có giá trị
                return false;
            }
            // Lấy giá trị đường kính và chuyển đổi nó sang đơn vị milimét
            double diameterInMillimeters = diameterParam.AsDouble() * 304.8;

            // Kiểm tra xem đường kính của ống có nằm trong khoảng từ from đến to hay không
            return diameterInMillimeters > fromValue && diameterInMillimeters <= toValue;
        }
        public static void ProcessCheckPipe(Document doc, Pipe pipe, List<GetInfoCheckInsulationPipe> checkInsulations)
        {
            var matchedChecks = checkInsulations.Where(check =>
                pipe.IsPipeTypeMatched(doc, check.PipeType) &&
                pipe.IsPipeInPipingSystem(doc, check.SytemPipe) &&
                pipe.IsLengthPipe(doc, check.From, check.To)
            );

            foreach (var check in matchedChecks)
            {
                ElementId insulationType = CalculateRevit.GetPipeInsulationTypeId(doc, check.InsulationType);
                PipeInsulation.Create(doc, pipe.Id, insulationType, CalculateRevit.ConvertDouble(check.thickness) / 304.8);
            }
        }
        public static void RemoveInsulationPipe(Document doc, Pipe pipe)
        {
            var insulationId = pipe.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_TYPE);    
            if (insulationId.HasValue)
            {
                var ids = PipeInsulation.GetInsulationIds(doc, pipe.Id);
                if (ids != null && ids.Any())
                {
                    doc.Delete(ids);
                }
            }
        }
        public static void RemoveInsulationPipeFitting(Document doc, FamilyInstance pipe)
        {
            var insulationId = pipe.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_TYPE);
            if (insulationId.HasValue)
            {
                var ids = PipeInsulation.GetInsulationIds(doc, pipe.Id);
                if (ids != null && ids.Any())
                {
                    doc.Delete(ids);
                }
            }
        }
        public static ElementId GetPipeInsulationTypeId(Document doc, string name)
        {
            // Tìm và trả về ElementId của loại cách nhiệt mong muốn
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(PipeInsulationType));
            foreach (PipeInsulationType insulationType in collector)
            {
                // Kiểm tra điều kiện để chọn đúng loại cách nhiệt
                if (insulationType.Name == name)
                {
                    return insulationType.Id;
                }
            }
            throw new InvalidOperationException("Pipe insulation type not found.");
        }
        //convert
        public static int GetCategoryCount(string categoryName, List<Element> elements)
        {
            int count = 0;
            foreach (Element element in elements) if (element.Category.Name == categoryName) count++;
            return count;
        }
        public static List<Element> GetAllElementByCategory(string categoryName, List<Element> elements)
        {
            List<Element> elements1 = new List<Element>();
            foreach (Element element in elements) if (element.Category.Name == categoryName)
                    elements1.Add(element);
            return elements1;


        }

        public static List<Parameter> GetParameters(this List<Element> elements)
        {
            var parameters = new List<Parameter>();

            foreach (Element e in elements)
            {
                foreach (Parameter p in e.Parameters)
                {
                    parameters.Add(p);
                }
            }
            return parameters.Distinct(new ParameterComparer()).ToList();
        }
        //
        public static void SetAllDuct(Document doc, Duct elduct, double segmentLength)
        {
            Parameter lengthParameter = elduct.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
            double ductLength = lengthParameter.AsDouble() * 304.8;
            double segmentLengthFeet = segmentLength / 304.8;

            if (ductLength <= segmentLength) return;

            List<Duct> newDucts = BreakDuctIntoSegments(doc, elduct, ductLength, segmentLengthFeet);

            newDucts.Zip(newDucts.Skip(1), (duct1, duct2) => (duct1, duct2))
                    .Where(pair => pair.duct1 != null && pair.duct2 != null)
                    .ToList()
                    .ForEach(pair => TrimDuctWithOni(doc, pair.duct1, pair.duct2));
        }
        private static List<Duct> BreakDuctIntoSegments(Document doc, Duct duct, double ductLength, double segmentLength)
        {
            List<Duct> newDucts = new List<Duct>();
            double remainingLength = ductLength;

            Curve curve = (duct.Location as LocationCurve).Curve;
            XYZ start = curve.GetEndPoint(0);
            XYZ end = curve.GetEndPoint(1);
            XYZ vector = end.Subtract(start).Normalize();

            while (remainingLength > segmentLength * 304.8)
            {
                XYZ breakPoint = start.Add(vector.Multiply(segmentLength));
                try
                {
                    ElementId newDuctId = MechanicalUtils.BreakCurve(doc, duct.Id, breakPoint);
                    Duct newDuct = doc.GetElement(newDuctId) as Duct;
                    if (newDuct != null)
                    {
                        newDucts.Add(newDuct);
                    }
                    start = breakPoint;
                    remainingLength -= segmentLength * 304.8;
                }
                catch (Exception)
                {
                    break;
                }
            }

            newDucts.Add(duct); // Add the original duct at the end
            return newDucts;
        }
        private static void TrimDuctWithOni(Document doc, Duct duct1, Duct duct2)
        {
            var connectors1 = duct1.ConnectorManager.Connectors.Cast<Connector>();
            var connectors2 = duct2.ConnectorManager.Connectors.Cast<Connector>();

            var conPair = connectors1.SelectMany(c1 => connectors2, (c1, c2) => (c1, c2))
                                     .FirstOrDefault(pair => pair.c1.Origin.IsAlmostEqualTo(pair.c2.Origin));

            if (conPair != default)
            {
                doc.Create.NewUnionFitting(conPair.c1, conPair.c2);
            }
        }
        //

        public static double ConvertDouble(string value)
        {
            if (!double.TryParse(value, out double fromValue))
            {
                throw new ArgumentException("Invalid 'from' value. It must be a valid number.");
            }
            return fromValue;
        }   
        public static bool IsAlmostEqualTo(this XYZ point1, XYZ point2, double tolerance = 1e-6)
        {
            return point1.IsAlmostEqualTo(point2, tolerance);
        }
        public static bool IsTooShort(XYZ point1, XYZ point2, double minLength = 0.01)
        {
            double distance = point1.DistanceTo(point2);
            return distance < minLength;
        }


    }

}
