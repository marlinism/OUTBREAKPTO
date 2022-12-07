using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadSystem : MonoBehaviour
{
    // Singleton instance
    private static UnloadSystem instance;

    private GameObject targetObject;
    private LinkedList<Unloadable> unloadedList;

    // Instance property
    public static UnloadSystem Inst
    {
        get { return instance; }
    }

    // Target property
    public GameObject TargetObject
    {
        get { return targetObject; }
        set { targetObject = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        unloadedList = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null)
        {
            targetObject = PlayerSystem.Inst.GetPlayer();
            return;
        }

        Vector2Int targetPos = Vector2Int.RoundToInt(targetObject.transform.position);
        float cameraZoomScale = GameSystem.Inst.CameraControl.SizeScale;

        LinkedListNode<Unloadable> curr = unloadedList.First;
        while (curr != null)
        {
            Unloadable currVal = curr.Value;
            int scaledUnloadDist = (int)(currVal.UnloadDistance * cameraZoomScale);
            if ((targetPos - currVal.UnloadedPosition).sqrMagnitude < currVal.UnloadDistance * currVal.UnloadDistance)
            {
                currVal.Reload();
                LinkedListNode<Unloadable> toRemove = curr;
                curr = curr.Next;
                unloadedList.Remove(toRemove);
            }
            else
            {
                curr = curr.Next;
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    public void Add(Unloadable toAdd)
    {
        unloadedList.AddLast(toAdd);
    }

    public bool Remove(Unloadable toRemove)
    {
        return unloadedList.Remove(toRemove);
    }
}
