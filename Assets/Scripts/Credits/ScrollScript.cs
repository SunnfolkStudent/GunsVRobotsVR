using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;

public class ScrollScript : MonoBehaviour
{
    private float _time;
    [SerializeField]
    private Vector3 _startPosition;
    [SerializeField]
    private Vector3 _target;
    private float _timeToReachTarget;
    void Start()
    {
        _startPosition = transform.position;
        // Making target resolution independent
        var rectTransform = GetComponentInChildren<RectTransform>();
        _target = new Vector3(_startPosition.x, 200, 100);
        SetDestination();
    }
    void Update()
    {
        _time += Time.deltaTime / _timeToReachTarget;
        transform.position = Vector3.Lerp(_startPosition, _target, _time);

        if (Keyboard.current.anyKey.isPressed)
        {
            _timeToReachTarget = 5f;
        }
        else
        {
            _timeToReachTarget = 30f;
        }

        if (transform.position.y > _target.y - 1)
        {
            transform.position = _startPosition;
            SetDestination();
        }
    }
    public void SetDestination()
    {
        _time = 0;
        _startPosition = transform.position;
        _timeToReachTarget = 30f;
    }
}
