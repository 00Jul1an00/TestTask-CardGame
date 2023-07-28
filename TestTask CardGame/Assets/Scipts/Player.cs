using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    [SerializeField] private List<CardCell> _playerCardCells;
    [SerializeField] private List<CardCell> _enemyCardCells;
    [SerializeField] private Deck _playerDeck;
    [SerializeField] private Timer _timer;

    private CardLogic _selectedPlayerCard;

    public event Action TurnEnded;

    private void OnEnable()
    {
        _playerCardCells.ForEach(cell => cell.CardSelected += PlayerCardSelected);
        _enemyCardCells.ForEach(cell => cell.CardSelected += EnemyCardSelected);
        _timer.TimeEnd += OnTimeEnd;

    }

    private void OnDisable()
    {
        _playerCardCells.ForEach(cell => cell.CardSelected -= PlayerCardSelected);
        _enemyCardCells.ForEach(cell => cell.CardSelected -= EnemyCardSelected);
        _timer.TimeEnd -= OnTimeEnd;
    }

    private void PlayerCardSelected(CardLogic playerCard)
    {
        _selectedPlayerCard = playerCard;
        print("Card Selected");
    }

    private void EnemyCardSelected(CardLogic enemyCard)
    {
        if (_selectedPlayerCard == null)
            return;

        print("Enemy Card Selected");

        if (CanAttack())
        {
            if (!CanAttackOnTarget(enemyCard))
            {
                print("cant attack " + enemyCard.Stats.Name);
                return;
            }

            Attack(enemyCard);
            GameManager.Instance.IsPlayerTurn = false;
            TurnEnded?.Invoke();
        }

        _selectedPlayerCard = null;
    }

    private void Attack(CardLogic enemyCard)
    {
        if (_selectedPlayerCard.HasAttackType)
        {
            _selectedPlayerCard.Attack(enemyCard);
        }
        else
        {
            enemyCard.TakeDamage(_selectedPlayerCard.Health);
            _selectedPlayerCard.TakeDamage(enemyCard.HealthBeforeAttack);
        }
    }

    private bool CanAttackOnTarget(CardLogic enemyCard)
    {
        var notEmptyEnemyCells = _enemyCardCells.Where(c => c.IsEmpty == false).ToList();

        foreach (var cell in notEmptyEnemyCells)
            if (cell.CurrentCard.TargetType is ProvocationTarget && enemyCard != cell.CurrentCard)
                if (_selectedPlayerCard.TargetType is not FlyingTarget)
                    return false;


        if (enemyCard.TargetType is FlyingTarget && (_selectedPlayerCard.AttackType is not RangeAttack || _selectedPlayerCard.TargetType is FlyingTarget))
            if (notEmptyEnemyCells.TrueForAll(c => c.CurrentCard.TargetType is not FlyingTarget))
                return false;

        return true;
    }

    private bool CanAttack()
    {
        if (_playerDeck.Cards.Count >= _playerCardCells.Count)
        {
            foreach (var cell in _playerCardCells)
                if (cell.IsEmpty)
                    return false;
        }

        return true;
    }

    private void OnTimeEnd() => TurnEnded?.Invoke();
}
