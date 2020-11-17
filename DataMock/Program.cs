using Bogus;
using DataModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace DataMock
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating fake data...");
            var data = FakePerson().Generate(5000);

            CreateDataWithCollections(data);
            CreateDataWithDocuments(data);

            Console.WriteLine("...finished!");
        }

        static void CreateDataWithCollections(List<PersonModel> data)
        {
            Console.WriteLine("Generating data for CreateDataWithCollections...");

            Console.WriteLine("Connect and insert do database...");
            var client = new MongoClient(
                "mongodb://user:user@localhost:27017/DB_BY_COLLECTIONS"
            );
            var database = client.GetDatabase("DB_BY_COLLECTIONS");
            var index = 0;
            Console.Write("Progress = " + index);
            foreach (var person in data)
            {
                ClearCurrentConsoleLine();
                index++;
                Console.Write("Progress = " + index);
                var collection = database.GetCollection<PersonModel>("Persons_id" + person.PersonId.ToString());
                collection.InsertOne(person);
            }
            Console.WriteLine("...done!");
        }

        static void CreateDataWithDocuments(List<PersonModel> data)
        {
            Console.WriteLine("Generating data for CreateDataWithDocuments...");

            Console.WriteLine("Connect and insert do database...");
            var client = new MongoClient(
                "mongodb://user:user@localhost:27018/DB_BY_DOCUMENTS"
            );
            var database = client.GetDatabase("DB_BY_DOCUMENTS");
            var collection = database.GetCollection<PersonModel>("Persons");
            var index = 0;
            Console.Write("Progress = " + index);
            foreach (var person in data)
            {
                ClearCurrentConsoleLine();
                index++;
                Console.Write("Progress = " + index);
                
                collection.InsertOne(person);
            }
            Console.WriteLine("...done!");
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static Faker<OrderModel> FakeOrder()
        {
            return new Faker<OrderModel>()
                .RuleFor(u => u.OrderId, (f, u) => f.Random.Int(0, 999999999))
                .RuleFor(u => u.ProductName, (f, u) => f.Commerce.ProductName())
                .RuleFor(u => u.Price, (f, u) => f.Commerce.Price());
        }

        static Faker<PersonModel> FakePerson()
        {
            return new Faker<PersonModel>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.PersonId, f => f.IndexVariable++)
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.JobArea, (f, u) => f.Name.JobArea())
                .RuleFor(u => u.JobDescriptor, (f, u) => f.Name.JobDescriptor())
                .RuleFor(u => u.JobTitle, (f, u) => f.Name.JobTitle())
                .RuleFor(u => u.JobType, (f, u) => f.Name.JobType())
                .RuleFor(u => u.Orders, () => FakeOrder().Generate(5));
        }
    }
}
