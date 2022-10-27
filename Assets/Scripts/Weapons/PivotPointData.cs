using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPointData : MonoBehaviour
{
    public bool flipWeaponY = false;
    public bool behindPlayer = false;

    public int LayerOrder
    {
        get
        {
            if (behindPlayer)
            {
                return -2;
            }
            else
            {
                return 2;
            }
        }
    }
}
