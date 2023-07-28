using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastAttack : AttackTypeBase
{
    protected override AttackTypeEnum AttackType => AttackTypeEnum.Fast;

    public override void Attack(CardLogic target)
    {
        if (target == null)
            return;

        target.TakeDamage(_card.Health);

        if (target.Health <= 0)
            return;

        _card.TakeDamage(target.Health);
    }

    private void Start()
    {
        Init();
    }
}
