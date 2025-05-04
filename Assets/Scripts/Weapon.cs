using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    // bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;

    //private Animator animator;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public enum WeaponModel
    {
        Pistol1911,
        AK74
    }
    public WeaponModel thisWeaponModel;
    public enum ShootingMode
    {
        Single,
        Bursst,
        Auto
    }

    public ShootingMode currentShootingMode;
    [Header("Animation")]
    public Animator soldierAnimator;
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        //animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSound1911.Play();
        }
        if (currentShootingMode == ShootingMode.Auto)
        {
            //holding down left mouse button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Bursst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
        {
            Reload();
        }
        if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
        {
            Reload();
        }
        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        if (soldierAnimator != null)
            soldierAnimator.SetTrigger("Shoot");
        //animator.SetTrigger("RECOIL");

        //SoundManager.Instance.shootingSound1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        //pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
        ///shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        //destroy the bullet after ssome time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        //Burst Mode
        if (currentShootingMode == ShootingMode.Bursst && burstBulletsLeft > 1)
        {//we already shoot one before this check
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        //SoundManager.Instance.reloadSound1911.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        //animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        //shooting from the middle of the screen to check whre are we pointing at
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //hitting something
            targetPoint = hit.point;
        }
        else
        {
            //shooting at the air
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //return the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
