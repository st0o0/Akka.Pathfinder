﻿using DotNet.Testcontainers.Builders;
using Serilog;
using IContainer = DotNet.Testcontainers.Containers.IContainer;

namespace Akka.Pathfinder.AcceptanceTests.Containers;

public class PostgreContainer : IAsyncLifetime
{
    private const int port = 5432;
    private const string database = "pathfinder";
    private const string username = "mongo";
    private const string password = "mongo";

    public PostgreContainer()
    {
        Log.Information("[TEST][{PostgreContainerName}] ctor", GetType().Name);
        Container = new ContainerBuilder()
            .WithName($"postgres_{Guid.NewGuid():D}")
            .WithImage("postgres:15.1")
            .WithPortBinding(port, true)
            .WithEnvironment("POSTGRES_PASSWORD", password)
            .WithEnvironment("PGPASSWORD", password)
            .WithEnvironment("PGUSER", username)
            .WithEnvironment("PGDATABASE", database)
            .WithCommand("-c", "fsync=off")
            .WithCommand("-c", "full_page_writes=off")
            .WithCommand("-c", "synchronous_commit=off")
            .WithWaitStrategy(Wait.ForUnixContainer()
                .AddCustomWaitStrategy(new WaitUntilLogMessage("database system is ready to accept connections")))
            .Build();
    }

    public int Port { get; private set; }

    public string Hostname { get; private set; } = string.Empty;

    public IContainer Container { get; init; }

    public string GetConnectionString()
    {
        var properties = new Dictionary<string, string>
        {
            { "Host", Hostname },
            { "Port", Port.ToString() },
            { "Database", database },
            { "Username", username },
            { "Password", password }
        };

        return string.Join(";", properties.Select(property => string.Join("=", property.Key, property.Value)));
    }

    public async Task InitializeAsync()
    {
        Log.Information("[TEST][{PostgreContainerName}] InitializeAsync", GetType().Name);

        var timeoutCts = new CancellationTokenSource();
        timeoutCts.CancelAfter(TimeSpan.FromMinutes(1));

        await Container.StartAsync(timeoutCts.Token).ConfigureAwait(false);

        Port = Container.GetMappedPublicPort(port);
        Hostname = Container.Hostname;

        Log.Information("[TEST][{PostgreContainerName}] started and ready on Port [{Hostname}:{PublicPort}]", GetType().Name, Hostname, Port);

        Log.Information("[TEST][{PostgreContainerName}] InitializeAsync finished", GetType().Name);
    }

    public async Task DisposeAsync()
    {
        Log.Information("[TEST][{PostgreContainerName}] DisposeAsync", GetType().Name);
        await Container.StopAsync();
        await Container.DisposeAsync();
        Log.Information("[TEST][{PostgreContainerName}] DisposeAsync finished", GetType().Name);
    }
}