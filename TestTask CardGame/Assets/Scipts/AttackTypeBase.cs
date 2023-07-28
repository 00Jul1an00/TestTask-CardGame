using UnityEngine;

public abstract class AttackTypeBase : MonoBehaviour
{
    protected CardLogic _card;
    
    protected abstract AttackTypeEnum AttackType { get; }

    public string AttackTypeName { get => AttackType.ToString(); }

    public abstract void Attack(CardLogic target);

    protected void Init()
    {
        _card = GetComponent<CardLogic>();
    }
}
