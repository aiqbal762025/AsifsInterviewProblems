using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using Common;
//using Raven.Abstractions.Data;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents;

namespace Data.Repositories
{
    [AutoRegister]
    public class Repository<T> : IRepository<T> where T : IdObject
    {
        private readonly IDocumentStore _documentStore;

        public Repository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }


        public T Get(Guid id)
        {
            using (var session = _documentStore.OpenSession())  // Open a session using documentStore
            {
                return session.Load<T>(id.ToString());  // Load the document using the session
            }
        }

        public void Save(T entity)
        {
            var session = _documentStore.OpenSession();
            session.Store(entity, entity.Id.ToString());
            session.SaveChanges();
        }

        public void Delete(T entity)
        {
            var session = _documentStore.OpenSession();
            session.Delete(entity.Id.ToString());
            session.SaveChanges();
        }
                
        public void DeleteAll<TIndex>() where TIndex : AbstractIndexCreationTask
        {
            var indexName = typeof(TIndex).Name;

            var query = new IndexQuery
            {
                Query = $"from index '{indexName}'" // NOTE: 'from index' syntax is required
            };

            var deleteByQuery = new DeleteByQueryOperation(query);

            // Properly send the delete operation
            _documentStore.Operations.Send(deleteByQuery);
        }
    }
}