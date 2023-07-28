using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : Effect
{
    [SerializeField] private int _multiplair;

    public override void TriggerEffect()
    {
        if (_triggerNumbers <= 0 && !IsInfitiny)
            return;

        if (_card.Health <= 0)
            return;

        _triggerNumbers--;
        _card.TakeHeal(_card.Health * _multiplair);
    }

    public override void Init(CardLogic card)
    {
        base.Init(card);
        _card.DamageTaken += OnDamageTaken;
    }

    private void OnDestroy()
    {
        _card.DamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken()
    {
        TriggerEffect();
    }
}
