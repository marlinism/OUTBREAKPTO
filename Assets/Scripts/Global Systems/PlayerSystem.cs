using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    // Singleton instance
    private static PlayerSystem instance;

    // Player instance
    GameObject player;

    // Instance property
    public static PlayerSystem Inst
    {
        get { return instance; }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetPlayer(GameObject inPlayer)
    {
        if (inPlayer.tag != "Player")
        {
            return;
        }

        player = inPlayer;
        UISystem.Inst.ShowUI();
    }

    public void RemovePlayer()
    {
        player = null;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public PlayerManager GetPlayerManager()
    {
        if (player == null)
        {
            return null;
        }

        return player.GetComponent<PlayerManager>();
    }
}
