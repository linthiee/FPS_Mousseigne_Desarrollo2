using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;

    private IEventBus _eventBus;
    private RaycastHit hit;

    private int maxBullets = 17;
    private int currentBullets;

    private bool canShoot = false;
    private bool wantsToShoot = false;

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

    void FixedUpdate()
    {
        Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 10f, Color.red);
        
        if (canShoot && wantsToShoot)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit,
                    Mathf.Infinity, LayerMask.GetMask("Wall")))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance,
                    Color.yellow);
                Debug.Log("Wall!");
            }
            else
            {
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1000f, Color.tomato,
                    2.0f, false);
            }

            wantsToShoot = false;
        }
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
            currentBullets--;
            canShoot = true;
            wantsToShoot = true;
        }
        else
        {
            canShoot = false;
            wantsToShoot = true;
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