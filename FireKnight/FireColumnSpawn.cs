using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FireColumnSpawn : MonoBehaviour
{
    [SerializeField] float _flySpeed;
    private bool _isMoving = false;
    private float _startTime;
    private Vector3 _startPosition;
    private Vector3 _destinationPosition;

    float _nextTimeSpawn;
    [SerializeField] float _spawnCooldown;

    void Update()
    {
        if (_isMoving)
        {
            float distanceCovered = (Time.time - _startTime) * _flySpeed;
            float journeyFraction = distanceCovered / Vector2.Distance(_startPosition, _destinationPosition);

            transform.position = Vector2.Lerp(_startPosition, _destinationPosition, journeyFraction);
            SpawnFireColumn();

            if (journeyFraction >= 1.0f)
            {
                _isMoving = false;
                transform.position = _startPosition;
                Debug.Log("Refresh " + transform.position);
            }
        }
    }

    public void MoveToTarget(Vector3 targetPosition, Vector3 startPosition)
    {
        Debug.Log(startPosition);
        _isMoving = true;
        _startTime = Time.time;
        _startPosition = startPosition;
        _destinationPosition = targetPosition;
    }


    private void SpawnFireColumn()
    {
        if (Time.time > _nextTimeSpawn)
        {
            FireColumn fireColumn = PoolingFireColumnSpawnSystem.Instance.TakeFireColumn();
            fireColumn.transform.position = transform.position;
            fireColumn.gameObject.SetActive(true);
            _nextTimeSpawn = Time.time + _spawnCooldown;
        }
    }

}
