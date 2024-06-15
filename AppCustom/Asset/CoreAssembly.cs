using Autodesk.Revit.DB;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppCustom.Asset
{
    public static class CoreAssembly
    {
        public static string GetAssemblyLocation()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
        public static string GetDataFileName()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = "AppCustom.data.txt";

            // Adjust resource path for nested resources if needed
            if (!"AppCustom.data.txt".StartsWith(assembly.GetName().Name))
            {
                resourcePath = assembly.GetName().Name + "." + "AppCustom.data.txt".Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Resource not found: " + resourcePath);
                }

                string tempFilePath = Path.GetTempFileName();
                using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                return tempFilePath;
            }
        }
    }
}
