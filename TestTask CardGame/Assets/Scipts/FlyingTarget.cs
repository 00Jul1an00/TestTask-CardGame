using UnityEngine;

public class FlyingTarget : TargetTypeBase
{
    protected override TargetTypeEnum _targetTypeEnum => TargetTypeEnum.Flying;

    public override bool TargetCondition(CardLogic attackerCard)
    {
        return true;
    }

    private void Start()
    {
        Init();
    }
}
