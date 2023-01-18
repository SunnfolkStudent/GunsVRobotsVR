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
    [SerializeField]
    private float _timeToReachTarget;
    [SerializeField]
    private Vector3 distance;
    public AnimationCurve curve;
    void Start()
    {
        _startPosition = transform.position;
        // Making target resolution independent
        var rectTransform = GetComponentInChildren<RectTransform>();
        _target = new Vector3(_startPosition.x + distance.x, _startPosition.y + distance.y, _startPosition.z + distance.z);
        SetDestination();
    }
    void Update()
    {
        _time += Time.deltaTime / _timeToReachTarget;
        Vector3 position = transform.position;
        position = Vector3.Lerp(_startPosition, _target, curve.Evaluate(_time));
        //position.z = zDistance * ;
        transform.position = position;    
        
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
