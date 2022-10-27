using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AnimationEvents : MonoBehaviour
{
    // Component references
    private PlayerManager player;
    private MoveStateManager msManager;

    // Start is called before the first frame update
    void Start()
    {
        // bruh this is so scuffed
        Transform playerGameObject = transform.parent.transform.parent.transform.parent;

        player = playerGameObject.GetComponent<PlayerManager>();
        Assert.IsNotNull(player);

        msManager = playerGameObject.GetComponent<MoveStateManager>();
        Assert.IsNotNull(msManager);
    }

    public void CompleteState()
    {
        msManager.CompleteCurrentState();
    }
}
