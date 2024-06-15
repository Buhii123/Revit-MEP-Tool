using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.StoreExible
{
    public static class ExtensibleStorageSettingPipe
    {
        //[Guid("F3C5A626-AE0E-4134-9ED0-781B43A37CE2")]
        public static Guid SchemaGUID = new Guid("F3C5A626-AE0E-4134-9ED0-781B43A37CE2");
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
