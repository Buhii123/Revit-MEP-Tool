using AppCustom.Asset;
using AppCustom.Commands.ExcuteAction;
using AppCustom.StoreExible;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls;



namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class DownDuctCommand : IExternalCommand
    {
        private Guid SchemaGUID = ExtensibleStorageSettingDuct.SchemaGUID;
        private string FieldName = ExtensibleStorageSettingDuct.FieldName;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;
            if (doc.IsFamilyDocument) return Result.Cancelled;
            Schema schema = Schema.Lookup(SchemaGUID);
            int intValue = 500;
            if (schema != null)
            {
                intValue = ExtensibleStorageSettingDuct.GetStoreExibleOffsetValue(doc, FieldName, SchemaGUID);
            }

            double offset = Convert.ToDouble(intValue) / 304.8;

            try
            {
                // Prompt the user to select two points on the duct
                var pointsRef = uidoc.Selection.PickObjects(ObjectType.PointOnElement, new DuctSelectionFilter(), "Select two points on the duct");

                // Ensure exactly two points are selected
                if (pointsRef.Count != 2)
                {
                    message = "Please select exactly two points.";
                    return Result.Failed;
                }


                // Check if both points are on the same element
                if (pointsRef[0].ElementId != pointsRef[1].ElementId)
                {
                    message = "Please select two points on the same duct.";
                    return Result.Failed;
                }
                ExcuteActioneRevit.ExcuteDownUpDuct(doc, pointsRef, offset, false);

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
