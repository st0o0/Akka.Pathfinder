﻿using Akka.Pathfinder.Core.Configs;
using Akka.Pathfinder.Core.Messages;
using Akka.Persistence;
using Akka.Pathfinder.Core.States;
using Akka.Actor;
using Akka.Cluster.Sharding;
using Akka.Pathfinder.Core;

namespace Akka.Pathfinder.Workers;

public record LocalPointConfig(PointConfig Config);

public partial class PointWorker : ReceivePersistentActor
{
    public override string PersistenceId => $"PointWorker_{EntityId}";
    public string EntityId;
    private PointWorkerState _state = null!;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Serilog.ILogger _logger = Serilog.Log.Logger.ForContext<PointWorker>();
    private readonly IActorRef _mapManagerClient = ActorRefs.Nobody;
    public PointWorker(string entityId, IServiceProvider serviceProvider)
    {
        Context.SetReceiveTimeout(TimeSpan.FromSeconds(20));
        EntityId = entityId;
        _serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        _mapManagerClient = Context.System.GetRegistry().Get<MapManagerProxy>();

        var result = Context.System.EventStream.Subscribe(Self, typeof(PathfinderDeactivated));
        if (!result)
        {
            _logger.Error("[{PointId}] Subscribe [PathfinderDeactivated] failed", entityId);
        }

        Recover<SnapshotOffer>(RecoverSnapshotOffer);
    }

    protected override void OnReplaySuccess()
    {
        _logger.Debug("[{PointId}][RECOVER] SUCCESS", EntityId);

        if (_state?.Initialize == true)
        {
            Become(Ready);
        }
        else
        {
            Become(Initialize);
        }
    }

    protected override void PreRestart(Exception reason, object message)
        => _logger.Error("[{PointId}] PreRestart(): [{RestartReason}] on [{MessageType}][{MessageData}]",
            EntityId, reason.Message, message.GetType().Name, message);

    protected override void OnPersistFailure(Exception cause, object @event, long sequenceNr)
        => _logger.Error("[{PointId}] OnPersistFailure(): [{ExceptionMessage}] on [{MessageType}][{MessageData}][{SeqNr}]",
            EntityId, cause.Message, @event.GetType().Name, @event, sequenceNr);

    protected override void OnPersistRejected(Exception cause, object @event, long sequenceNr)
        => _logger.Error("[{PointId}] OnPersistRejected(): [{ExceptionMessage}] on [{MessageType}][{MessageData}][{SeqNr}]",
            EntityId, cause.Message, @event.GetType().Name, @event, sequenceNr);
}