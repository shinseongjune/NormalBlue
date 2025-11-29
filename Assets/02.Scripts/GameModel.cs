using UnityEngine;
using System.Collections.Generic;

public class GameModel : MonoBehaviour
{
    public CharacterStats playerStats;
    public List<Item_SO> inventory = new();
    public List<Item_SO> equippedItems = new();

    public void SetPlayer(CharacterStats stats)
    {
        playerStats = stats;
    }

    public void AddToInventory(Item_SO item)
    {
        if (item == null) return;
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
        }
    }
    public void RemoveFromInventory(Item_SO item)
    {
        if (item == null) return;
        if (playerStats == null)
        {
            var stats = FindFirstObjectByType<CharacterStats>();
            if (stats != null)
            {
                SetPlayer(stats);
            }
            else
            {
                return;
            }
        }
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            if (equippedItems.Contains(item))
            {
                equippedItems.Remove(item);
                playerStats.Unequip(item);
            }
        }
    }

    public void Equip(Item_SO item)
    {
        if (item == null) return;
        if (playerStats == null)
        {
            var stats = FindFirstObjectByType<CharacterStats>();
            if (stats != null)
            {
                SetPlayer(stats);
            }
            else
            {
                return;
            }
        }
        if (inventory.Contains(item) && !equippedItems.Contains(item))
        {
            equippedItems.Add(item);
            playerStats.Equip(item);
        }
    }

    public void Unequip(Item_SO item)
    {
        if (item == null) return;
        if (playerStats == null)
        {
            var stats = FindFirstObjectByType<CharacterStats>();
            if (stats != null)
            {
                SetPlayer(stats);
            }
            else
            {
                return;
            }
        }
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
            playerStats.Unequip(item);
        }
    }

    public void UnequipAll()
    {
        if (playerStats == null)
        {
            var stats = FindFirstObjectByType<CharacterStats>();
            if (stats != null)
            {
                SetPlayer(stats);
            }
            else
            {
                return;
            }
        }
        foreach (var item in equippedItems)
        {
            playerStats.Unequip(item);
        }
        equippedItems.Clear();
    }
}
