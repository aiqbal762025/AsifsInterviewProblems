using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using Common;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;

namespace Data.Repositories
{
    [AutoRegister]
    //public class UserRepository : Repository<User>, IUserRepository
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IDocumentStore _documentStore;

        public UserRepository(IDocumentStore documentStore) : base(documentStore)
        {
            _documentStore = documentStore;
        }

        public IEnumerable<User> Get(UserTypes? userType = null, string name = null, string email = null)
        {
            var session = _documentStore.OpenSession();
            var query = session.Query<User>();

            if (userType != null)
                query = query.Where(u => u.Type == userType);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(u => u.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(u => u.Email == email);

            return query.ToList();
        }

        public void DeleteAll()
        {
            var session = _documentStore.OpenSession();
            var users = session.Query<User>().ToList();
            foreach (var user in users)
                session.Delete(user);

            session.SaveChanges();
        }
    }

    //public class UserRepository : Repository<User>, IUserRepository
    //{
    //    private readonly IDocumentSession _documentSession;

    //    public UserRepository(IDocumentSession documentSession) : base(documentSession)
    //    {
    //        _documentSession = documentSession;
    //    }

    //    public IEnumerable<User> Get(UserTypes? userType = null, string name = null, string email = null)
    //    {
    //        var query = _documentSession.Advanced.DocumentQuery<User, UsersListIndex>();

    //        var hasFirstParameter = false;
    //        if (userType != null)
    //        {
    //            query = query.WhereEquals("Type", (int)userType);
    //            hasFirstParameter = true;
    //        }

    //        if (name != null)
    //        {
    //            if (hasFirstParameter)
    //            {
    //                query = query.AndAlso();
    //            }
    //            else
    //            {
    //                hasFirstParameter = true;
    //            }
    //            query = query.Where($"Name:*{name}*");
    //        }

    //        if (email != null)
    //        {
    //            if (hasFirstParameter)
    //            {
    //                query = query.AndAlso();
    //            }
    //            query = query.WhereEquals("Email", email);
    //        }
    //        return query.ToList();
    //    }

    //    public void DeleteAll()
    //    {
    //        base.DeleteAll<UsersListIndex>();
    //    }
    //}
}