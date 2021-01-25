using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySettings : MonoBehaviour {
    public Canvas UI;
    public float previousWidth;

    public void Update() {
        ScaleCanvas();
    }

    public void Start() {
        Screen.fullScreen = false;
    }

    public void ScaleCanvas() {
        if (Screen.width != previousWidth) {
            if (Screen.width < 1000) {
                UI.scaleFactor = 1;
            } else {
                UI.scaleFactor = 2;
            }
        }

        previousWidth = Screen.width;
    }
}
