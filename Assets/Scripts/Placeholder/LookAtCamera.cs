using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Update()
    {
        if (Camera.main != null) transform.LookAt(Camera.main.transform);
    }
}