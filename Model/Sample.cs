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
