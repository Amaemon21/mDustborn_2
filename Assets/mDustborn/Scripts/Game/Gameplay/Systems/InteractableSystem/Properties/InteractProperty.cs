using System;
using UnityEngine;

[Serializable]
public class InteractProperty
{
    public Camera Camera;
    public LayerMask HitScanMask;
    public float InteractRange = 5f;
}