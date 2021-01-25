using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimensionController : MonoBehaviour {
    public InputField length;
    public InputField width;
    public InputField height;

    public bool lengthSelected;
    public bool widthSelected;
    public bool heightSelected;

    public Image dimensionImage;

    public Sprite dimensionDefault;
    public Sprite dimensionLength;
    public Sprite dimensionWidth;
    public Sprite dimensionHeight;

    public CanvasController canvas;
    public CustomButton create;

    public void Start() {
        create.onLeftClick.AddListener(delegate () { CallCanvasCreate(); });
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (lengthSelected) {
                length.DeactivateInputField();
                width.Select();
            } else if (widthSelected) {
                width.DeactivateInputField();
                height.Select();
            } else if (heightSelected) {
                height.DeactivateInputField();
                length.Select();
            }
        }

        if ((length.isFocused == false && lengthSelected == true) || (width.isFocused == false && widthSelected == true) || (height.isFocused == false && heightSelected == true)) {
            dimensionImage.sprite = dimensionDefault;
        }

        if (length.isFocused == true && lengthSelected == false) {
            dimensionImage.sprite = dimensionLength;
        }

        if (width.isFocused == true && widthSelected == false) {
            dimensionImage.sprite = dimensionWidth;
        }

        if (height.isFocused == true && heightSelected == false) {
            dimensionImage.sprite = dimensionHeight;
        }

        lengthSelected = length.isFocused;
        widthSelected = width.isFocused;
        heightSelected = height.isFocused;
    }

    public void CallCanvasCreate() {
        int x, y, z;

        if (int.TryParse(length.text, out x) && int.TryParse(height.text, out y) && int.TryParse(width.text, out z)) {
            canvas.Create(true, x, y, z);
        }
    }
}
