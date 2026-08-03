using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "FPS/Enemy Stats")]
public class EnemySO : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;
    public float stoppingDistance;
    public float range;

    [Header("Combat")]
    public float damage;
    public float attackRange;
    public float attackCooldown;
    
    [Header("Health")]
    public int maxHealth;
    
    [Header("Audio")]
    public AudioClip walk;
    public AudioClip attack;
    public AudioClip hit;
    public AudioClip dead;
}
