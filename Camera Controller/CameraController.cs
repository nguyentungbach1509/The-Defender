using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] MeowKnightControllerPC _meowKnight;
    [SerializeField] FireKnightController _fireKnight;

    #region Camera Change
    [SerializeField] float _changeDuration;
    float _endChangeTime;
    bool _change;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(_meowKnight.transform.position.x, _meowKnight.transform.position.y, transform.position.z);
        if(_fireKnight.gameObject.activeInHierarchy)
        {
            if(!_change)
            {
                _endChangeTime = Time.time + _changeDuration;
                _change = true;
            }
            else
            {
                if(Time.time <= _endChangeTime) newPosition = new Vector3(_fireKnight.transform.position.x, _fireKnight.transform.position.y, transform.position.z);
                    
            }
        }    
        float clampX = Mathf.Clamp(newPosition.x, GameManager.Instance._minBoundHorizontal, GameManager.Instance._maxBoundHorizontal);
        float clampY = Mathf.Clamp(newPosition.y, GameManager.Instance._minBoundVertical, GameManager.Instance._maxBoundVertical);
        transform.position = new Vector3(clampX, clampY, newPosition.z);
    }

    
}
