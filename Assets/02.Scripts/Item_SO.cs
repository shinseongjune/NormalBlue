using System.Collections.Generic;
using UnityEngine;

public enum EquipSlot
{
    Weapon,
    Armor,
}

[CreateAssetMenu(fileName = "new Item", menuName = "SO/Item", order = 1)]
public class Item_SO : ScriptableObject
{
    public string itemName;
    public EquipSlot equipSlot;

    public List<StatMod> mods;
}
