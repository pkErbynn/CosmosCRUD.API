using CosmosCRUDConsole.API.Models;
using CosmosCRUDConsole.API.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//This demo dives into leveraging Cosmos SDK to;
//- create databases and collections
//- write to these collections, and 
//- read documents from collections from db

namespace CosmosCRUDConsole.API
{
    class Program
    {
      

        static void Main(string[] args)
        {

// DB SETUPS
            var cosmosService = new CosmosConfigService();

            //var db = cosmosService.VarifyDatabaseCreated();
            //var coll = cosmosService.VerifyCollectionCreation();
            //var doc = cosmosService.VerifyDocumentCreation(new Castle { Name = "Elmina Castle" });
            //var doc2 = cosmosService.VerifyDocumentCreation(new Castle { Name = "Cape Coast"});

            //Task.FromResult(db, coll, doc);

            //Console.WriteLine("IsDBCreated: " + db.Result);
            //Console.WriteLine("IsCollectionCreated: " + coll.Result);
            //Console.WriteLine("IsDocumentCreated: " + doc2.Result);

// FETCH DATA
            //var castles = cosmosService.GetItemsAsync<Castle>(e => e.Name == "Elmina Castle").Result;
            //Console.WriteLine("BEFORE 4EACH");
            //Console.WriteLine("Count: ", castles.ToList().Count);
            //castles.ToList().ForEach(Console.WriteLine);

            //var castleMatches = cosmosService.GetItemsAsync(e => e.Name.StartsWith("El")).Result;
            //castleMatches.ToList().ForEach(Console.WriteLine);

            //var r3 = cosmosService.GetCastleQuery();
            //Console.WriteLine(r3.Count());
            //r3.ToList().ForEach(Console.WriteLine);

            var r4 = cosmosService.GetByName("Elmina Castle");
            Console.WriteLine(r4.Count());
            r4.ToList().ForEach(Console.WriteLine);

            //var r = cosmosService.GetData<Castle>().Result;
            //Console.WriteLine(r.ToString());
            //Console.WriteLine(r.Result);


// DELETE DATA
            //string docId = "0ad0182f-847f-475b-8486-d16fea58e37a";
            //var r2 = cosmosService.DeleteUserAsync(docId).Result;
            //Console.WriteLine(r2);

            Console.Read();
        }
    }
}
