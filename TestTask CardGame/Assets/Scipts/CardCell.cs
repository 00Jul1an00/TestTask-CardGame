using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class CardCell : MonoBehaviour
{
    [SerializeField] private Membership _membership;
    [SerializeField] private Deck _playingDeck;
    [SerializeField] private Deck _startDeck;
    [SerializeField] private SelectCardPanel _selectCardPanel;

    private CardLogic _currentCard;
    private CardCellUI _cellUI;

    public Membership Membership => _membership;
    public CardLogic CurrentCard => _currentCard;
    public bool IsEmpty { get { return _currentCard == null; } }
    public event Action<CardLogic> CardSelected;
    public event Action<CardLogic> CardChanged;

    private void Start()
    {
        _cellUI = GetComponent<CardCellUI>();
        _cellUI.CellButtonClicked += OnCellButtonClicked;
        PickStartRandomCard();
    }

    private void OnDisable()
    {
        _cellUI.CellButtonClicked -= OnCellButtonClicked;
    }

    private void OnCellButtonClicked()
    {
        if (_membership == Membership.Enemy || !GameManager.Instance.IsPlayerTurn)
            return;

        _selectCardPanel.gameObject.SetActive(true);
        _selectCardPanel.SetActivatedCell(this);
    }

    private void OnCardButtonClicked(CardLogic card)
    {
        CardSelected?.Invoke(card);
    }

    private void PickStartRandomCard()
    {
        List<CardLogic> startDeck = new();

        for (int i = 0; i < _startDeck.Cards.Count; i++)
        {
            for (int j = 0; j < _playingDeck.Cards.Count; j++)
            {
                if (_startDeck.Cards[i].Stats.Name == _playingDeck.Cards[j].Stats.Name)
                {
                    startDeck.Add(_playingDeck.Cards[j]);
                    break;
                }
            }
        }

        _startDeck.ChangeDeck(startDeck);
        CardLogic card = null;

        while (card == null)
        {
            int rand = UnityEngine.Random.Range(0, _startDeck.Cards.Count);
            card = _startDeck.Cards[rand];

            if (card.IsUsedForStartDeck)
                card = null;
            else
                card.IsUsedForStartDeck = true;
        }

        SelectCard(card);
        _playingDeck.RemoveCardFromDeck(_currentCard);
    }

    public void ChangeCurrentCard(CardLogic card)
    {
        if (card == null)
            throw new NullReferenceException();

        SelectCard(card);
        CardChanged?.Invoke(card);
    }

    private void SelectCard(CardLogic card)
    {
        _currentCard = card;
        _cellUI.DisplayCard(_currentCard);
        _currentCard.CardUI.CardButtonClicked += OnCardButtonClicked;
        _currentCard.Die += RemoveCard;
        _currentCard.IsSelected = true;
    }

    private void RemoveCard()
    {
        if (_currentCard == null)
            return;

        _currentCard.Die -= RemoveCard;
        _playingDeck.RemoveCardFromDeck(_currentCard);
        Destroy(_currentCard.gameObject);
        _currentCard = null;
    }
}
