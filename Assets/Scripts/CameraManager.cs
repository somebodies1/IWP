using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 originalPos;
    private Vector3 originalRotation;

    private void Start()
    {
        originalPos = mainCamera.transform.position;
        originalRotation = mainCamera.transform.rotation.eulerAngles;
    }

    public void SetMainCameraToOriginalState()
    {
        mainCamera.transform.position = originalPos;
        mainCamera.transform.eulerAngles = originalRotation;
    }

    public void CameraMovement(Vector3 _v3)
    {
        mainCamera.transform.position = _v3;
        //mainCamera.transform.Rotate(new Vector3(0, -90, 0));
    }

    public void CameraCloseUp(Vector3 _v3)
    {
        mainCamera.transform.position = _v3;
    }
}
