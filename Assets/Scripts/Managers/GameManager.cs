using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static EventBus;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject defeatPanel;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] UnityEngine.Audio.AudioMixer audioMixer;
    
    private IEventBus _eventBus;
    
    private void Start()
    {
        Application.wantsToQuit += WantsToQuit;

        _eventBus = ServiceLocator.GetService<IEventBus>();

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
        _eventBus.Subscribe<EndGameEvent>(OnGameEnd);
        _eventBus.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
        _eventBus.Subscribe<GameWonEvent>(OnGameWon);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
            _eventBus.Unsubscribe<EndGameEvent>(OnGameEnd);
            _eventBus.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
            _eventBus.Unsubscribe<GameWonEvent>(OnGameWon);
        }
        Application.wantsToQuit -= WantsToQuit;
    }

    private void OnPlayerDeath(PlayerDeathEvent eventData)
    {
        Time.timeScale = 0f;
        audioMixer.SetFloat("VFXVolume", -80f);

        defeatPanel.SetActive(true);
    } 
    
    private void OnGameWon(GameWonEvent eventData)
    {
        Time.timeScale = 0f;
        victoryPanel.SetActive(true);
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
