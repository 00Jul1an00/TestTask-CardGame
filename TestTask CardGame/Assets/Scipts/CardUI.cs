using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardUI : MonoBehaviour
{
    [SerializeField] private CardLogic _card;
    [SerializeField] private TMP_Text _cardNameText;
    [SerializeField] private TMP_Text _cardHealthText;
    [SerializeField] private TMP_Text _cardAttackTypeText;
    [SerializeField] private TMP_Text _cardTargetTypeText;
    [SerializeField] private TMP_Text _cardEffectText;
    [SerializeField] private Button _cardButton;
    public CardLogic Card => _card;

    public event Action<CardLogic> CardButtonClicked;

    private void Start()
    {
        DisplayStats();
    }

    private void OnEnable()
    {
        _cardButton.onClick.AddListener(OnButtonClick);
        _card.DamageTaken += OnDamageTaken;
        _card.HealTaken += OnHealTaken;
    }

    private void OnDisable()
    {
        _cardButton.onClick.RemoveListener(OnButtonClick);
        _card.DamageTaken -= OnDamageTaken;
        _card.HealTaken -= OnHealTaken;
    }

    private void DisplayStats()
    {
        _cardNameText.text = _card.Stats.Name;
        _cardHealthText.text = _card.Stats.Health.ToString();

        if (_card.Stats.AttackType != AttackTypeEnum.None)
            _cardAttackTypeText.text = _card.AttackType.AttackTypeName;
        else
            _cardAttackTypeText.text = String.Empty;

        if (_card.Stats.TargetType != TargetTypeEnum.None)
            _cardTargetTypeText.text = _card.TargetType.TargetTypeName;
        else
            _cardTargetTypeText.text = String.Empty;

        if (_card.Stats.Effect != null)
            _cardEffectText.text = _card.Stats.Effect.Name;
        else _cardEffectText.text = String.Empty;
    }

    private void OnDamageTaken() => _cardHealthText.text = _card.Health.ToString();

    private void OnHealTaken() => _cardHealthText.text = _card.Health.ToString();

    private void OnButtonClick()
    {
        if(GameManager.Instance.IsPlayerTurn)
            CardButtonClicked?.Invoke(_card);
    }
}
