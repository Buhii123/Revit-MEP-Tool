using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Compares
{
    public class CategoryComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            else
            {
                return x.Id.Equals(y.Id);
            }
        }

        public int GetHashCode(Category obj)
        {
            return obj.Id.IntegerValue;
        }
    }
}
