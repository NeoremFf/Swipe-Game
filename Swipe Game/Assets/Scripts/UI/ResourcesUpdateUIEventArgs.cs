using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesUpdateUIEventArgs : MonoBehaviour
{
    public int Value { get; }
    public int Value_2 { get; }
    public ResourcesUpdateUIEventArgs(int value1, int value2 = 0)
    {
        Value = value1;
        Value_2 = value2;
    }
}
