using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{
    [SerializeField]
    private int moveSpeed = 4;

    [SerializeField]
    private int speedIncrement = 1;

    private bool initialized = false;
    private GameObject oldPlayer;

    // Start is called before the first frame update
    void Start()
    {
        initialized = true;
        enabled = false;
    }

    private void OnEnable()
    {
        if (!initialized)
        {
            return;
        }

        oldPlayer = PlayerSystem.Inst.GetPlayer();
        transform.position = oldPlayer.transform.position;
        PlayerSystem.Inst.GetPlayerManager().enabled = false;
        PlayerSystem.Inst.SetPlayer(gameObject);
        UnloadSystem.Inst.SetTarget(gameObject);
    }

    private void OnDisable()
    {
        if (!initialized || oldPlayer == null)
        {
            return;
        }

        PlayerSystem.Inst.SetPlayer(oldPlayer);
        UnloadSystem.Inst.SetTarget(oldPlayer);
        oldPlayer.GetComponent<PlayerManager>().enabled = true;
        oldPlayer = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Speed
        if (Input.GetKeyDown(KeyCode.Period))
        {
            moveSpeed += speedIncrement;
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            moveSpeed -= speedIncrement;
        }

        // Move vector
        Vector3 moveDir = new();
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
        moveDir.z = 0f;
        moveDir.Normalize();

        // Apply speed
        transform.position += moveDir * (Time.deltaTime * moveSpeed);
    }
}
