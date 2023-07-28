using UnityEngine;

public abstract class TargetTypeBase : MonoBehaviour
{
    protected abstract TargetTypeEnum _targetTypeEnum { get; }

    public string TargetTypeName { get => _targetTypeEnum.ToString(); }

    protected CardLogic _card;

    public abstract bool TargetCondition(CardLogic attackerCard);


    protected void Init()
    {
        _card = GetComponent<CardLogic>();
    }
}
