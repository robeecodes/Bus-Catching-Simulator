using UnityEngine;

public class HandleDialogueRotation : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - _mainCamera.transform.position).normalized);
    }
}
