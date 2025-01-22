using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private bool isFirstPerson = false;
    public float firstPersonHeight = 0.5f;
    public float mouseSensitivity = 100f;
    public float movementSpeed = 5f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Rigidbody playerRigidbody;

    void Start()
    {
        offset = transform.position - player.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFirstPerson = !isFirstPerson;
            Cursor.lockState = isFirstPerson ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (isFirstPerson)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation += mouseX;
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    void FixedUpdate()
    {
        if (isFirstPerson)
        {
            HandleFirstPersonMovement();
        }
    }

    void LateUpdate()
    {
        if (isFirstPerson)
        {
            transform.position = player.transform.position + new Vector3(0, firstPersonHeight, 0);
        }
        else
        {
            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform.position);
        }
    }

    private void HandleFirstPersonMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 movementDirection = (forward * moveVertical + right * moveHorizontal).normalized;
        playerRigidbody.velocity = movementDirection * movementSpeed + new Vector3(0, playerRigidbody.velocity.y, 0);
    }
}
