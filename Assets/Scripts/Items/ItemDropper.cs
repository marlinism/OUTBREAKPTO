using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemDropper : MonoBehaviour
{
    [System.Serializable]
    private struct ListNode
    {
        public GameObject item;
        public float possibility;
    }

    [System.Serializable]
    private class MinMaxFloat
    {
        public float min = 0.5f;
        public float max = 2f;
    }

    [SerializeField]
    private List<ListNode> itemList;

    [SerializeField]
    private GameObject spawnTarget = null;

    [SerializeField]
    private MinMaxFloat spawnAddForce;

    [SerializeField]
    private int minDroppedItems = 0;

    [SerializeField]
    private int maxDroppedItems = 3;

    void Awake()
    {
    }

    public void DropItems()
    {
        if (itemList.Count <= 0)
        {
            return;
        }

        int droppedCount = 0;
        do
        {
            for (int i = 0; i < itemList.Count; ++i)
            {
                if (itemList[i].possibility <= 0)
                {
                    continue;
                }
                else if (itemList[i].possibility >= 1)
                {
                    InstantiateItem(itemList[i].item);
                    ++droppedCount;
                }
                else if (Random.Range(0f, 1f) <= itemList[i].possibility)
                {
                    InstantiateItem(itemList[i].item);
                    ++droppedCount;
                }

                if (droppedCount >= maxDroppedItems)
                {
                    return;
                }
            }

            
        } 
        while (droppedCount < minDroppedItems);
    }

    private void InstantiateItem(GameObject item)
    {
        GameObject spawnedItem = Instantiate(item);
        if (spawnTarget != null)
        {
            spawnedItem.transform.position = spawnTarget.transform.position;
        }
        else
        {
            spawnedItem.transform.position = transform.position;
        }
        StorageSystem.Inst.StoreItem(spawnedItem);

        Rigidbody2D rb = spawnedItem.GetComponent<Rigidbody2D>();
        if (rb != null && spawnAddForce.min * spawnAddForce.max > 0)
        {
            Vector2 direction = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();

            rb.AddForce(direction * Random.Range(spawnAddForce.min, spawnAddForce.max), ForceMode2D.Impulse);
        }
    }
}
