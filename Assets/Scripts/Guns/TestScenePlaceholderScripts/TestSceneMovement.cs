
using UnityEngine;

[RequireComponent(typeof(PlaceHolderInputs))]
public class TestSceneMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -13f;
    [SerializeField] private float jumpSpeed = 3f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] [Range(0.0f, 0.5f)] private float moveSmoothTime = 0.3f;
            
    private float _velocityY = 0.0f;
    private bool _jumpPressed;
        
    public Vector2 _moveDirection;
    private Vector2 _currentDirection = Vector2.zero;
    private Vector2 _currentDirectionVelocity = Vector2.zero;
        
    private PlaceHolderInputs _controller;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _controller = GetComponent<PlaceHolderInputs>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        updateMovement();
    }

    private void updateMovement()
    {
        if (_controller.MoveVector != Vector2.zero)
        {
            _rigidbody.velocity = transform.forward * moveSpeed;
        }
        else
        {
            _moveDirection = Vector2.zero;
        }
    }


    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        _currentDirection = Vector2.SmoothDamp(_currentDirection, _moveDirection,
            ref _currentDirectionVelocity, moveSmoothTime);

        var velocity = (transform.forward * _currentDirection.y + transform.right * 
            _currentDirection.x) * moveSpeed + Vector3.up * _velocityY;
    }
}


    

