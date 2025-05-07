using UnityEngine;
using static Weapon;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource ShootingChanel;
    public AudioClip P1911Shot;
    public AudioClip AK74Shot;
    public AudioSource reloadSound1911;
    public AudioSource reloadSoundAK74;
    public AudioSource emptyMagazineSound1911;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            // Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChanel.PlayOneShot(P1911Shot);
                break;
            case WeaponModel.AK74:
                ShootingChanel.PlayOneShot(AK74Shot);
                break;
        }
    }
    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadSound1911.Play();
                break;
            case WeaponModel.AK74:
                reloadSoundAK74.Play();
                break;
        }
    }
}
