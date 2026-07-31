using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "FPS/Level Info")]
public class LevelSO : ScriptableObject
{
    public int meleeEnemyCount;
    public int sniperEnemyCount;
    public int fastShooterEnemyCount;
}