using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private IEventBus _eventBus;
    private InputSystem_Actions actions;
    
    private void OnEnable()
    {
        actions.Player.Enable();
        
        actions.Player.Shoot.performed += DoShoot;
        actions.Player.Recharge.performed += DoRecharge;
    }

    public void Awake()
    {
        _eventBus = ServiceLocator.GetService<IEventBus>();
        actions = new InputSystem_Actions();
    }

    public void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnDisable()
    {
        actions.Player.Shoot.performed -= DoShoot;
        actions.Player.Recharge.performed -= DoRecharge;
        
        actions.Player.Disable();
    }

    private void OnDestroy()
    {
        actions.Dispose();
    }
    
    private void DoShoot(InputAction.CallbackContext value)
    {
        Debug.Log("llamando a shoot event");
        _eventBus.Publish(new PlayerShootEvent());
    }    
    private void DoRecharge(InputAction.CallbackContext value)
    {
        Debug.Log("llamando a recharge event");
        _eventBus.Publish(new PlayerRechargeEvent());
    }
}
