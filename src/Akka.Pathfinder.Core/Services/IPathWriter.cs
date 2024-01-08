using MongoDB.Driver;
using Path = Akka.Pathfinder.Core.Persistence.Data.Path;

namespace Akka.Pathfinder.Core.Services;

public interface IPathWriter : IPathReader
{
    public bool Write(Path path, CancellationToken cancellationToken = default);
}

public class PathWriter(IMongoCollection<Path> collection) : PathReader(collection), IPathWriter
{
    public bool Write(Path path, CancellationToken cancellationToken = default)
    {
        var update = Builders<Path>.Update
        .Set(x => x.Directions, path.Directions)
        .Set(x => x.PathfinderId, path.PathfinderId)
        .Set(x => x.Id, path.Id);

        var result = Collection.UpdateOne(x => x.Id == path.Id, update, new UpdateOptions() { IsUpsert = true }, cancellationToken);
        return result.IsAcknowledged;
    }
}
