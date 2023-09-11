using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMenuButton : MonoBehaviour
{
    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel(0);
    }
}
