using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MassHeal : Effect
{
    [SerializeField] private int _heal;

    private List<CardCell> _playerCells = new();

    public override void TriggerEffect()
    {
        if (_triggerNumbers <= 0 && !IsInfitiny)
            return;

        _triggerNumbers--;
        var notEmptyCells = _playerCells.Where(c => c.IsEmpty == false).ToList();

        foreach (var cell in notEmptyCells)
            cell.CurrentCard.TakeHeal(_heal);
    }


    public override void Init(CardLogic card)
    {
        base.Init(card);
        var cells = FindObjectsOfType<CardCell>().Where(c => c.Membership == Membership);

        foreach (var cell in cells)
        {
            _playerCells.Add(cell);
            cell.CardChanged += OnCardChanged;
        }
    }

    private void OnDestroy()
    {
        foreach (var cell in _playerCells)
            cell.CardChanged -= OnCardChanged;
    }

    private void OnCardChanged(CardLogic card)
    {
        if (card.Stats.Effect == this)
            TriggerEffect();
    }
}
