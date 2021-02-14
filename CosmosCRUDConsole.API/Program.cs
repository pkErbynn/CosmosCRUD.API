using CosmosCRUDConsole.API.Models;
using CosmosCRUDConsole.API.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

//This demo dives into leveraging Cosmos SDK to;
//- create databases and collections
//- write to these collections, and 
//- read documents from collections from db

namespace CosmosCRUDConsole.API
{
    class Program
    {
        private static readonly string CosmosEndpoint = "https://localhost:8081";
        private static readonly string EmulatorKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string DatabaseId = "Tourism";
        private static readonly string CastlesCollection = "Castles";

        static void Main(string[] args)
        {
            // Create the client connection
            var client = new DocumentClient(
                new Uri(CosmosEndpoint),
                EmulatorKey,
                new ConnectionPolicy
                {
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                });

            // Set up database and collection Uris
            var databaseUri = UriFactory.CreateDatabaseUri(DatabaseId);
            var castleCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CastlesCollection);
            var castlesCollection = new DocumentCollection { Id = CastlesCollection };

            // Create a new db in cosmos
            client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseId }).Wait();  // .Result makes it sync

            // Create new Container(Collection/Table) inside db to store Landmarks
             client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, castlesCollection).Wait(); ;


            // Create Item/Document/Row in Collection to be stored...specifying db and collection to target
            var elCastle = new Castle { Name = "Elmina Castle" };

            client.CreateDocumentAsync(castleCollectionUri, elCastle).Wait();


            // READING DATA....USING STRING QUERY

            var cosmosService = new CosmosService<Castle>
            {
                DocumentClient = client,
                DatabaseId = DatabaseId,
                CollectionId = CastlesCollection
            };

            var castles = cosmosService.GetItemsAsync(e => e.Name == "Elmina Castle").Result;
            Console.WriteLine("BEFORE 4EACH");
            castles.ToList().ForEach(Console.WriteLine);

            var castleMatches = cosmosService.GetItemsAsync(e => e.Name.StartsWith("El")).Result;
            castleMatches.ToList().ForEach(Console.WriteLine);
        }
    }
}
