using System;
using System.Collections.ObjectModel;

namespace Bugx.Test.Model
{
    [Serializable]
    public class Category
    {
        int? _Id;
        string _Name;
        ProductCollection _Products = new ProductCollection();

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public ProductCollection Products
        {
            get { return _Products; }
        }

        public int? Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
    }

    [Serializable]
    public class ProductCollection : Collection<Product>
    {
    }

    [Serializable]
    public class Product
    {
        int? _Id;
        string _Name;
        double _Price;

        public int? Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public double Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
    }
}