using UnityEngine;
using UnityEngine.SceneManagement;
using static EventBus;

public class GameMenuManager : MonoBehaviour
{
    private IEventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.GetService<IEventBus>();

        _eventBus.Subscribe<PlayGameEvent>(OnPlayClicked);

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<PlayGameEvent>(OnPlayClicked);
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
        }
    }

    private void OnPlayClicked(PlayGameEvent eventData)
    {
        SceneManager.LoadScene("FPS");
    }

    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        SceneManager.LoadScene("MainMenu");
    }
}