using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosmosCRUDConsole.API.Services
{
    public class CosmosService<T> where T : class
    {
        public string CollectionId { get; set; }

        public string DatabaseId { get; set; }

        public DocumentClient DocumentClient { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var docCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
            var query = DocumentClient.CreateDocumentQuery<T>(docCollectionUri)     // creating doc query from collection
                                        .Where(predicate)
                                        .AsDocumentQuery();

          
              var results = new List<T>();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }
    }
}
