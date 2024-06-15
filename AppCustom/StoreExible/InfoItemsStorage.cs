using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using AppCustom.Commands;

namespace AppCustom.Storage
{
    public static class InfoItemsStorage
    {
        private static Guid schemaGuid = new Guid("D8E5B3D8-6D0D-4C9E-BD36-7A5B9CE9FACA");
        private static string schemaName = "InfoItemsSchema";

        public static void SaveInfoItems(Document doc, List<GetInfoCheckInsulationPipe> infoItems)
        {
            Schema schema = GetOrCreateSchema();
            Entity entity = new Entity(schema);

            // Store each property of GetInfoCheckInsulationPipe
            IList<string> pipeTypes = infoItems.Select(i => i.PipeType).ToList();
            IList<string> systemPipes = infoItems.Select(i => i.SytemPipe).ToList();
            IList<string> insulationTypes = infoItems.Select(i => i.InsulationType).ToList();
            IList<string> froms = infoItems.Select(i => i.From).ToList();
            IList<string> tos = infoItems.Select(i => i.To).ToList();
            IList<string> thicknesses = infoItems.Select(i => i.thickness).ToList();

            entity.Set("PipeTypes", pipeTypes);
            entity.Set("SystemPipes", systemPipes);
            entity.Set("InsulationTypes", insulationTypes);
            entity.Set("Froms", froms);
            entity.Set("Tos", tos);
            entity.Set("Thicknesses", thicknesses);

            using (Transaction trans = new Transaction(doc, "Save InfoItems"))
            {
                trans.Start();
                doc.ProjectInformation.SetEntity(entity);
                trans.Commit();
            }
        }

        public static List<GetInfoCheckInsulationPipe> GetInfoItems(Document doc)
        {
            Schema schema = GetOrCreateSchema();
            Entity entity = doc.ProjectInformation.GetEntity(schema);

            if (entity.IsValid())
            {
                IList<string> pipeTypes = entity.Get<IList<string>>("PipeTypes");
                IList<string> systemPipes = entity.Get<IList<string>>("SystemPipes");
                IList<string> insulationTypes = entity.Get<IList<string>>("InsulationTypes");
                IList<string> froms = entity.Get<IList<string>>("Froms");
                IList<string> tos = entity.Get<IList<string>>("Tos");
                IList<string> thicknesses = entity.Get<IList<string>>("Thicknesses");

                List<GetInfoCheckInsulationPipe> infoItems = new List<GetInfoCheckInsulationPipe>();
                for (int i = 0; i < pipeTypes.Count; i++)
                {
                    infoItems.Add(new GetInfoCheckInsulationPipe
                    {
                        PipeType = pipeTypes[i],
                        SytemPipe = systemPipes[i],
                        InsulationType = insulationTypes[i],
                        From = froms[i],
                        To = tos[i],
                        thickness = thicknesses[i]
                    });
                }

                return infoItems;
            }

            return null;
        }

        private static Schema GetOrCreateSchema()
        {
            Schema schema = Schema.Lookup(schemaGuid);

            if (schema == null)
            {
                SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGuid);
                schemaBuilder.SetSchemaName(schemaName);
                schemaBuilder.AddArrayField("PipeTypes", typeof(string));
                schemaBuilder.AddArrayField("SystemPipes", typeof(string));
                schemaBuilder.AddArrayField("InsulationTypes", typeof(string));
                schemaBuilder.AddArrayField("Froms", typeof(string));
                schemaBuilder.AddArrayField("Tos", typeof(string));
                schemaBuilder.AddArrayField("Thicknesses", typeof(string));
                schema = schemaBuilder.Finish();
            }

            return schema;
        }
    }
}
