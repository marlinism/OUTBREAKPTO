using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneTimer : MonoBehaviour
{
    [SerializeField]
    private int waitSeconds = 20;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReturnTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator ReturnTimer()
    {
        yield return new WaitForSeconds(waitSeconds);
        SceneManager.LoadScene(0);
        yield break;
    }
}
