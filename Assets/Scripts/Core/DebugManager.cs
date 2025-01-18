using System;
using Ky;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : Singleton<DebugManager>
{
    [SerializeField] private KeyCode refreshKey = KeyCode.F5;

    private void Update()
    {
        if (Input.GetKeyDown(refreshKey))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}