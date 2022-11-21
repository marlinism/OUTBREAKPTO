using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffects : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> spriteList;

    [SerializeField]
    private Color defaultFlashColor = Color.red;

    [SerializeField]
    private float defaultFlashDuration = 0.1f;

    private float flashStart;
    private float flashDuration;

    // Start is called before the first frame update
    void Start()
    {
        flashStart = Time.time;
        flashDuration = defaultFlashDuration;
        SetFlashColor(defaultFlashColor);
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElapsed = Time.time - flashStart;

        // Check if flash is done
        if (timeElapsed > flashDuration)
        {
            //LinkedListNode<SpriteRenderer> currSR = spriteList.First;
            //while (currSR != null)
            //{
            //    currSR.Value.material.SetFloat("FlashIntensity", 0f);
            //    currSR = currSR.Next;
            //}

            foreach (SpriteRenderer sr in spriteList)
            {
                sr.material.SetFloat("_FlashIntensity", 0f);
            }

            enabled = false;
            return;
        }

        //LinkedListNode<SpriteRenderer> currSR = spriteList.First;
        //while (currSR != null)
        //{
        //    currSR.Value.material.SetFloat("FlashIntensity", timeElapsed / flashDuration);
        //    currSR = currSR.Next;
        //}

        foreach (SpriteRenderer sr in spriteList)
        {
            //sr.material.SetFloat("FlashIntensity", timeElapsed / flashDuration);
            sr.material.SetFloat("_FlashIntensity", 1f);
        }
    }

    public void PlayFlash()
    {
        PlayFlash(defaultFlashDuration);
    }

    public void PlayFlash(float duration)
    {
        flashStart = Time.time;
        flashDuration = duration;
        enabled = true;
    }

    public void SetFlashColor(Color flashColor)
    {
        foreach(SpriteRenderer sr in spriteList)
        {
            sr.material.SetColor("_FlashColor", flashColor);
        }
    }
}
