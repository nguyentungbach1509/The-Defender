using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void Play()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel(2);
    }
}
