using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject defeatPanel;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] UnityEngine.Audio.AudioMixer audioMixer;
    [SerializeField] private LevelSO currentLevelInfo;
    [SerializeField] private Transform[] spawnPoints; 
  
    private IEventBus _eventBus;
    private int enemiesRemaining = 0;
    
    private void Start()
    {
        Application.wantsToQuit += WantsToQuit;

        _eventBus = ServiceLocator.GetService<IEventBus>();

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
        _eventBus.Subscribe<EndGameEvent>(OnGameEnd);
        _eventBus.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
        _eventBus.Subscribe<GameWonEvent>(OnGameWon);
        _eventBus.Subscribe<EnemyDeathEvent>(OnEnemyDeath);
        
        SpawnEnemies();
        
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
            _eventBus.Unsubscribe<EndGameEvent>(OnGameEnd);
            _eventBus.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
            _eventBus.Unsubscribe<GameWonEvent>(OnGameWon);
            _eventBus.Unsubscribe<EnemyDeathEvent>(OnEnemyDeath);
        }
        Application.wantsToQuit -= WantsToQuit;
    }

    private void SpawnEnemies()
    {
        foreach (EnemySpawnConfig config in currentLevelInfo.enemiesToSpawn)
        {
            enemiesRemaining += config.spawnCount;
            
            for (int i = 0; i < config.spawnCount; i++)
            {
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                
                Instantiate(config.enemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
            }
        }
    }
    
    private void OnEnemyDeath(EnemyDeathEvent eventData)
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            _eventBus.Publish(new GameWonEvent());
        }
    }
    
    private void OnPlayerDeath(PlayerDeathEvent eventData)
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2.5f);

        defeatPanel.SetActive(true);
        
        audioMixer.SetFloat("VFXVolume", -80f);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; 
    } 
    
    private void OnGameWon(GameWonEvent eventData)
    {
        victoryPanel.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; 
    }
    
    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        Debug.Log("OnBackToMenu called");
        Time.timeScale = 1f;
        audioMixer.SetFloat("VFXVolume", 0f);
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }

    public void OnGameEnd(EndGameEvent eventData)
    {
        Debug.Log("OnGameEnd called");

#if UNITY_EDITOR
        Time.timeScale = 1f;
        Debug.Log("quitting...");
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
#elif UNITY_WEBGL
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Menu");
#else
        SceneManager.LoadSceneAsync("Menu");
#endif
    }

    private bool WantsToQuit()
    {
        return true;
    }
}
