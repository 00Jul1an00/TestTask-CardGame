using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Heal : Effect
{
    [SerializeField] private int _heal;
    private EnemyBot _enemy;
    private Player _player;

    private List<CardCell> _playerCells = new();

    public override void TriggerEffect()
    {
        if (_triggerNumbers <= 0 && !IsInfitiny)
            return;

        _triggerNumbers--;
        var notEmptyCells = _playerCells.Where(c => c.IsEmpty == false).ToList();
        int rand = Random.Range(0, notEmptyCells.Count);
        CardLogic card = notEmptyCells[rand].CurrentCard;
        card.TakeHeal(_heal);
    }

    
    public override void Init(CardLogic card)
    {
        base.Init(card);
        _enemy = FindObjectOfType<EnemyBot>();
        _player = FindObjectOfType<Player>();
        var cells = FindObjectsOfType<CardCell>();

        foreach (var cell in cells)
            if (cell.Membership == Membership)
                _playerCells.Add(cell);

        if(Membership == Membership.Player)
            _enemy.BotTurnEnd += TriggerEffect;
        else
            _player.TurnEnded += TriggerEffect;
    }

    private void OnDestroy()
    {
        if (Membership == Membership.Player)
            _enemy.BotTurnEnd -= TriggerEffect;
        else
            _player.TurnEnded -= TriggerEffect;
    }
}
