using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("General Settings")]

    [SerializeField]
    private int chipsThrown = 1;

    [SerializeField]
    private float throwForce = 0;

    [SerializeField]
    [Range(0, 100)]
    private float minThrowForce = 10;

    [SerializeField]
    [Range(0, 100)]
    private float maxThrowForce = 10;

    [SerializeField]
    private float forceGrowthRate = 5;

    [SerializeField]
    private GameObject chip;

    [SerializeField]
    private Transform spawnPoint;

    [Header("Camera Settings")]
    [SerializeField]
    [Range(0.1f, 5)]
    private float mouseSensitivity = 0f;

    private Camera myCamera = null;

    private float xAxisClamp = 0;

    private void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        RotateCamera();

        if (Input.GetMouseButton(0))
        {
            throwForce += forceGrowthRate * Time.deltaTime;

            throwForce = Mathf.Clamp(throwForce, minThrowForce, maxThrowForce);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Throw();
            throwForce = minThrowForce;
        }
    }

    private void Throw()
    {
        for (int i = 0; i < chipsThrown; i++)
        {
            Rigidbody rb = Instantiate(chip, spawnPoint.position, spawnPoint.rotation).GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * throwForce, ForceMode.Impulse);
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCam = myCamera.transform.eulerAngles;
        Vector2 targetRotBody = transform.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }

        targetRotCam.z = 0f;

        targetRotBody.y += rotAmountX;

        myCamera.transform.rotation = Quaternion.Euler(targetRotCam);
        transform.rotation = Quaternion.Euler(targetRotBody);
    }
}