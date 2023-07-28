using UnityEngine;
using System;

public class CardLogic : MonoBehaviour
{
    private CardUI _cardUI;
    private CardSO _cardSO;
    private int _health;
    private AttackTypeBase _attackType;
    private TargetTypeBase _targetType;
    private Effect _effect;

    public CardUI CardUI => _cardUI;
    public int Health => _health;
    public int HealthBeforeAttack { get; private set; }
    public CardSO Stats => _cardSO;

    public TargetTypeBase TargetType => _targetType;
    public AttackTypeBase AttackType => _attackType;
    public Effect Effect => _effect;
    public bool HasAttackType => _attackType != null;
    public bool HasTargetType => _targetType != null;
    public bool HasEffect;

    [HideInInspector] public bool IsUsedForStartDeck;
    [HideInInspector] public bool IsSelected;

    public event Action DamageTaken;
    public event Action HealTaken;
    public event Action Die;
    public event Action StatsChanged;

    private void OnEnable()
    {
        _cardUI = GetComponent<CardUI>();
    }

    public void ChangeStats(CardSO cardSO)
    {
        if (cardSO == null)
            throw new System.NullReferenceException();

        _cardSO = cardSO;
        ChangeCardStats();
        ValidateCard();
    }

    private void ChangeCardStats()
    {
        _health = _cardSO.Health;
        StatsChanged?.Invoke();
    }

    public void Attack(CardLogic target)
    {
        if(_attackType != null)
            _attackType.Attack(target);
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            return;

        HealthBeforeAttack = _health;
        _health -= damage;
        DamageTaken?.Invoke();

        if (_health <= 0)
            Die?.Invoke();
    }

    public void TakeHeal(int heal)
    {
        if (heal < 0)
            return;

        _health += heal;
        HealTaken?.Invoke();
    }

    private void ValidateCard()
    {
        if (Stats.AttackType == AttackTypeEnum.Range)
        {
            _attackType = gameObject.AddComponent<RangeAttack>();
        }
        else if (Stats.AttackType == AttackTypeEnum.Fast)
        {
            _attackType = gameObject.AddComponent<FastAttack>();
        }

        if (Stats.TargetType == TargetTypeEnum.Provocation)
        {
            _targetType = gameObject.AddComponent<ProvocationTarget>();
        }
        else if (Stats.TargetType == TargetTypeEnum.Flying)
        {
            _targetType = gameObject.AddComponent<FlyingTarget>();
        }

        ValidateEffect();
    }

    private void ValidateEffect()
    {
        if (Stats.Effect != null)
        {
            _effect = Stats.Effect;
            _effect.SetMembership(GetParentMembership());
            _effect.Init(this);
        }
    }

    private Membership GetParentMembership()
    {
        Transform parent = transform.parent;

        if (parent.gameObject.GetComponent<Deck>() != null)
            return parent.gameObject.GetComponent<Deck>().Membership;
        else if (parent.gameObject.GetComponent<CardCell>() != null)
            return parent.gameObject.GetComponent<CardCell>().Membership;
        else return Membership.Player;
    }
}
