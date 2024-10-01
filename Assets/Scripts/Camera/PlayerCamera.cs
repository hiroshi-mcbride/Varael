using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + Vector3.up * 2;
        
        Vector2 movement = new Vector2(Input.mousePositionDelta.x, Input.mousePositionDelta.y);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float pitch = Utility.ClampAngle(currentRotation.x - movement.y, -60.0f, 60.0f);
        //float yaw = Utility.ClampAngle(currentRotation.y - movement.x, -60.0f, 60.0f);
        
        transform.rotation = Quaternion.Euler(pitch, currentRotation.y + movement.x,
            currentRotation.z);

    }
}
