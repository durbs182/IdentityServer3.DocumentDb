using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.DocumentDb.Entities;

namespace IdentityServer3.DocumentDb.Repositories.Impl
{
    public class ScopeRepository : RepositoryBase<ScopeDocument>, IScopeRepository
    {
        public ScopeRepository(ICollectionNameResolver resolver, ConnectionSettings setting):base(resolver.ScopeCollectionName, DocumentTypeNames.Scope, setting) { }

        public async Task<IEnumerable<ScopeDocument>> GetByScopeNames(string[] scopeNames)
        {
            //documentdb IN clause limited to 100 values:
            //https://azure.microsoft.com/en-us/blog/new-documentdb-sql-keywords-operators-and-functions/

            //TODO: sql injection sanitized somehow?
            string collectionName = CollectionName;
            string namesSerialized = string.Join(", ", scopeNames.Select(s => $"'{s}'"));

            string sql = $"SELECT * FROM {collectionName} WHERE {collectionName}.doc_type = '{DocumentTypeNames.Scope}' AND {collectionName}.Name IN ({namesSerialized})";
            var query = base.Client.CreateDocumentQuery<ScopeDocument>(Collection.DocumentsLink,  
                sql);
            return await base.QueryAsync(query);
        }

        public async Task AddScope(ScopeDocument scope)
        {
            await base.Upsert(scope);
        }

        public async Task<IEnumerable<ScopeDocument>> GetAllScopes()
        {
            return await base.GetAll();
        }
    }
}