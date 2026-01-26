using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ProfileData
{
    public string profileName;
    public int vehicleID;
    public float bestTime;

    public ProfileData(string name, int vehicleID, float bestTime)
    {
        this.profileName = name;
        this.vehicleID = vehicleID;
        this.bestTime = bestTime;
    }
}
