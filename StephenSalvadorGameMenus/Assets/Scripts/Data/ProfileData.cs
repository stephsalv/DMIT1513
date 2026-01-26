using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ProfileData
{
    public string profileName;
    public int vehicleID;
    public float bestTime;

    public ProfileData(Profile profile)
    {
        profileName = profile.profileName;
        vehicleID = profile.vehicleID;
        bestTime = profile.bestTime;
    }
}
