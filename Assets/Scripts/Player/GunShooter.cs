using System.Collections;
using TMPro;
using UnityEngine;
using Ursaanimation.CubicFarmAnimals;

public class GunShooter : MonoBehaviour
{
    [Header("Settings")]
    public float fireRate = 0.1f;
    public float recoilAmount = 2f;
    public float recoilSpeed = 10f;
    public int countBullet = 90; // Общий запас патронов

    [Header("References")]
    public AudioSource gunAudioSource;
    public AudioClip shotClip;
    public Transform cameraTransform;
    public ParticleSystem muzzleFlash;
    public GameObject bulletHolePrefab, bloodHolePreafab;

    [SerializeField] RectTransform crossFirePosition;
    [SerializeField] TextMeshProUGUI textAmmoDisplay;          // Общие
    [SerializeField] Animator animator;
    [SerializeField] int damageWeapon = 1;
    [SerializeField] AudioManager audioManager;

    private float nextFireTime;
    private int currentCountBulletWeapon;
    private int maxPatronWeapon = 30;
    
    bool isReloading = false;
    private void Start()
    {
       // currentCountBulletWeapon = maxPatronWeapon; // Заполнить магазин при старте
        UpdateUIWeapon();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentCountBulletWeapon > 0)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        } else if(Input.GetButton("Fire1") && currentCountBulletWeapon ==0 && Time.time >= nextFireTime)
        {
            
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartReloading();
        }
    }

    void Fire()
    {   
        if (currentCountBulletWeapon == 0) return;
        if (shotClip != null && gunAudioSource != null)
            gunAudioSource.PlayOneShot(shotClip);

        if (muzzleFlash != null)
            muzzleFlash.Play();
        if(currentCountBulletWeapon <= 0)
        {
            audioManager.PlaySounFx(1);
        }
        currentCountBulletWeapon--;
        RayCastGo();
        UpdateUIWeapon();
    }

    void RayCastGo()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, 400f))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(500f, hit.point, 5f);
            }

            if (bulletHolePrefab != null)
            {
                Quaternion rot = Quaternion.LookRotation(hit.normal);
                Vector3 pos = hit.point + hit.normal * 0.01f;
                if(hit.collider.tag == "Other")
                {
                    GameObject hole = Instantiate(bulletHolePrefab, pos, rot);
                    hole.transform.SetParent(hit.collider.transform);
                    Destroy(hole, 10f);
                }

                if(hit.collider.tag == "Animal")
                {   
                    AnimationController baran = hit.collider.GetComponent<AnimationController>();
                    if(baran != null)
                    {
                        baran.TakeDamage(damageWeapon);
                    }
                    
                    GameObject hole = Instantiate(bloodHolePreafab, pos, rot);
                    hole.transform.SetParent(hit.collider.transform);
                    Destroy(hole, 10f);
                }
                
               
            }
        }
    }

    
   public void StartReloading()
    {
        if (isReloading) return;

        int neededAmmo = maxPatronWeapon - currentCountBulletWeapon;
        if (neededAmmo > 0 && countBullet > 0)
        {
            isReloading = true;
            animator.SetBool("isReload", true);
            audioManager.PlaySounFx(0);
        }
    }
    public void FinishReload()
    {
        int neededAmmo = maxPatronWeapon - currentCountBulletWeapon;
        int ammoToReload = Mathf.Min(neededAmmo, countBullet);
        currentCountBulletWeapon += ammoToReload;
        countBullet -= ammoToReload;

        animator.SetBool("isReload", false);
        UpdateUIWeapon();
        isReloading = false;
    }

    public void UpdateUIWeapon()
    {
        textAmmoDisplay.text = $"{currentCountBulletWeapon} / {countBullet}";
    }

    public void AddAmmo(int amount)
    {
        countBullet += amount;
        UpdateUIWeapon();
    }
}
