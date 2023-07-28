using UnityEngine;

public class ProvocationTarget : TargetTypeBase
{
    protected override TargetTypeEnum _targetTypeEnum => TargetTypeEnum.Provocation;

    public override bool TargetCondition(CardLogic attackerCard)
    {
        return true;
    }
}
