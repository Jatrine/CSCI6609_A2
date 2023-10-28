using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MoveStep : MonoBehaviour
{
    [SerializeField]
    float speed = 0.5f;
    [SerializeField]
    GameObject canvas;

    int step = 0;
    int actualStep = 0;

    private void Start()
    {
        // The codes to access the step counter is from an external source.
        // https://www.reddit.com/r/Unity3D/comments/wjrg5q/how_to_use_the_pedometerstepcounter_sensor/
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
            AndroidRuntimePermissions.RequestPermission("android.permission.ACTIVITY_RECOGNITION");
        InputSystem.EnableDevice(StepCounter.current);
        StepCounter.current.MakeCurrent();
    }


    void Update()
    {
        canvas.GetComponent<Text>().text = " Step: " + actualStep;
        if (!DestroyRock.selecting)
        {
            if (StepCounter.current.stepCounter.ReadValue() > step)
            {
                MoveForward();
                step = StepCounter.current.stepCounter.ReadValue();
                actualStep++;
            }
        }

    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed);
        NetworkManager.UserAction = "Move forward";
    }
}
