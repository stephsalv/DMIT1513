using UnityEngine;

[CreateAssetMenu(fileName = "NewAmmo", menuName = "Weapons/Ammo")]
public class AmmoData : ScriptableObject
{
    public string ammoName;      // e.g., "Pistol Ammo", "Rifle Ammo"
    public int maxCapacity = 30; // max bullets in a clip
    public int totalCarried = 90; // total bullets player can carry
}
