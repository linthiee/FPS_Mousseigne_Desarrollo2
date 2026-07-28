using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IEventBus _eventBus;
    
    public void Awake()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();
    }

    public void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
