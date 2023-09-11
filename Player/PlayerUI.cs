using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] MeowKnightControllerPC _player;
    [SerializeField] Image _mask;
    [SerializeField] Text _playerPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mask.fillAmount = (_player._currentHealth / _player._maxHealth);
        _playerPoints.text = "  POINTS: " + _player.PlayerPoints;
    }
}
