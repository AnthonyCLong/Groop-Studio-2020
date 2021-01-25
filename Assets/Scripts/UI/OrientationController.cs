using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrientationController : MonoBehaviour, IPointerClickHandler {
    public CameraController cameraController;

    public Camera orientationCamera;

    public GameObject negX;
    public GameObject posX;
    public GameObject negY;
    public GameObject posY;
    public GameObject negZ;
    public GameObject posZ;

    public void OnPointerClick(PointerEventData pointerEventData) {
        Vector3 normalMousePosition = pointerEventData.pressPosition / (2 * transform.GetComponent<RectTransform>().sizeDelta);

        RaycastHit hit;
        Ray ray = orientationCamera.ViewportPointToRay(normalMousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.magenta, 2);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject == negX) {
                cameraController.SetLeft();
            } else if (hit.transform.gameObject == posX) {
                cameraController.SetRight();
            } else if (hit.transform.gameObject == negY) {
                cameraController.SetBottom();
            } else if (hit.transform.gameObject == posY) {
                cameraController.SetTop();
            } else if (hit.transform.gameObject == negZ) {
                cameraController.SetFront();
            } else if (hit.transform.gameObject == posZ) {
                cameraController.SetBack();
            }
        }
    }

}
