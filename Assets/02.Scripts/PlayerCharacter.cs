using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Character_SO baseData;
    private CharacterStats stats;

    private void Awake()
    {
        stats = gameObject.AddComponent<CharacterStats>();
        stats.Initialize(baseData);
    }
}
