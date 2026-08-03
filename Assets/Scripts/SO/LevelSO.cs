using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct EnemySpawnConfig
{
    public GameObject enemyPrefab;
    
    public int spawnCount;
}

[CreateAssetMenu(fileName = "LevelSO", menuName = "FPS/Level Stats")]
public class LevelSO : ScriptableObject
{
    public List<EnemySpawnConfig> enemiesToSpawn;
}