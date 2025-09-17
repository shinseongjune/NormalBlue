using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    ATK,
    DEF,
}

public enum StatModType
{
    ADD,
    MUL,
}

public class StatMod
{
    public StatModType modType;
    public float value;
}

public class Stat
{
    public StatType type;
    public float baseValue;

    public float Value
    {
        get
        {
            if (isDirty)
            {

                isDirty = false;
            }
            return Value;
        }
    }

    private Dictionary<Item_SO, StatMod> mods;
    private bool isDirty;

    public void ApplyMod(Item_SO item)
    {

        isDirty = true;
    }

    public void RemoveMod(Item_SO item)
    {

        isDirty = true;
    }
}

[CreateAssetMenu(fileName = "new Character", menuName = "SO/Character", order = 0)]
public class Character_SO : ScriptableObject
{
    public string charName;
    public float hp;
    public float maxHP;
    public float atk;
    public float def;
}
