using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform target;
    float followSpeed = 10f;
    public float senstivity = 400f;
    float clampAnglge = 70f;

    private float rotX;
    private float rotY;

    //Cam Info
    public Transform realCam;
    // Cam Direction
    public Vector3 dirNormalized;
    // Cam final Direction
    public Vector3 finalDir;

    public float finalDistance;

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCam.localPosition.normalized;
        finalDistance = realCam.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rotX += -(Input.GetAxis("Mouse Y")) * senstivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * senstivity * Time.deltaTime;

        // Limit Angle
        rotX = Mathf.Clamp(rotX, -clampAnglge, clampAnglge);

        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;

        transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNormalized);


    }


}
