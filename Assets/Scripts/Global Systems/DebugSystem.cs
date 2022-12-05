using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSystem : MonoBehaviour
{
    private static DebugSystem instance;

    public GameObject spawnObject;
    public BossTentacleAttackSpawner tentacleSpawner;
    public int tentacleSpawnCount = 1;

    private float reloadPauseStart;

    private bool cameraZoomedOut = false;

    public static DebugSystem Instance
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

    // Update is called once per frame
    void Update()
    {
        // Create a spawnObject instance at the mouse if P is pressed
        if (Input.GetKeyDown(KeyCode.P) && spawnObject != null)
        {
            GameObject obj = Instantiate(spawnObject);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objPos.z = 0f;
            obj.transform.position = objPos;
        }
        // Create a spawnObject instance at the mouse if O is held
        if (Input.GetKey(KeyCode.O) && spawnObject != null)
        {
            GameObject obj = Instantiate(spawnObject);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objPos.z = 0f;
            obj.transform.position = objPos;
        }

        // Exit program
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}

        // Reload scene
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Spawn tentacle
        if (Input.GetKeyDown(KeyCode.T))
        {
            tentacleSpawner.Spawn(tentacleSpawnCount);
        }

        // Spawn tentacle wave
        if (Input.GetKeyDown(KeyCode.Y))
        {
            tentacleSpawner.SpawnWave();
        }

        // Toggle camera size
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    if (cameraZoomedOut)
        //    {
        //        GameSystem.Inst.CameraControl.ChangeCameraSizeScale(ZoomLevel.Normal);
        //        cameraZoomedOut = false;
        //    }
        //    else
        //    {
        //        GameSystem.Inst.CameraControl.ChangeCameraSizeScale(ZoomLevel.ZoomOut2);
        //        cameraZoomedOut = true;
        //    }
        //}
    }

    public void DeathReload()
    {
        StartCoroutine(DeathReloadCoroutine());
    }

    IEnumerator DeathReloadCoroutine()
    {
        reloadPauseStart = Time.unscaledTime;
        Time.timeScale = 0f;
        while (Time.unscaledTime - reloadPauseStart < 1f)
        {
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield break;
    }

}
