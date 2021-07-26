using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinApp.Data
{
    interface IDocumentRepository<T> where T:class
    {
        Task<Document> CreateItemAsync(T item, string collectionId);
        Task DeleteItemAsync(string name, string collectionId, string PartitionKey);
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, string collectionId);
        Task<IEnumerable<T>> GetItemsAsync(string collectionId);
        Task<Document> UpdateItemAsync(string name, T item, string collectionId);
    }
}
