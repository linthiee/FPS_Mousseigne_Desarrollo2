using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator gunAnimator;

    [Header("Weapon Stats")]
    [SerializeField] private int maxBullets = 17;
    [SerializeField] private float fireRate = 5f;

    [Header("Effects")] [SerializeField] private ParticleSystem muzzleFlash;

    private IEventBus _eventBus;
    private int currentBullets;

    private float nextTimeToFire = 0f;
    private bool isTriggerPulled = false;

    private readonly int fireHash = Animator.StringToHash("Fire");

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

    void Update()
    {
        Debug.DrawLine(playerCamera.transform.position,
            playerCamera.transform.position + playerCamera.transform.forward * 10f, Color.red);

        if (isTriggerPulled && currentBullets > 0 && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            ExecuteShoot();
        }
    }

    private void ExecuteShoot()
    {
        currentBullets--;
        Debug.Log(currentBullets);
        
        if (gunAnimator != null)
        {
            gunAnimator.CrossFade(fireHash, 0.05f, 0, 0f);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity,
                LayerMask.GetMask("Enemy")))
        {
            Enemy enemyHit = hit.collider.GetComponent<Enemy>();

            if (enemyHit != null)
            {
                enemyHit.TakeDamage(20);
                Debug.Log($"You hit {enemyHit.gameObject.name}!");
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance,
                Color.yellow, 2.0f);
        }
        else
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1000f, Color.tomato, 2.0f,
                false);
        }

        if (currentBullets <= 0)
        {
            isTriggerPulled = false;
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<PlayerShootEvent>(OnPlayerShoot);
        _eventBus.Unsubscribe<PlayerRechargeEvent>(OnPlayerRecharge);
    }

    private void OnPlayerShoot(PlayerShootEvent eventData)
    {
        isTriggerPulled = eventData.isShooting;
    }

    private void OnPlayerRecharge(PlayerRechargeEvent eventData)
    {
        if (currentBullets < maxBullets)
        {
            currentBullets = maxBullets;
        }
        else
        {
            Debug.Log("capped");
        }
    }
}