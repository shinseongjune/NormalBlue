using System;
using UnityEngine;

public enum DungeonType
{
    Red,
    Green,
    Blue,
}

[Serializable]
public class DungeonData
{
    public DungeonType type;
    public Dungeon_SO[] dungeonTiers;
    public float atkMultiplier = 1f;
    public float hpMultiplier = 1f;
}

public class DungeonDatabase : MonoBehaviour
{
    public DungeonData[] dungeons;
}