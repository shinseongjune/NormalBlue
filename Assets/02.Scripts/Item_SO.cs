using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "SO/Item", order = 1)]
public class Item_SO : ScriptableObject
{
    public string itemName;

    public List<StatMod> mods;
}
