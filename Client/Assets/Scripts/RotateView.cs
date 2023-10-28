using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateView : MonoBehaviour
{
    Gyroscope gyro;
    Vector3 invisibleCam;

    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    void Update()
    {
        invisibleCam = new Vector3 (0, (transform.eulerAngles - gyro.rotationRateUnbiased * Time.deltaTime * Mathf.Rad2Deg).y, 0);
        if (!DestroyRock.selecting)
        {
            NetworkManager.IsGyroActivate = true;
            transform.eulerAngles = invisibleCam;
            NetworkManager.GyroAngle = invisibleCam;
        }
        else
            NetworkManager.IsGyroActivate = false;
    }
}
