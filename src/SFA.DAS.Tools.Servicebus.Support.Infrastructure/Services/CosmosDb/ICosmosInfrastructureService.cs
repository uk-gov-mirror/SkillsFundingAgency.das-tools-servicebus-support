using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services.CosmosDb;

public interface ICosmosInfrastructureService
{
    Task<Container> CreateContainer();
    Task<FeedIterator<T>> GetItemQueryIterator<T>(QueryDefinition queryDefinition);
}