using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Parrying : Effect
{
    [SerializeField] private int _damage;

    private List<CardCell> _cells = new();

    public override void TriggerEffect()
    {
        if (_triggerNumbers <= 0 && !IsInfitiny)
            return;

        _triggerNumbers--;
        var notEmptyCells = _cells.Where(c => c.IsEmpty == false).ToList();
        int rand = Random.Range(0, notEmptyCells.Count);
        CardLogic card = notEmptyCells[rand].CurrentCard;
        card.TakeDamage(_damage);
        print("triggered");
    }

    public override void Init(CardLogic card)
    {
        base.Init(card);
        var cellsArr = FindObjectsOfType<CardCell>();
        
        foreach(var cell in cellsArr)
            if(cell.Membership != Membership)
                _cells.Add(cell);

        _card.DamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken()
    {
        TriggerEffect();
    }
}
