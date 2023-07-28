using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectCardPanel : MonoBehaviour
{
    [SerializeField] private Deck _deck;
    [SerializeField] private int _cardNumberToChoseFrom;
    [SerializeField] Transform _cardsHolder;

    private CardCell _activatedFromCell;
    private List<CardLogic> _shuffledCards = new();

    private void Start()
    {
        ShuffleCards();
    }

    private void OnEnable()
    {
        foreach (var card in _deck.Cards)
            card.CardUI.CardButtonClicked += OnCardSelected;
    }

    private void OnDisable()
    {
        foreach (var card in _deck.Cards) 
            card.CardUI.CardButtonClicked -= OnCardSelected;
    }

    private void OnCardSelected(CardLogic card)
    {
        ShuffleCards();
        _activatedFromCell.ChangeCurrentCard(card);
        gameObject.SetActive(false);
    }

    private void ShuffleCards()
    {
        _shuffledCards = new();
        var cardsInDeck = _deck.Cards.Where(c => c.IsSelected == false).ToList();

        for(int i = 0; i < cardsInDeck.Count && i < _cardNumberToChoseFrom; i++)
        {
            int rand = Random.Range(0, cardsInDeck.Count);
            var card = cardsInDeck[rand];

            while (_shuffledCards.Contains(card) && cardsInDeck.Count >= 3)
            {
                rand = Random.Range(0, cardsInDeck.Count);
                card = cardsInDeck[rand];
            }

            _shuffledCards.Add(card);
            card.transform.parent = _cardsHolder.transform;
            card.gameObject.SetActive(true);
        }
    }

    public void SetActivatedCell(CardCell cell)
    {
        if (cell == null)
            throw new System.NullReferenceException();

        _activatedFromCell = cell;
    }
}
