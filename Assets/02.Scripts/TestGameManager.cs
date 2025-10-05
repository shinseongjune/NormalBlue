using TMPro;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField] private DungeonDatabase dungeonDB;
    [SerializeField] private DungeonRunner dungeonRunner;

    [SerializeField] private Character_SO character;

    [SerializeField] private int dungeonTier = 0;
    [SerializeField] private DungeonType dungeonType = DungeonType.Red;

    public TMP_InputField Input_dungeonTier;
    public TMP_Dropdown Dropdown_dungeonType;

    public void Btn_DoRun()
    {
        if (Input_dungeonTier != null)
        {
            int.TryParse(Input_dungeonTier.text, out dungeonTier);
            dungeonTier = Mathf.Clamp(dungeonTier, 1, 3) - 1;
        }
        if (Dropdown_dungeonType != null)
        {
            dungeonType = (DungeonType)Dropdown_dungeonType.value;
        }

        dungeonRunner.RunDungeon(character, dungeonDB.dungeons[(int)dungeonType], dungeonTier);
    }
}
