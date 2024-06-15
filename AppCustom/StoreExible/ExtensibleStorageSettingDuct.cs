using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.StoreExible
{
    public static class ExtensibleStorageSettingDuct
    {
      //  [Guid("79549798-95CC-481A-820B-D1F20ECD1CF8")]
        public static Guid SchemaGUID = new Guid("79549798-95CC-481A-820B-D1F20ECD1CF8");
        public static string FieldName = "OffsetValue";


        public static int GetStoreExibleOffsetValue(Document doc, string FieldName, Guid guid)
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

    }
}
