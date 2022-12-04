using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unloadable : MonoBehaviour
{
    [SerializeField]
    private int unloadDistance = 16;

    [SerializeField]
    private bool autoUnload = true;

    private Vector2Int unloadPos;

    // Properties
    public int UnloadDistance
    {
        get { return unloadDistance; }
    }
    public Vector2 UnloadedPosition
    {
        get { return unloadPos; }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (!autoUnload)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = PlayerSystem.Inst.GetPlayer();
        if (player == null)
        {
            return;
        }

        if ((player.transform.position - transform.position).sqrMagnitude > unloadDistance * unloadDistance)
        {
            Unload();
        }
    }

    public void Unload()
    {
        unloadPos = Vector2Int.RoundToInt(transform.position);
        UnloadSystem.Inst.Add(this);
        gameObject.SetActive(false);
    }

    public void Reload()
    {
        gameObject.SetActive(true);
    }

    public void ForceReload()
    {
        UnloadSystem.Inst.Remove(this);
        gameObject.SetActive(true);
    }
}
