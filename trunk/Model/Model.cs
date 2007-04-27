/*
BUGx: An Asp.Net Bug Tracking tool.
Copyright (C) 2007 Olivier Bossaer

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

Wavenet, hereby disclaims all copyright interest in
the library `BUGx' (An Asp.Net Bug Tracking tool) written
by Olivier Bossaer. (olivier.bossaer@gmail.com)
*/

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
    [DebuggerDisplay("Product {Id}: {Name}")]
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