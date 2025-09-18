using UnityEngine;

[CreateAssetMenu(fileName = "new Character", menuName = "SO/Character", order = 0)]
public class Character_SO : ScriptableObject
{
    public string charName;
    public float baseMaxHP;
    public float baseATK;
    public float baseDEF;
}
