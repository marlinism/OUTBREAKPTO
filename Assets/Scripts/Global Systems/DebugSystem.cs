using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSystem : MonoBehaviour
{
    private static DebugSystem instance;

    public GameObject spawnObject;

    private float reloadPauseStart;

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

        //// Exit program
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}

        // Reload scene
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
