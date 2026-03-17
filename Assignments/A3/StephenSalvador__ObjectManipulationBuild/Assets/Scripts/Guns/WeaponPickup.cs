using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && weaponData != null)
        {
            player.PickupWeapon(weaponData);
            gameObject.SetActive(false);
        }
    }
}
