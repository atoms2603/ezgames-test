using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new();
}