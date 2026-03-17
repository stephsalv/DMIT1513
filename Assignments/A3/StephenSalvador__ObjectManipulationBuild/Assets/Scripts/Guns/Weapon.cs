using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    [SerializeField] private GameObject barrelEnd;

    private float lastFireTime = 0f;

    public void FireWeapon()
    {
        if (weaponData == null) return;

        // Check ammo
        if (weaponData.ammo != null && weaponData.ammo.totalCarried <= 0)
        {
            Debug.Log("Out of ammo for " + weaponData.weaponName);
            return;
        }

        // Fire rate cooldown
        if (Time.time < lastFireTime + weaponData.fireRate) return;

        // Spawn projectile
        if (weaponData.projectilePrefab != null)
        {
            GameObject proj = Instantiate(
                weaponData.projectilePrefab,
                barrelEnd.transform.position,
                barrelEnd.transform.rotation
            );

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = barrelEnd.transform.forward * weaponData.velocity;

            // Ignore collision with weapon/player
            Collider weaponCol = GetComponentInChildren<Collider>();
            Collider projCol = proj.GetComponentInChildren<Collider>();
            if (weaponCol != null && projCol != null)
                Physics.IgnoreCollision(weaponCol, projCol);
        }

        lastFireTime = Time.time;

        // Reduce ammo
        if (weaponData.ammo != null)
            weaponData.ammo.totalCarried--;
    }
}