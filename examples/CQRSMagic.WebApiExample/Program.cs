using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CQRSMagic.WebApiExample.Products;
using CQRSMagic.WebApiExample.Products.Events;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;

namespace CQRSMagic.WebApiExample
{
    public class Program
    {
        private const string BaseAddress = "http://localhost:9000/";

        public static void Main()
        {
            try
            {
                // Start OWIN host 
                using (WebApp.Start<Startup>(url: BaseAddress))
                {
                    var eventPublisher = ServiceLocator.EventPublisher;

                    eventPublisher.SubscribeTo<AddedProductEvent>(e => new AddedProductEventHandler().Handle(e));

                    AddProduct("Product 1", (decimal)1.23);
                    AddProduct("Product 2", 1024);

                    var products = ShowProducts();

                    var product = products.First();
                    var productId = product.Id;

                    ShowProduct(productId);
                    UpdateProduct(productId, "Product 1 updated", 23);
                    DeleteProduct(productId);
                }

            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(exception);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        private static void AddProduct(string productName, decimal productUnitPrice)
        {
            var uri = string.Format("{0}api/products", BaseAddress);

            WriteHeading("[POST] {0} - Name: {1}, UnitPrice: {2}", uri, productName, productUnitPrice);

            var client = new HttpClient();

            var content = new { name = productName, unitPrice = productUnitPrice };
            var response = client.PostAsJsonAsync(uri, content).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("POST product failed: {0}", response.StatusCode));
            }

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine();
        }

        private static IEnumerable<ProductReadModel> ShowProducts()
        {
            var uri = string.Format("{0}api/products", BaseAddress);

            WriteHeading("[GET] {0}", uri);

            var client = new HttpClient();

            var response = client.GetAsync(uri).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var products = response.Content.ReadAsAsync<IEnumerable<ProductReadModel>>().Result;

            Console.WriteLine(response);
            Console.WriteLine(content);
            Console.WriteLine();
            
            return products;
        }

        private static void ShowProduct(Guid productId)
        {
            var uri = string.Format("{0}api/products/{1}", BaseAddress, productId);

            WriteHeading("[GET] {0}", uri);

            var client = new HttpClient();

            var response = client.GetAsync(uri).Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            Console.WriteLine();
        }

        private static void UpdateProduct(Guid productId, string productUpdated, int i)
        {
            throw new NotImplementedException();
        }

        private static void DeleteProduct(Guid productId)
        {
            throw new NotImplementedException();
        }

        private static void WriteHeading(string format, params object[] args)
        {
            var message = string.Format(format, args);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.WriteLine("-----------------------------".PadRight(message.Length, '-'));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}