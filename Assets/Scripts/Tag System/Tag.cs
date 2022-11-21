using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tag", menuName = "Tags/New Tag")]

public class Tag : ScriptableObject
{
    public string Name
    {
        get { return name; }
    }
}
