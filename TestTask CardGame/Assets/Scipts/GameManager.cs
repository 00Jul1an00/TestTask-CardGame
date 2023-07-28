using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyBot _bot;
    [SerializeField] private Player _player;
    [SerializeField] private Deck _botDeck;
    [SerializeField] private Deck _playerDeck;
    [SerializeField] private List<CardCell> _playerCells;
    [SerializeField] private List<CardCell> _botCells;

    public bool IsPlayerTurn { get; set; }

    public static GameManager Instance { get; private set; }

    public event Action PlayerWon;
    public event Action BotWon;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            IsPlayerTurn = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _bot.BotTurnEnd += OnEndTurn;
        _player.TurnEnded += OnEndTurn;
    }

    private void OnDisable()
    {
        _bot.BotTurnEnd -= OnEndTurn;
        _player.TurnEnded -= OnEndTurn;
    }

    private void OnEndTurn()
    {
        if (_playerCells.TrueForAll(c => c.IsEmpty) && _playerDeck.Cards.Count == 0)
            BotWon?.Invoke();

        if (_botCells.TrueForAll(c => c.IsEmpty) && _botDeck.Cards.Count == 0)
            PlayerWon?.Invoke();
    }
}
