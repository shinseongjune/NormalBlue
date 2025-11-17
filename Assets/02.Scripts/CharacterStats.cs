using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHP,
    ATK,
    DEF,
}

public enum StatModType
{
    ADD,
    MUL,
}

[Serializable]
public class StatMod
{
    public StatType statType;
    public StatModType modType;
    public float value;
}

public class Stat
{
    public StatType Type { get; }

    private float _baseValue;
    public float BaseValue
    {
        get => _baseValue;
        set { if (!Mathf.Approximately(_baseValue, value)) { _baseValue = value; RecomputeAndNotify(); } }
    }

    private float _valueCache;
    private bool _isDirty = true;

    // 값 변경 이벤트: (oldValue, newValue)
    public event Action<float, float> OnValueChanged;

    private readonly Dictionary<Item_SO, List<StatMod>> _mods = new();

    public Stat(StatType type, float baseValue = 0f)
    {
        Type = type;
        _baseValue = baseValue;
        _valueCache = baseValue;
        _isDirty = true;
    }

    public float Value
    {
        get
        {
            if (_isDirty) RecomputeAndNotify(); // 게터에서도 필요 시 재계산
            return _valueCache;
        }
    }

    public void ApplyMod(Item_SO item)
    {
        if (item == null || item.mods == null || item.mods.Count == 0) return;

        if (!_mods.TryGetValue(item, out var list) || list == null)
        {
            list = new List<StatMod>(item.mods.Count);
            _mods[item] = list;
        }

        // 해당 StatType만 복사 추가 (공유 참조 방지)
        foreach (var m in item.mods)
        {
            if (m == null) continue;
            if (m.statType != Type) continue;
            list.Add(new StatMod { statType = m.statType, modType = m.modType, value = m.value });
        }

        RecomputeAndNotify();
    }

    public void RemoveMod(Item_SO item)
    {
        if (item == null) return;
        if (_mods.Remove(item))
            RecomputeAndNotify();
    }

    public void ClearAllMods()
    {
        if (_mods.Count == 0) return;
        _mods.Clear();
        RecomputeAndNotify();
    }

    // --- 내부 도우미 ---
    private float ComputeValue()
    {
        float sum = _baseValue;
        float mul = 1f;

        foreach (var kv in _mods)
        {
            var list = kv.Value;
            for (int i = 0; i < list.Count; i++)
            {
                var mod = list[i];
                if (mod.modType == StatModType.ADD) sum += mod.value;
                else if (mod.modType == StatModType.MUL) mul += mod.value;
            }
        }
        return sum * mul;
    }

    private void RecomputeAndNotify()
    {
        float oldVal = _valueCache;
        float newVal = ComputeValue();
        _valueCache = newVal;
        _isDirty = false;

        if (!Mathf.Approximately(oldVal, newVal))
            OnValueChanged?.Invoke(oldVal, newVal);
    }
}

public sealed class CharacterStats : MonoBehaviour
{
    [SerializeField] private Character_SO data;

    // 런타임 스탯
    public Stat MaxHP { get; private set; }
    public Stat ATK { get; private set; }
    public Stat DEF { get; private set; }

    // 현재 체력
    public float CurrentHP { get; private set; }

    private void Awake()
    {
        float baseMax = data ? data.baseMaxHP : 100f;

        MaxHP = new Stat(StatType.MaxHP, baseMax);
        ATK = new Stat(StatType.ATK, data ? data.baseATK : 10f);
        DEF = new Stat(StatType.DEF, data ? data.baseDEF : 5f);

        // 최초 HP는 풀피로 시작(원하면 퍼센트 유지 시작 등으로 바꿔도 됨)
        CurrentHP = MaxHP.Value;

        // MaxHP 변경 시 현재 HP 조정
        MaxHP.OnValueChanged += HandleMaxHPChanged;
    }

    private void OnDestroy()
    {
        if (MaxHP != null) MaxHP.OnValueChanged -= HandleMaxHPChanged;
    }

    private void HandleMaxHPChanged(float oldMax, float newMax)
    {
        float delta = newMax - oldMax;
        if (delta > 0f)
        {
            // 증가: 증가분만큼 회복
            CurrentHP += delta;
        }
        else if (delta < 0f)
        {
            // 감소: 상한 클램프
            if (CurrentHP > newMax) CurrentHP = newMax;
        }

        // 하한/상한 보정
        if (CurrentHP < 0f) CurrentHP = 0f;
        if (CurrentHP > newMax) CurrentHP = newMax;
    }

    // 장비 장착/해제 — 해당 스탯들에 모드 적용
    public void Equip(Item_SO item)
    {
        if (item == null) return;
        MaxHP.ApplyMod(item);
        ATK.ApplyMod(item);
        DEF.ApplyMod(item);
        // MaxHP가 바뀌면 이벤트를 통해 CurrentHP가 자동 조정됨
    }

    public void Unequip(Item_SO item)
    {
        if (item == null) return;
        MaxHP.RemoveMod(item);
        ATK.RemoveMod(item);
        DEF.RemoveMod(item);
    }

    // 데미지/회복 유틸
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;
        CurrentHP = Mathf.Max(0f, CurrentHP - amount);
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        CurrentHP = Mathf.Min(MaxHP.Value, CurrentHP + amount);
    }
}