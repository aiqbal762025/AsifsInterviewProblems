using System.Reflection;
using BusinessEntities;
using Common;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Indexes;
using SimpleInjector;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using System;

namespace Data
{
    public class DataConfiguration
    {
        public static void Initialize(Container container, Lifestyle lifestyle, bool createIndexes = true)
        {
            var assembly = typeof(DataConfiguration).Assembly;

            container.RegisterSingleton<IListTypeLookup<Assembly>, ListTypeLookup<Assembly>>();

            InitializeAssemblyInstancesService.RegisterAssemblyWithSerializableTypes(container, typeof(User).Assembly);
            InitializeAssemblyInstancesService.RegisterAssemblyWithSerializableTypes(container, assembly);

            InitializeAssemblyInstancesService.Initialize(container, lifestyle, assembly);
            container.RegisterSingleton(() => InitializeDocumentStore(assembly, createIndexes));

            container.Register(() =>
            {
                var session = container.GetInstance<IDocumentStore>().OpenSession();
                session.Advanced.MaxNumberOfRequestsPerSession = 5000;
                return session;
            }, lifestyle);
        }

        private static IDocumentStore InitializeDocumentStore(Assembly assembly, bool createIndexes)
        {
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:443/" },  // URL to your RavenDB server
                Database = "SampleProject",               // Your database name
                Conventions = new DocumentConventions
                {
                    // Enable optimistic concurrency
                    UseOptimisticConcurrency = true,

                    // Save Enums as integers (instead of strings)
                    SaveEnumsAsIntegers = true
                }
            };

            // Set up the OnBeforeStore event to modify the ID
            //documentStore.OnBeforeStore += (sender, args) =>
            //{
            //    // Ensure the entity is of type User
            //    if (args.Entity is User user)
            //    {
            //        // If the user entity doesn't already have an ID, set one
            //        if (string.IsNullOrEmpty(user.Id.ToString()))
            //        {
            //            // Set a custom ID for the User entity
            //            user.Id = "users/" + Guid.NewGuid().ToString();  // For example: users/<guid>
            //        }
            //    }
            //};

            // Initialize the document store
            documentStore.Initialize();

            // Optionally create indexes if required
            if (createIndexes)
            {
                IndexCreation.CreateIndexes(assembly, documentStore);
            }

            return documentStore;
        }

    }
}