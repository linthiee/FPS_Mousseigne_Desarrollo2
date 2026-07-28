using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private IEventBus _eventBus;

    public void Awake()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();
    }

    public void Start()
    {
        _eventBus.Subscribe<PlayerRechargeEvent>(OnPlayerRecharge);
    }

    public void Update()
    {
        
    }
    
    public void FixedUpdate()
    {
    }

    private void OnPlayerRecharge(PlayerRechargeEvent eventData)
    {
    }
}