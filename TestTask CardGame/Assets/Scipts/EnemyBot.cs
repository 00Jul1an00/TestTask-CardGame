using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyBot : MonoBehaviour
{
    [SerializeField] private List<CardCell> _botCells;
    [SerializeField] private List<CardCell> _playerCells;
    [SerializeField] private Deck _botDeck;
    [SerializeField] private Player _player;
    [SerializeField] private Deck _playerDeck;
    [SerializeField] private Timer _timer;

    public event Action BotTurnEnd;
    public event Action<CardLogic> CardAttacker;

    private void OnEnable()
    {
        _player.TurnEnded += OnTurnEnded;
    }

    private void OnDisable()
    {
        _player.TurnEnded -= OnTurnEnded;
    }

    private void OnTurnEnded()
    {
        _timer.RestartTimer();
        BotTurn();
    }

    private void BotTurn()
    {
        do
        {
            SetBotCards();
        }
        while (!CanAttack());

        var playerNotEmptyCells = _playerCells.Where(c => c.IsEmpty == false).ToList();
        var botNotEmptyCells = _botCells.Where(c => c.IsEmpty == false).ToList();

        if (playerNotEmptyCells.Count == 0 || botNotEmptyCells.Count == 0)
        {
            BotTurnEnd?.Invoke();
            return;
        }

        int rand = UnityEngine.Random.Range(0, playerNotEmptyCells.Count);
        CardCell cellToAttack = playerNotEmptyCells[rand];
        rand = UnityEngine.Random.Range(0, botNotEmptyCells.Count);
        CardCell cellFromAttack = botNotEmptyCells[rand];
        CardLogic playerCard = cellToAttack.CurrentCard;
        CardLogic botCard = cellFromAttack.CurrentCard;

        if (!CanAttackOnTarget(playerCard, botCard, playerNotEmptyCells))
        {
            for (int i = 0; i < botNotEmptyCells.Count; i++)
            {
                bool isFoundTarget = false;
                botCard = botNotEmptyCells[i].CurrentCard;

                for (int j = 0; j < playerNotEmptyCells.Count; j++)
                {
                    playerCard = playerNotEmptyCells[j].CurrentCard;

                    if (CanAttackOnTarget(playerCard, botCard, playerNotEmptyCells))
                    {
                        isFoundTarget = true;
                        break;
                    }
                }

                if (isFoundTarget)
                    break;
            }
        }

        Attack(playerCard, botCard);
        print(botCard.Stats.Name + " On cardCell " + rand + " deal " + botCard.HealthBeforeAttack + " damage to " + playerCard.Stats.Name + " and left him" + playerCard.Health + " HP, " + botCard.Health + " bot card HP");
        CardAttacker?.Invoke(botCard);

        if (IsAllCellsEmpty() && _botDeck.Cards.Count > 0)
        {
            int prevRand = rand;
            int r = UnityEngine.Random.Range(0, _botDeck.Cards.Count);
            _botCells[prevRand].ChangeCurrentCard(_botDeck.Cards[r]);
        }

        BotTurnEnd?.Invoke();
        GameManager.Instance.IsPlayerTurn = true;
    }

    private static void Attack(CardLogic playerCard, CardLogic botCard)
    {
        if (botCard.HasAttackType)
        {
            botCard.Attack(playerCard);
        }
        else
        {
            playerCard.TakeDamage(botCard.Health);
            botCard.TakeDamage(playerCard.HealthBeforeAttack);
        }
    }

    private bool CanAttackOnTarget(CardLogic playerCard, CardLogic botCard, List<CardCell> notEmptyCells)
    {
        foreach (var cell in notEmptyCells)
            if (cell.CurrentCard.TargetType is ProvocationTarget && playerCard != cell.CurrentCard)
                if (botCard.TargetType is not FlyingTarget)
                    return false;

        if (playerCard.TargetType is FlyingTarget && (botCard.AttackType is not RangeAttack || botCard.TargetType is FlyingTarget))
            if (notEmptyCells.TrueForAll(c => c.CurrentCard.TargetType is not FlyingTarget))
                return false;

        return true;
    }

    private bool CanAttack()
    {
        if (_botDeck.Cards.Count >= _botCells.Count)
        {
            foreach (var cell in _botCells)
                if (cell.IsEmpty)
                    return false;
        }

        return true;
    }

    private void SetBotCards()
    {
        var cards = _botDeck.Cards.Where(c => c.IsSelected == false).ToList();
        int rand = UnityEngine.Random.Range(0, cards.Count);

        if (cards.Count == 0)
            return;

        foreach (var cell in _botCells)
        {
            if (cell.IsEmpty)
                cell.ChangeCurrentCard(cards[rand]);
        }
    }

    private bool IsAllCellsEmpty()
    {
        foreach (var cell in _botCells)
        {
            if (!cell.IsEmpty)
                return false;
        }

        return true;
    }
}
