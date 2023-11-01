﻿using Akka.Pathfinder.Core.Configs;

namespace Akka.Pathfinder.AcceptanceTests.InitialData;

//map 0
// dictionary 0, -> Map0()


public static class MapProvider
{
    private const uint _baseCost = 42;
    private static Dictionary<int, MapConfig> _mapConfigs = new();


    public static Dictionary<int, MapConfig> MapConfigs
    {
        get
        {
            if (_mapConfigs.Count == 0)
            {
                _mapConfigs.Add(0, Map0());
                _mapConfigs.Add(1, Map1());
            }

            return _mapConfigs;
        }
        set => _mapConfigs = value;
    }

    private static MapConfig Map0()
    {

        return new MapConfig(0, new List<PointConfig>()
        {
            new(1, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Front, new DirectionConfig(2, _baseCost) },
            }),
            new(2, _baseCost, new Dictionary<Direction, DirectionConfig>()),

        });

    }

    private static MapConfig Map1()
    {

        return new MapConfig(1, new List<PointConfig>()
        {
            new(1, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Left, new DirectionConfig(8, _baseCost) },
            }),
            new(2, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Bottom, new DirectionConfig(1, _baseCost) },
                { Direction.Left, new DirectionConfig(9, _baseCost) },
            }),
            new(3, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Bottom, new DirectionConfig(2, _baseCost) },
            }),
            new(4, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Right, new DirectionConfig(3, _baseCost) },
                { Direction.Left, new DirectionConfig(5, _baseCost) },
            }),
            new(5, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Bottom, new DirectionConfig(5, _baseCost) },
            }),
            new(6, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Bottom, new DirectionConfig(7, _baseCost) },
                { Direction.Right, new DirectionConfig(9, _baseCost) },
            }),
            new(7,_baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Right, new DirectionConfig(8, _baseCost) },
            }),
            new(8, _baseCost,new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Top, new DirectionConfig(9, _baseCost) },
            }),
            new(9, _baseCost, new Dictionary<Direction, DirectionConfig>()
            {
                { Direction.Top, new DirectionConfig(4, _baseCost) },
            })
        });
    }
}