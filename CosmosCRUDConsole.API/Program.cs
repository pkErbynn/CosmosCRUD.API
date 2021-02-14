using CosmosCRUDConsole.API.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;

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

            // Create a new db in cosmos
            var dbCreationResult = client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseId }).Result;  // .Result makes it sync

            Console.WriteLine("The database Id created is: " + dbCreationResult.Resource.Id);

            // Create new Container(Collection/Table) inside db to store Landmarks
            var collectionCreationResult = client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DatabaseId), 
                new DocumentCollection { Id = CastlesCollection }).Result;

            Console.WriteLine("The collection created has the ID: " + collectionCreationResult.Resource.Id);

            // Create Item/Document/Row in Collection to be stored...specifying db and collection to target
            var elCastle = new Castle { Name = "Elmina Castle" };

            var itemResult = client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CastlesCollection),
                elCastle).Result;

            Console.WriteLine("The document has been created with the ID:  " + itemResult.Resource.Id);


            // READING DATA....targeted db, collection and id of data to read provided
            var document = client.ReadDocumentAsync<Castle>(
                UriFactory.CreateDocumentUri(DatabaseId, CastlesCollection, itemResult.Resource.Id)).Result;

            // convert db document result to Castle POCO
            Castle castle = (dynamic)document;      // "dynamic" does magic  ;-)

            Console.WriteLine("Castle retrieved: " + castle);

        }
    }
}
