public class RangeAttack : AttackTypeBase
{
    protected override AttackTypeEnum AttackType => AttackTypeEnum.Range;

    public override void Attack(CardLogic target)
    {
        target.TakeDamage(_card.Health);
    }

    private void Start()
    {
        Init();
    }
}
