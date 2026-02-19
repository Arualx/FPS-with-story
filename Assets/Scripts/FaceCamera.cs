using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    private Transform localTransform;

    private void Start()
    {
        localTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        localTransform.LookAt(2*localTransform.position - mainCamera.transform.position);
    }

}
