using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;

    private IEventBus _eventBus;
    private RaycastHit hit;

   private int maxBullets = 17;
   private int currentBullets;
    
    void Awake()
    {
        _eventBus = ServiceLocator.GetService<IEventBus>();

        _eventBus.Subscribe<PlayerShootEvent>(OnPlayerShoot);
        _eventBus.Subscribe<PlayerRechargeEvent>(OnPlayerRecharge);
    }

    void Start()
    {
        currentBullets = maxBullets;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<PlayerShootEvent>(OnPlayerShoot);
        _eventBus.Unsubscribe<PlayerRechargeEvent>(OnPlayerRecharge);
    }

    private void OnPlayerShoot(PlayerShootEvent eventData)
    {
        if (currentBullets > 0)
        {
            Debug.Log(currentBullets);
            
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerCamera.farClipPlane);
            currentBullets--;

            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1000f, Color.tomato, 2.0f, false);
        }
    }

    private void OnPlayerRecharge(PlayerRechargeEvent eventData)
    {
        if (currentBullets < maxBullets)
        {
            currentBullets = maxBullets;
            Debug.Log("recargando");
        }
        else
            Debug.Log("capped");
    }
}