using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;      // 3D model in-hand
    public GameObject projectilePrefab;  // projectile prefab
    public float fireRate = 0.5f;        // seconds between shots
    public float damage = 10f;
    public float velocity = 20f;         // projectile speed
    public bool automatic = false;
    public AmmoData ammo;                // reference to ammo type
}