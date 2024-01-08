using Moin.Core;

namespace Akka.Pathfinder.Core;

public record PointWorkerProxy;

public record PathfinderProxy;

public record MapManagerProxy;

public record SenderManagerProxy;

public static class Endpoints
{
    public static IEndpoint PointWorker => new Endpoint<PointWorkerProxy>("PointWorker", "KEKW", EndpointType.Shard);
    public static IEndpoint PathfinderWorker => new Endpoint<PathfinderProxy>("Pathfinder", "KEKW", EndpointType.Shard);
    public static IEndpoint MapManager => new Endpoint<MapManagerProxy>("MapManager", "KEKW", EndpointType.Singleton);
    public static IEndpoint SenderManager => new Endpoint<SenderManagerProxy>("SenderManager", "KEKW", EndpointType.Singleton);
}