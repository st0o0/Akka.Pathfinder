using Akka.Pathfinder.Core.Configs;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Akka.Pathfinder.Core;

public interface IMapConfigReader
{
    IMongoQueryable<MapConfig> Get();
    Task<MapConfig> GetAsync(Guid mapId, CancellationToken cancellationToken = default);
}

public class MapConfigReader(IMongoCollection<MapConfig> collection) : IMapConfigReader
{
    protected IMongoCollection<MapConfig> Collection { get; } = collection;

    public IMongoQueryable<MapConfig> Get()
        => Collection.AsQueryable();

    public Task<MapConfig> GetAsync(Guid mapId, CancellationToken cancellationToken = default)
        => Get().Where(x => x.Id == mapId).SingleOrDefaultAsync(cancellationToken);
}