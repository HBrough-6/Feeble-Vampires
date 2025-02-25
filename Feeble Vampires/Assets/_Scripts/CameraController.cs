using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public InputManager inputManager;

    public int cameraRotation;

    public UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        cameraRotation = 0;

        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inputManager.clockwise)) rotateCamera(90);
        if (Input.GetKeyDown(inputManager.counterClockwise)) rotateCamera(-90);
    }

    public void rotateCamera(int rotationValue)
    {
        cameraRotation += rotationValue;

        cameraRotation = cameraRotation % 360;

        mainCamera.transform.rotation = Quaternion.identity;
        mainCamera.transform.rotation *= Quaternion.Euler(76, cameraRotation, 0);

        if (cameraRotation == 0) mainCamera.transform.position = new Vector3(11.5f, mainCamera.transform.position.y, 4f);
        else if (cameraRotation == 90 || cameraRotation == -270) 
            mainCamera.transform.position = new Vector3(4f, mainCamera.transform.position.y, 11.5f);
        else if (Mathf.Abs(cameraRotation) == 180) mainCamera.transform.position = new Vector3(11.5f, mainCamera.transform.position.y, 19f);
        if (cameraRotation == -90 || cameraRotation == 270)
            mainCamera.transform.position = new Vector3(19f, mainCamera.transform.position.y, 11.5f);

        
    }
}
