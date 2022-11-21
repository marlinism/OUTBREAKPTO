using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleScript : MonoBehaviour
{
    private SpriteRenderer rend;
    public Color visibleColor;
    public Transform playerMapIcon;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, playerMapIcon.position) < 1f) {
            rend.color = visibleColor;
        }
    }
}
