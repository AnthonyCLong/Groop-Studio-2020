using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class GeneralColor : MonoBehaviour {

    public Button ring;
    public Button square;
    public GameObject ringCursor;
    public GameObject squareCursor;
    public GameObject squareCursorReference;

    public GameObject colorDisplay;
    public Color color;
    private float h;
    private float s;
    private float v;

    public Color defaultColor;

    public void Start() {
        ring.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        square.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        ResetColors();
    }

    public void SetRingCursor() {
        Vector2 pos = Input.mousePosition - ringCursor.transform.position;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        ringCursor.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (angle < 0) {
            h = (angle + 360) / 360f;
        } else {
            h = angle / 360f;
        }

        ColorSquare();
        ColorRingCursor();
        ColorSquareCursor();
        ColorPrimary();
    }

    public void SetRingCursor(float hue) {
        ringCursor.transform.localRotation = Quaternion.AngleAxis(hue * 360, Vector3.forward);

        h = hue;

        ColorSquare();
        ColorRingCursor();
        ColorSquareCursor();
        ColorPrimary();
    }

    public void SetSquareCursor() {
        Vector2 min = new Vector2(square.transform.position.x - 100, square.transform.position.y - 100);
        Vector2 max = new Vector2(square.transform.position.x, square.transform.position.y);
        Vector2 mousePos = new Vector2(Input.mousePosition.x - 50, Input.mousePosition.y - 50);
        Vector2 cursorPos = new Vector2(Mathf.Clamp(mousePos.x, min.x, max.x), Mathf.Clamp(mousePos.y, min.y, max.y));

        squareCursor.transform.position = cursorPos;
        Vector2 values = (cursorPos - min) / 100;

        s = values.x;
        v = values.y;

        ColorSquareCursor();
        ColorPrimary();
    }

    public void SetSquareCursor(float saturation, float value) {
        Vector2 min = new Vector2(square.transform.position.x - 100, square.transform.position.y - 100);
        Vector2 max = new Vector2(square.transform.position.x, square.transform.position.y);
        Vector3 position = squareCursorReference.transform.position - new Vector3(100, 100, 100);
        Vector2 cursorPos = new Vector2(Mathf.Clamp(position.x + saturation * 100, min.x, max.x), Mathf.Clamp(position.y + value * 100, min.y, max.y));

        squareCursor.transform.position = cursorPos;
        Vector2 values = (cursorPos - min) / 100;

        s = values.x;
        v = values.y;

        ColorSquareCursor();
        ColorPrimary();
    }

    public void ColorPrimary() {
        Color tmp = Color.HSVToRGB(h, s, v);
        color = new Color(tmp.r, tmp.g, tmp.b, 1);
        colorDisplay.GetComponent<Image>().color = color;
    }

    public void ResetColors() {
        Color.RGBToHSV(defaultColor, out float h, out float s, out float v);
        SetRingCursor(h);
        SetSquareCursor(s, v);
    }

    public void ColorSquare() {
        Texture2D tex = square.GetComponent<Image>().sprite.texture;

        for (int y = 0; y < 100; y++) {
            for (int x = 0; x < 100; x++) {
                tex.SetPixel(x, y, Color.HSVToRGB(h, x * .01f, y * .01f));
            }
        }

        tex.Apply();
    }

    public void ColorSquareCursor() {
        squareCursor.GetComponent<Image>().color = Color.HSVToRGB(h, s, v);
    }

    public void ColorRingCursor() {
        ringCursor.GetComponent<Image>().color = Color.HSVToRGB(h, 1, 1);
    }
}