using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] Text _txtTime;

    public void CountDownWave(float timer)
    {
        if (timer > 0) timer -= Time.deltaTime;
        else timer = 0;

        DisplayTime(timer);
    } 

    void DisplayTime(float time)
    {
        if (time < 0) time = 0;
        
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        _txtTime.text = "Time Left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        
    }
}
