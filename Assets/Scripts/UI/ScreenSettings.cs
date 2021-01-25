using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour {
    public GeneralColor ambientColor;

    public Light directionalLight;
    public Light directionalBackLight;
    public GeneralColor directionalColor;

    public Camera main;
    public GeneralColor backgroundColor;

    public GameObject grid;
    public GeneralColor gridColor;

    public GameObject curentVoxel;
    public GameObject lightContainer;
    public Button ring;
    public GameObject cursor;

    public void UpdateAmbient() {
        RenderSettings.ambientLight = ambientColor.color;
    }

    public void UpdateDirectional() {
        directionalLight.color = directionalColor.color;
        directionalBackLight.color = directionalColor.color;
    }

    public void UpdateBackground() {
        main.backgroundColor = backgroundColor.color;
    }

    public void UpdateGrid() {
        grid.GetComponent<MeshRenderer>().material.color = gridColor.color;
    }

    public void Shadow(bool state) {
        if (state)
            curentVoxel.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        else
            curentVoxel.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void SetLightPosition() {
        Vector2 pos = Input.mousePosition - cursor.transform.position;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg + 90;
        cursor.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        lightContainer.transform.eulerAngles = new Vector3(0, -angle - 30, 0);
    }
}
