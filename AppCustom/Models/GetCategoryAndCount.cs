using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCustom.Models
{
    public class GetCategoryAndCount
    {
        public Category InfoObject { get; set; }
        public int Count { get; set; }
        public List<Element> ElementAll { get; set; }


        public GetCategoryAndCount(Category category, List<Element> elements)
        {
            InfoObject = category;
            Count = CalculateRevit.GetCategoryCount(category.Name, elements);
            ElementAll = CalculateRevit.GetAllElementByCategory(category.Name, elements);

        }
    }
}
