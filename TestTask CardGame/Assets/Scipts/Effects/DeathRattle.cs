using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRattle : Effect
{
    [SerializeField] private int _damage;

    private List<CardCell> _allCells = new();

    public override void TriggerEffect()
    {
        if (_triggerNumbers <= 0 && !IsInfitiny)
            return;

        _triggerNumbers--;
        foreach (var cell in _allCells)
        {
            if (!cell.IsEmpty)
                cell.CurrentCard.TakeDamage(_damage);
        }
    }

    public override void Init(CardLogic card)
    {
        base.Init(card);
        _card.Die += TriggerEffect;
        var cells = FindObjectsOfType<CardCell>();
        
        foreach(var cell in cells)
            _allCells.Add(cell);
    }

    private void OnDestroy()
    {
        _card.Die -= TriggerEffect;
    }
}
