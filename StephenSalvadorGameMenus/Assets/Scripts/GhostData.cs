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
    public GhostDataFrame(Vector3 position_, Vector3 rotation_)
    {
        this.position =  position_;
        this.rotation = rotation_;
    }
}
