using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip breathing;
    [SerializeField] AudioClip death;
    [SerializeField] Camera playerCamera;

    private IEventBus _eventBus;
    private InputSystem_Actions actions;
    public float health = 100f;

    private bool isDead = false;

    private void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Shoot.performed += DoShoot;
        actions.Player.Recharge.performed += DoRecharge;
        actions.Player.Shoot.canceled += DoNotShoot;
    }

    public void Awake()
    {
        _eventBus = ServiceLocator.GetService<IEventBus>();
        actions = new InputSystem_Actions();
    }

    public void Start()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().PlayOneShot(breathing);
        }
    }

    private void OnDisable()
    {
        actions.Player.Shoot.performed -= DoShoot;
        actions.Player.Recharge.performed -= DoRecharge;
        actions.Player.Shoot.canceled -= DoNotShoot;

        actions.Player.Disable();
    }

    private void OnDestroy()
    {
        actions.Dispose();
    }

    public void TakeDamage(float amount)
    {
        if (!isDead)
        {
            health -= amount;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;

        actions.Player.Disable();

        audioSource.PlayOneShot(death);
        
        _eventBus.Publish(new PlayerDeathEvent());
    }

    private void DoShoot(InputAction.CallbackContext value)
    {
        Debug.Log("llamando a shoot event");
        _eventBus.Publish(new PlayerShootEvent(true));
    }

    private void DoRecharge(InputAction.CallbackContext value)
    {
        Debug.Log("llamando a recharge event");
        _eventBus.Publish(new PlayerRechargeEvent());
    }

    private void DoNotShoot(InputAction.CallbackContext value)
    {
        Debug.Log("not shooting");
        _eventBus.Publish(new PlayerShootEvent(false));
    }
}