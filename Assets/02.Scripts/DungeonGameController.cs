using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameModel gameModel;
    [SerializeField] private DungeonDatabase dungeonDatabase;
    [SerializeField] private DungeonRunner dungeonRunner;

    [SerializeField] private TMP_InputField dungeonIndexInputField;
    [SerializeField] private TMP_Dropdown dungeonTierDropdown;

    [Header("Dungeon Selection")]
    [SerializeField] private int dungeonIndex = 0;
    [SerializeField] private int dungeonTier = 0;

    [Header("Drop Settings")]
    [SerializeField] private List<Item_SO> possibleDrops;
    [SerializeField, Range(0f, 1f)] private float dropChance = 1f;
    [SerializeField] private bool autoEquipOnDrop = true;

    private void Awake()
    {
        // GameModel의 playerStats가 비어있으면 자동으로 찾아줌
        if (gameModel != null && gameModel.playerStats == null)
        {
            var stats = FindFirstObjectByType<CharacterStats>();
            if (stats != null)
            {
                gameModel.SetPlayer(stats);
            }
        }
    }

    private void Start()
    {
        SetDungeonIndex();
        SetDungeonTier();
        
    }

    // UI 버튼에서 이 메서드를 연결해서 사용
    public void RunSelectedDungeon()
    {
        if (gameModel == null)
        {
            Debug.LogWarning("GameModel이 설정되지 않았습니다.");
            return;
        }
        if (gameModel.playerStats == null)
        {
            var stats = FindFirstObjectByType<CharacterStats>();
            if (stats != null)
            {
                gameModel.SetPlayer(stats);
            }
            else
            {
                Debug.LogWarning("playerStats가 설정되지 않았습니다.");
                return;
            }
        }

        if (dungeonDatabase == null ||
            dungeonDatabase.dungeons == null ||
            dungeonIndex < 0 ||
            dungeonIndex >= dungeonDatabase.dungeons.Length)
        {
            Debug.LogWarning("DungeonDatabase 설정이 잘못되었습니다.");
            return;
        }

        DungeonData dungeonData = dungeonDatabase.dungeons[dungeonIndex];

        if (dungeonTier < 0 || dungeonTier >= dungeonData.dungeonTiers.Length)
        {
            Debug.LogWarning("Dungeon tier index가 범위를 벗어났습니다.");
            return;
        }

        // 실제 전투 시뮬레이션
        dungeonRunner.RunDungeon(gameModel.playerStats, dungeonData, dungeonTier);

        // 던전 돌고 난 뒤 드랍 처리
        TryDropReward();
    }

    private void TryDropReward()
    {
        if (possibleDrops == null || possibleDrops.Count == 0)
        {
            Debug.Log("possibleDrops가 비어있음. 드랍 없음.");
            return;
        }

        if (Random.value > dropChance)
        {
            Debug.Log("드랍 실패");
            return;
        }

        int index = Random.Range(0, possibleDrops.Count);
        Item_SO drop = possibleDrops[index];

        Debug.Log($"아이템 드랍: {drop.itemName}");

        gameModel.AddToInventory(drop);

        if (autoEquipOnDrop)
        {
            gameModel.Equip(drop);
            Debug.Log($"아이템 장비: {drop.itemName}");
        }
    }

    public void InputField_SetDungeonIndex()
    {
        SetDungeonIndex();
    }

    private void SetDungeonIndex()
    {
        dungeonIndex = int.Parse(dungeonIndexInputField.text);
        print(dungeonIndexInputField.text);
    }

    public void Dropdown_SetDungeonTier()
    {
        SetDungeonTier();
    }

    private void SetDungeonTier()
    {
        dungeonTier = dungeonTierDropdown.value;
        print(dungeonTierDropdown.value);
    }
}
