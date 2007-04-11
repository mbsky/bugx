using System;
using System.Collections.Generic;
using System.Text;

namespace Bugx.Test.Model
{
    public static class Sample
    {
        public static Category BuildCategory()
        {
            Category result = new Category();
            result.Id = 1;
            result.Name = "Test category";
            for(int i = 0; i < 10; i++)
            {
                result.Products.Add(BuildProduct());
            }
            return result;
        }

        static int NextProductId;
        public static Product BuildProduct()
        {
            Product result = new Product();
            result.Id = (NextProductId++ == 0) ? null : (int?)NextProductId;
            result.Name = "Product" + result.Id;
            result.Price = 97*(result.Id ?? 1);
            return result;
        }
    }
}
