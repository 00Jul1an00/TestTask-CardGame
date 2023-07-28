using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    [SerializeField] private Membership _membership;
    [SerializeField] private List<CardSO> _cardsSo;
    [SerializeField] private CardLogic _cardPrefab;

    public List<CardLogic> Cards { get; private set; } = new();
    private bool _isCleaned;
    public Membership Membership => _membership;

    protected void Awake()
    {
        foreach(var cardSo in _cardsSo)
        {
            CardLogic spawned = Instantiate(_cardPrefab, transform);
            spawned.ChangeStats(cardSo);
            spawned.gameObject.SetActive(false);
            Cards.Add(spawned);
        }
    }

    public void ChangeDeck(List<CardLogic> cards)
    {
        if(!_isCleaned)
            CleanDeck();
        
        Cards = cards;
    }

    public void RemoveCardFromDeck(CardLogic card)
    {
        Cards.Remove(card);
    }

    private void CleanDeck()
    {
        for (int i = 0; i < Cards.Count; i++)
            Destroy(Cards[i].gameObject);

        Cards.Clear();
        _isCleaned = true;
    }
}