using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GhostData
{
    public List<GhostDataFrame> ghostDataFrames = new List<GhostDataFrame>();
    public float bestTime;
    public void AddFrame(Vector3 position_, Vector3 rotation_)
    {
        ghostDataFrames.Add(new GhostDataFrame(position_, rotation_));
    }
}
[Serializable]
public class GhostDataFrame
{
    public Vector3 position;
    public Vector3 rotation;

    public GhostDataFrame(Vector3 position_, Vector3 rotation_)
    {
        position = position_;
        rotation = rotation_;
    }
}
