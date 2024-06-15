﻿using AppCustom.Controller;
using AppCustom.StoreExible;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class SettingDownUPDuctCommand : IExternalCommand
    {
        private Guid SchemaGUID = ExtensibleStorageSettingDuct.SchemaGUID;
        private string FieldName = ExtensibleStorageSettingDuct.FieldName;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ViewSetting view = new ViewSetting();
            view.Mainview.ShowDialog();
            int offset = view.OffsetValue;

            SchemaBuilder schemaBuilder = new SchemaBuilder(SchemaGUID);
            schemaBuilder.SetSchemaName("MyProjectInfoData");
            schemaBuilder.AddSimpleField(FieldName, typeof(int));
            Schema schema = schemaBuilder.Finish();

            using (Transaction trans = new Transaction(doc, "Add Extensible Storage"))
            {
                trans.Start();

                // Access the ProjectInfo
                Element projectInfoElement = new FilteredElementCollector(doc)
                    .OfClass(typeof(ProjectInfo))
                    .FirstOrDefault();

                if (projectInfoElement != null)
                {
                    // Create an entity and set values
                    Entity entity = new Entity(schema);
                    entity.Set(FieldName, offset);

                    // Add the entity to the ProjectInfo
                    projectInfoElement.SetEntity(entity);
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
        // Method to set the offset value in Extensible Storage
      
    }
}
