using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f;
    [SerializeField] private bool lockCursor = true;

    private Vector2 _lookVector;
    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;
        
    private float _cameraPitch = 0.0f;
    private PlaceHolderInputs _inputs;

    private void Start()
    {
        if (!lockCursor) return;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _inputs = GetComponent<PlaceHolderInputs>();
    }

    private void Update()
    {
        UpdateMouseLook();
        _lookVector = _inputs.LookDirection;
    }
    private void UpdateMouseLook()
    {
        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, _lookVector,
            ref _currentMouseDeltaVelocity, mouseSmoothTime);
            
        _cameraPitch -= _currentMouseDelta.y * mouseSensitivity;

        _cameraPitch = Mathf.Clamp(_cameraPitch, -90, 90);
            
        playerCamera.localEulerAngles = Vector3.right * _cameraPitch;
            
        transform.Rotate(Vector3.up * _currentMouseDelta.x * mouseSensitivity);
    }
}
