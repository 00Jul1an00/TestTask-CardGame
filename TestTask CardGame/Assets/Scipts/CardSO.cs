using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable Objects/Card")]
public class CardSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private AttackTypeEnum _attackType;
    [SerializeField] private TargetTypeEnum _targetType;
    [SerializeField] private Effect _effect;

    public string Name => _name;
    public int Health => _health;
    public AttackTypeEnum AttackType => _attackType;
    public TargetTypeEnum TargetType => _targetType;
    public Effect Effect => _effect;
}
