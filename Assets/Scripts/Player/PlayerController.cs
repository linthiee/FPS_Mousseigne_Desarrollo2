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
        actions.Player.Shoot.canceled += DoNotShoot;
        
    }

    public void Awake()
    {
        _eventBus = ServiceLocator.GetService<IEventBus>();
        actions = new InputSystem_Actions();
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
