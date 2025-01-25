using System;
using Ky;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : Singleton<DebugManager>
{
    [SerializeField] private KeyCode refreshKey = KeyCode.F5;
    [SerializeField] private KeyCode stopTimeKey = KeyCode.P;

    private void Update()
    {
        if (Input.GetKeyDown(refreshKey))
        {
            ResetScene();
        }
        if (Input.GetKeyDown(stopTimeKey))
        {
            Time.timeScale = (1f - Time.timeScale);
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}