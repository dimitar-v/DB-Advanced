namespace P03_SalesDatabase
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Data.Models;

    public class StarUp
    {
        static void Main(string[] args)
        {
            using (var db = new SalesContext())
            {
                var productsToAdd = GetProductsToSeed();

                db.Products.AddRange(productsToAdd);

                db.SaveChanges();

                var store = new Store
                {
                    Name = "Lidl"
                };

                db.Stores.Add(store);
                db.SaveChanges();
            }
        }

        private static List<Product> GetProductsToSeed()
        {
            List<Product> products = new List<Product>();

            var product1 = new Product
            {
                Name = "Banana",
                Price = 2.50M,
                Quantity = 10
            };

            var product2 = new Product
            {
                Name = "Tomato",
                Price = 4M,
                Quantity = 20
            };

            var product3 = new Product
            {
                Name = "Apple",
                Price = 1.50M,
                Quantity = 15
            };

            products.Add(product1);
            products.Add(product2);
            products.Add(product3);

            return products;
        }
    }
}
