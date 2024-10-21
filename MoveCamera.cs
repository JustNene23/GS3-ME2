using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Transform playerCamera; // Reference to the camera

    //public float height = 1.6f; // Adjust the height to match the player

    //new stuff
    public float sensX = 10f;
    public float sensY = 10f;

    private float xRotation = 0f; // Vertical rotation of the camera
    private float yRotation = 0f; // Horizontal rotation of the player

    public UpgradeManager upgradeManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Update horizontal (y-axis) rotation for the player
        yRotation += mouseX;

        // Update vertical (x-axis) rotation for the camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f); // Clamp vertical rotation to prevent flipping

        // Apply rotation to the player (horizontal)
        player.rotation = Quaternion.Euler(0, yRotation, 0);

        // Apply rotation to the camera (vertical)
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (upgradeManager.waveBreak == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (upgradeManager.waveBreak == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


    }

    /*
    void LateUpdate()
    {
        // Set the camera rig’s position to follow the player’s position
        Vector3 newPosition = player.position;
        newPosition.y = height;
        transform.position = newPosition;

        // Set the camera's rotation based on player input (if needed)
        playerCamera.rotation = player.rotation;
    }
    */
}
