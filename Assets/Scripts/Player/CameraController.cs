using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject orientationCamera;
    public GameObject orientationCube;

    public GameObject gridCenter;
    public GameObject focalPoint;
    public float sensitivity;

    public bool moving;
    public float moveTime;
    Vector3 mainOriginal;
    Vector3 mainTarget;
    Vector3 orientationOriginal;
    Vector3 orientationTarget;

    public void Start() {
        focalPoint.transform.position = gridCenter.transform.position;
        transform.position = new Vector3(gridCenter.transform.position.x, gridCenter.transform.position.y, -3 * gridCenter.transform.position.z);
        orientationCamera.transform.position = orientationCube.transform.position - new Vector3(0, 0, 50);
        PointCenter();
    }

    public void Update() {
        if (moving) {
            LerpCameras();
            moveTime += .05f;
            if (transform.position == mainTarget) {
                moving = false;
                moveTime = 0;
            }
        }
    }

    public void Orbit() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.RotateAround(focalPoint.transform.position, Vector3.up, mouseX * sensitivity);
        transform.RotateAround(focalPoint.transform.position, transform.right, -mouseY * sensitivity);
        transform.LookAt(focalPoint.transform.position);

        orientationCamera.transform.RotateAround(orientationCube.transform.position, Vector3.up, mouseX * sensitivity);
        orientationCamera.transform.RotateAround(orientationCube.transform.position, transform.right, -mouseY * sensitivity);
        orientationCamera.transform.LookAt(orientationCube.transform.position);
    }

    public void Zoom() {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float desiredSize = transform.gameObject.GetComponent<Camera>().orthographicSize - scrollWheel * sensitivity;
        transform.gameObject.GetComponent<Camera>().orthographicSize = Mathf.Clamp(desiredSize, 0.1f, 100);
    }

    public void Pan() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 moveX = transform.right * mouseX;
        Vector3 moveY = transform.up * mouseY;

        focalPoint.transform.position -= moveX + moveY;
        transform.position -= moveX + moveY;
    }

    public void Center() {
        focalPoint.transform.position = gridCenter.transform.position;
        transform.LookAt(focalPoint.transform.position, transform.up);
    }

    public void PointCenter() {
        transform.LookAt(focalPoint.transform.position);
        orientationCamera.transform.LookAt(orientationCube.transform.position);
    }

    public void LerpCameras() {
        transform.position = Vector3.Lerp(mainOriginal, mainTarget, moveTime);
        orientationCamera.transform.position = Vector3.Lerp(orientationOriginal, orientationTarget, moveTime);
        PointCenter();
    }

    public void SetFront() {
        focalPoint.transform.position = gridCenter.transform.position;
        mainOriginal = transform.position;
        mainTarget = new Vector3(gridCenter.transform.position.x, gridCenter.transform.position.y, -3 * gridCenter.transform.position.z);
        orientationOriginal = orientationCamera.transform.position;
        orientationTarget = orientationCube.transform.position - new Vector3(0, 0, 50);
        moving = true;
    }

    public void SetBack() {
        focalPoint.transform.position = gridCenter.transform.position;
        mainOriginal = transform.position;
        mainTarget = new Vector3(gridCenter.transform.position.x, gridCenter.transform.position.y, 3 * gridCenter.transform.position.z);
        orientationOriginal = orientationCamera.transform.position;
        orientationTarget = orientationCube.transform.position + new Vector3(0, 0, 50);
        moving = true;
    }

    public void SetRight() {
        focalPoint.transform.position = gridCenter.transform.position;
        mainOriginal = transform.position;
        mainTarget = new Vector3(3 * gridCenter.transform.position.x, gridCenter.transform.position.y, gridCenter.transform.position.z);
        orientationOriginal = orientationCamera.transform.position;
        orientationTarget = orientationCube.transform.position + new Vector3(50, 0, 0);
        moving = true;
    }

    public void SetLeft() {
        focalPoint.transform.position = gridCenter.transform.position;
        mainOriginal = transform.position;
        mainTarget = new Vector3(-3 * gridCenter.transform.position.x, gridCenter.transform.position.y, gridCenter.transform.position.z);
        orientationOriginal = orientationCamera.transform.position;
        orientationTarget = orientationCube.transform.position - new Vector3(50, 0, 0);
        moving = true;
    }

    public void SetTop() {
        focalPoint.transform.position = gridCenter.transform.position;
        mainOriginal = transform.position;
        mainTarget = new Vector3(gridCenter.transform.position.x, 3 * gridCenter.transform.position.y, gridCenter.transform.position.z);
        orientationOriginal = orientationCamera.transform.position;
        orientationTarget = orientationCube.transform.position + new Vector3(0, 50, 0);
        moving = true;
    }

    public void SetBottom() {
        focalPoint.transform.position = gridCenter.transform.position;
        mainOriginal = transform.position;
        mainTarget = new Vector3(gridCenter.transform.position.x, -3 * gridCenter.transform.position.y, gridCenter.transform.position.z);
        orientationOriginal = orientationCamera.transform.position;
        orientationTarget = orientationCube.transform.position - new Vector3(0, 50, 0);
        moving = true;
    }
}
