using UnityEngine;

public enum TriggerTime
{
    Spawm,
    StartTurn,
    PlayerAttack,
    EmemyAttack,
    OnDamage,
    OnDie
}

public enum Position
{
    CurrentCard,
    AttackedCard,
    RandomCard,
    AllCars
}

public enum ChangeHealthModificator
{
    Plus,
    Minus,
    Multiply,
    Divide
}


public abstract class Effect : MonoBehaviour
{
    [SerializeField] protected string _name;
    [SerializeField] protected bool _isInfitiny;
    [SerializeField] protected int _triggerNumbers;
    [SerializeField] protected TriggerTime _triggerTime;
    [SerializeField] protected Position _position;
    [SerializeField] protected ChangeHealthModificator _changeHealthModificator;
    [SerializeField] protected Membership _membership;

    protected CardLogic _card;

    public string Name => _name;
    public bool IsInfitiny => _isInfitiny;
    public int TriggerNumbers => _triggerNumbers;
    public TriggerTime TriggerTime => _triggerTime;
    public Position Position => _position;
    public ChangeHealthModificator ChangeHealthModificator => _changeHealthModificator;
    public Membership Membership => _membership;

    public virtual void Init(CardLogic card)
    {
        _card = card; 
    }

    public void SetMembership(Membership membership)
    {
        _membership = membership;
    }

    public abstract void TriggerEffect();
}
