using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Asset
{
    public static class ResourceAssembly
    {
        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
        public static string GetNamespace()
        {
            return typeof(ResourceAssembly).Namespace + ".";
        }
        public static string GetNameNames()
        {
            return ResourceAssembly.GetAssembly().GetName().Name + ".";
        }

    }
}
