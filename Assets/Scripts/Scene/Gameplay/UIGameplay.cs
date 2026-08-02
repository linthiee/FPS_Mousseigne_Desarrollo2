using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EventBus;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelDefeat;
    [SerializeField] private GameObject panelVictory;

    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonExit;

    [SerializeField] private Button buttonSettingsExit;

    [SerializeField] private Button buttonDefeatExit;
    [SerializeField] private Button buttonRetry;
    [SerializeField] private Button buttonRetryPause;

    [SerializeField] private Button buttonRetryWin;
    [SerializeField] private Button buttonExitWin;
    
    [SerializeField] private MonoBehaviour cameraLookScript;
    
    private IEventBus _eventBus;

    private bool isPaused = false;

    private void Awake()
    {
        _eventBus = ServiceLocator.GetService<IEventBus>();

        buttonSettings.onClick.AddListener(OnSettingsClicked);
        buttonExit.onClick.AddListener(OnExitClicked);

        buttonSettingsExit.onClick.AddListener(OnBackFromSettingsClicked);

        buttonDefeatExit.onClick.AddListener(OnExitClicked);
        buttonRetry.onClick.AddListener(OnRetryClicked);
        buttonRetryPause.onClick.AddListener(OnRetryClicked);

        buttonRetryWin.onClick.AddListener(OnRetryClicked);
        buttonExitWin.onClick.AddListener(OnExitClicked);

        _eventBus.Subscribe<TakeDmgEvent>(OnTakeDmg);

        panelPause.SetActive(false);
        panelSettings.SetActive(false);
        panelDefeat.SetActive(false);
        panelVictory.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private void OnDestroy()
    {
        buttonSettings.onClick.RemoveAllListeners();
        buttonExit.onClick.RemoveAllListeners();
        buttonSettingsExit.onClick.RemoveAllListeners();
        buttonDefeatExit.onClick.RemoveAllListeners();
        buttonRetry.onClick.RemoveAllListeners();
        buttonRetryPause.onClick.RemoveAllListeners();
        buttonRetryWin.onClick.RemoveAllListeners();
        buttonExitWin.onClick.RemoveAllListeners();
        _eventBus.Unsubscribe<TakeDmgEvent>(OnTakeDmg);
    }

    private void OnTakeDmg(TakeDmgEvent eventData)
    {
        bool hasFound = false;
    }


    private void TogglePause()
    {
        if (panelDefeat.gameObject.activeInHierarchy || panelVictory.gameObject.activeInHierarchy)
            return;
        
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        panelPause.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        if (cameraLookScript != null)
        {
            cameraLookScript.enabled = !isPaused;
        }
        
        if (!isPaused)
        {
            panelSettings.SetActive(false);
        }
    }

    private void OnRetryClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        panelDefeat.SetActive(false);
    }

    private void OnBackFromSettingsClicked()
    {
        panelSettings.SetActive(false);
        panelPause.SetActive(true);
    }

    private void OnExitClicked()
    {
        if (isPaused || panelDefeat.gameObject.activeInHierarchy || panelVictory.gameObject.activeInHierarchy)
        {
            Time.timeScale = 1.0f;

            _eventBus.Publish(new EndGameEvent());
            _eventBus.Publish(new ExitToMenuEvent());
        }
    }

    private void OnSettingsClicked()
    {
        if (isPaused)
        {
            panelPause.SetActive(false);
            panelSettings.SetActive(true);
        }
    }
}