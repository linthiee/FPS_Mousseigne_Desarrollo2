using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "FPS/Enemy Stats")]
public class EnemySO : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;
    public float stoppingDistance;

    [Header("Combat")]
    public float damage;
    public float attackRange;
    public float attackCooldown;
}
