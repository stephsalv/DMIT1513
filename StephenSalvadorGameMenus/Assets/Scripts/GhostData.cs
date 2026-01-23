using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostData : MonoBehaviour
{
    public List<GhostDataFrame> ghostDataFrames = new List<GhostDataFrame>();
}
[Serializable]
public class GhostDataFrame
{
    Vector3 position;
    Vector3 rotation;
    float time;
    public GhostDataFrame(Vector3 position_, Vector3 rotation_, float time)
    {
        this.position =  position_;
        this.rotation = rotation_;
        this.time = time;
    }
}
