using UnityEngine;
using System.Collections.Generic;

public class GameModel : MonoBehaviour
{
    public CharacterStats playerStats;
    public List<Item_SO> inventory = new();
    public List<Item_SO> equippedItems = new();
}
