using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct Character
{
    public string charName;
    public float hp;
    public float baseMaxHP;
    public float baseATK;
    public float baseDEF;
}

public class DungeonRunner : MonoBehaviour
{
    [SerializeField] private Transform gameplayTextField;
    [SerializeField] private GameObject prefab_gameplayText;

    [SerializeField] private float randomRangePercent = 0.5f;


    [SerializeField] private List<string> monsterNames = new List<string>()
    {
        "고블린", "오크", "트롤", "스켈레톤", "좀비"
    };

    public void RunDungeon(CharacterStats runnerStats, DungeonData dungeonData, int dungeonTier)
    {
        ClearTextField();

        var runner = new Character()
        {
            charName = runnerStats.data.charName,
            hp = runnerStats.data.baseMaxHP,
            baseMaxHP = runnerStats.data.baseMaxHP,
            baseATK = runnerStats.data.baseATK,
            baseDEF = runnerStats.data.baseDEF
        };
        var dungeon = dungeonData.dungeonTiers[dungeonTier];

        float atkMul = dungeonData.atkMultiplier;
        float hpMul = dungeonData.hpMultiplier;

        int winCount = 0;
        while (winCount < 10 && runner.hp > 0)
        {
            var monster = MakeMonster(dungeon, atkMul, hpMul);

            bool win = Fight(ref runner, monster);

            GameObject line = Instantiate(prefab_gameplayText, gameplayTextField);
            line.GetComponent<TextMeshProUGUI>().text = "------------------------------";

            if (win)
            {
                winCount++;
            }
        }

        GameObject fin = Instantiate(prefab_gameplayText, gameplayTextField);
        if (winCount >= 10)
        {
            fin.GetComponent<TextMeshProUGUI>().text = "던전 클리어!";
        }
        else
        {
            fin.GetComponent<TextMeshProUGUI>().text = "당신은 패배자입니다...";
        }
    }

    void ClearTextField()
    {
        for (int i = gameplayTextField.childCount - 1; i >= 0; i--)
        {
            Destroy(gameplayTextField.GetChild(i).gameObject);
        }
    }

    Character MakeMonster(Dungeon_SO dungeon, float atkMul, float hpMul)
    {
        float hp = dungeon.monsterBaseHP * (1 + Random.Range(-randomRangePercent, randomRangePercent)) * hpMul;
        var monster = new Character()
        {
            charName = monsterNames[Random.Range(0, monsterNames.Count)],
            baseATK = dungeon.monsterBaseATK * (1 + Random.Range(-randomRangePercent, randomRangePercent)) * atkMul,
            baseDEF = 0,
            baseMaxHP = hp,
            hp = hp,
        };

        return monster;
    }

    bool Fight(ref Character runner, Character monster)
    {
        for (int i = 0; i < 100; i++)
        {
            float currentMonsterHP = monster.hp;
            monster.hp -= Mathf.Max(1, runner.baseATK - monster.baseDEF);
            string log = $"{runner.charName}이(가) 공격했다! {monster.charName}에게 {Mathf.RoundToInt(currentMonsterHP - monster.hp)}의 피해를 주었다!";

            GameObject newText = Instantiate(prefab_gameplayText, gameplayTextField);
            newText.GetComponent<TextMeshProUGUI>().text = log;
            if (monster.hp <= 0)
            {
                return true;
            }

            float currentRunnerHP = runner.hp;
            runner.hp -= Mathf.Max(1, monster.baseATK - runner.baseDEF);
            string log2 = $"{monster.charName}이(가) 공격했다! {runner.charName}에게 {Mathf.RoundToInt(currentRunnerHP - runner.hp)}의 피해를 주었다!";

            GameObject newText2 = Instantiate(prefab_gameplayText, gameplayTextField);
            newText2.GetComponent<TextMeshProUGUI>().text = log2;
            if (runner.hp <= 0)
            {
                return false;
            }
        }

        return false;
    }
}
