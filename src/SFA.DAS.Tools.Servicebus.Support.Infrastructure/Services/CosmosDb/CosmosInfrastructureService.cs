using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Tools.Servicebus.Support.Domain.Configuration;

namespace SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

public class CosmosInfrastructureService(
    CosmosDbSettings cosmosDbSettings,
    CosmosClient client,
    ICosmosDbPolicies policies)
    : ICosmosInfrastructureService
{
    private readonly int _throughput = cosmosDbSettings.Throughput;
    private readonly string _collectionName = cosmosDbSettings.CollectionName;
    private readonly string _databaseName = cosmosDbSettings.DatabaseName;

    private async Task<Container> CreateContainer(Database database)
    {
        var container = await policies.ResiliencePolicy.ExecuteAsync(() =>
            database.DefineContainer(_collectionName, "/userId")
                .WithIndexingPolicy()
                .WithIncludedPaths()
                .Path("/originatingEndpoint/?")
                .Path("/processingEndpoint/?")
                .Path("/body/?")
                .Path("/exception/?")
                .Path("/exceptionType/?")
                .Path("/type/?")
                .Attach()
                .WithExcludedPaths()
                .Path("/*")
                .Attach()
                .Attach()
                .CreateIfNotExistsAsync(_throughput));

        return container.Container;
    }

    private async Task<Database> CreateDatabase()
    {
        return await policies.ResiliencePolicy.ExecuteAsync(() => client.CreateDatabaseIfNotExistsAsync(_databaseName));
    }

    public async Task<Container> CreateContainer()
    {
        var database = await CreateDatabase();
        return await CreateContainer(database);
    }

    public async Task<FeedIterator<T>> GetItemQueryIterator<T>(QueryDefinition queryDefinition)
    {
        var container = await CreateContainer();
        return await policies.ResiliencePolicy.ExecuteAsync(() => Task.FromResult(container.GetItemQueryIterator<T>(queryDefinition)));
    }
}