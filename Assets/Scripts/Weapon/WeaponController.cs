using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Camera playerCamera;

    [SerializeField] private Animator gunAnimator;

    [Header("Weapon Stats")] [SerializeField]
    private int maxBullets = 17;

    [SerializeField] private float fireRate = 5f;

    [Header("Effects")] [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip gunshot; 
    [SerializeField] private AudioClip reload; 
    [SerializeField] private AudioClip emptyMag; 

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

        if (isTriggerPulled && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (currentBullets > 0)
            {
                ExecuteShoot();
            }
            else
            {
                audioSource.PlayOneShot(emptyMag);
            }
        }
    }

    private void ExecuteShoot()
    {
        currentBullets--;
        Debug.Log(currentBullets);

        if (gunAnimator != null)
        {
            audioSource.PlayOneShot(gunshot);
            gunAnimator.CrossFade(fireHash, 0.05f, 0, 0f);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            muzzleFlash.Play(true);
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
        Debug.Log(isTriggerPulled);
    }

    private void OnPlayerRecharge(PlayerRechargeEvent eventData)
    {
        if (currentBullets < maxBullets)
        {
            audioSource.PlayOneShot(reload);
            currentBullets = maxBullets;
        }
        else
        {
            Debug.Log("capped");
        }
    }
}