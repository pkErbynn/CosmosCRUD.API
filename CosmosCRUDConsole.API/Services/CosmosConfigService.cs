using CosmosCRUDConsole.API.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CosmosCRUDConsole.API.Services
{
    public class CosmosConfigService
    {
        private static readonly string CosmosEndpoint = "https://localhost:8081";
        private static readonly string EmulatorKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string DatabaseId = "Tourism";
        private static readonly string CollectionId = "Castles";

        private readonly DocumentClient client;


        public CosmosConfigService()
        {
            client = new DocumentClient(
               new Uri(CosmosEndpoint),
               EmulatorKey,
               new ConnectionPolicy
               {
                   ConnectionMode = ConnectionMode.Direct,
                   ConnectionProtocol = Protocol.Tcp
               });
        }


        // Build Collection
        public async Task<bool> VarifyDatabaseCreated()
        {
            try
            {
                await client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseId });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while creating DB: "+ ex.Message);
                return false;
            }
        }

        public async Task<bool> VerifyCollectionCreation()
        {
            var databaseUri = UriFactory.CreateDatabaseUri(DatabaseId);
            var castlesCollection = new DocumentCollection { Id = CollectionId };

            try
            {
                await client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, castlesCollection);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
                return false;
            }
        }


        // Post data
        public async Task<bool> Post<T>(T item) where T : class
        {
            var castleCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);

            try
            {
                await client.CreateDocumentAsync(castleCollectionUri, item);  // upsert 
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
                return false;
            }
        }


         // Get data
        public IQueryable<Castle> GetCastleQuery()
        {
            return client.CreateDocumentQuery<Castle>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = 20 });
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var docCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
            var query = client.CreateDocumentQuery<T>(docCollectionUri)     // creating doc query from collection
                                        .Where(predicate)
                                        .AsDocumentQuery();

          
              var results = new List<T>();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }


        // Get By Id
        public async Task<Castle> GetById(string id)
        {
            return await client.ReadDocumentAsync<Castle>(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        //public async Task<Castle> GetCastleById(string id)  // has issue
        //{
        //    var result = client.CreateDocumentQuery<Castle>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId)).Where(i => i.Id == id).FirstOrDefault();
        //    return await Task.FromResult(result);
        //}


        // Get By Name
        public IQueryable<Castle> GetByName(string name)  
        {
            return client.CreateDocumentQuery<Castle>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = 3 }).Where( i => i.Name == name);
        }


        // not recommended 
        //public async Task<dynamic> GetData<T>() where T : class
        //{
        //    try
        //    {
        //        var doc = await client.ReadDocumentFeedAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
        //            new FeedOptions { MaxItemCount = 2 });

        //        return doc;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error occured: " + ex.Message); ;
        //        return null;
        //    }
        //}


        // Update
        public async Task<Castle> UpdateCastleAync(string id, Castle castle)
        {
            try
            {
                var collectionUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);

                var result = await client.ReplaceDocumentAsync(collectionUri, castle);
                return (dynamic)result.Resource;
            }
            catch(Exception e)
            {
                Console.WriteLine("error occured"+ e.Message);
                return new Castle();
            }
        }

        // DELETE
        public async Task<Castle> DeleteUserAsync(string id)
        {
            try
            {
                var collectionUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);

                var result = await client.DeleteDocumentAsync(collectionUri);

                return (dynamic)result.Resource;
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured wile deleting item: " + ex.Message) ;
                return null;
            }
        }

        


        // continue explore here - https://github.com/nhandrew/cosmosdbrestapi/blob/master/Controllers/ItemController.cs
    }
}
