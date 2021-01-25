using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class ColorController : MonoBehaviour {
    public SelectController selectController;
    public  ToolController toolController;

    public Canvas canvas;
    public Button ring;
    public Button square;
    public GameObject ringCursor;
    public GameObject squareCursor;
    public GameObject squareCursorReference;

    public GameObject primaryDisplay;
    public static Color primaryColor;
    private float primaryH;
    private float primaryS;
    private float primaryV;

    public void Awake() {
        ring.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        square.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void SetRingCursor() {
        Vector2 pos = Input.mousePosition - ringCursor.transform.position;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        ringCursor.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (angle < 0) {
            primaryH = (angle + 360) / 360f;
        } else {
            primaryH = angle / 360f;
        }

        ColorSquare();
        ColorRingCursor();
        ColorSquareCursor();
        ColorPrimary();
    }

    public void SetRingCursor(float hue) {
        ringCursor.transform.localRotation = Quaternion.AngleAxis(hue * 360, Vector3.forward);

        primaryH = hue;

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

        primaryS = values.x;
        primaryV = values.y;

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

        primaryS = values.x;
        primaryV = values.y;

        ColorSquareCursor();
        ColorPrimary();
    }

    public void ColorPrimary() 
    {
        Color tmp = Color.HSVToRGB(primaryH, primaryS, primaryV);
        primaryColor = new Color(tmp.r, tmp.g, tmp.b, 1);
        primaryDisplay.GetComponent<Image>().color = primaryColor;
        selectController.colorSelection(primaryColor);
    }

    public void ResetColors() {
        SetRingCursor(0);
        SetSquareCursor(0, 1);
    }

    public void ColorSquare() {
        Texture2D tex = square.GetComponent<Image>().sprite.texture;

        for (int y = 0; y < 100; y++) {
            for (int x = 0; x < 100; x++) {
                tex.SetPixel(x, y, Color.HSVToRGB(primaryH, x * .01f, y * .01f));
            }
        }

        tex.Apply();
    }

    public void ColorSquareCursor() {
        squareCursor.GetComponent<Image>().color = Color.HSVToRGB(primaryH, primaryS, primaryV);
    }

    public void ColorRingCursor() {
        ringCursor.GetComponent<Image>().color = Color.HSVToRGB(primaryH, 1, 1);
    }

    public void MakeSquare() {
        Texture2D tex = new Texture2D(100, 100);

        for (int y = 0; y < 100; y++) {
            for (int x = 0; x < 100; x++) {
                tex.SetPixel(x, y, Color.HSVToRGB(0, x * .01f, y * .01f));
            }
        }

        tex.Apply();

        byte[] data = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/square.png", data);
    }

    public void MakeSquareCursor() {
        Texture2D tex = new Texture2D(200, 200);

        for (int y = 0; y < 200; y++) {
            for (int x = 0; x < 200; x++) {
                float distance = Mathf.Sqrt(Mathf.Pow(x - 190, 2) + Mathf.Pow(y - 190, 2));

                if (distance < 10) {
                    if (distance >= 7) {
                        tex.SetPixel(x, y, Color.black);
                    } else {
                        tex.SetPixel(x, y, Color.white);
                    }
                } else {
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }

        tex.Apply();

        byte[] data = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/squareCursor.png", data);
    }

    public void MakeRing() {
        Texture2D tex = new Texture2D(400, 400);

        for (int y = 0; y < 400; y++) {
            for (int x = 0; x < 400; x++) {
                float distance = Mathf.Sqrt(Mathf.Pow(x - 200, 2) + Mathf.Pow(y - 200, 2));

                if (distance >= 145 && distance < 190) {
                    float angle = Mathf.Atan2(y - 200, x - 200) * Mathf.Rad2Deg;
                    if (angle < 0)
                        angle += 360;

                    tex.SetPixel(x, y, Color.HSVToRGB(angle / 360f, 1, 1));
                } else {
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }

        tex.Apply();

        byte[] data = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/ring.png", data);
    }

    public void MakeRingCursor() {
        Texture2D tex = new Texture2D(400, 400);

        for (int y = 0; y < 400; y++) {
            for (int x = 0; x < 400; x++) {
                float distance = Mathf.Sqrt(Mathf.Pow(x - 367, 2) + Mathf.Pow(y - 200, 2));

                if (distance < 20) {
                    if (distance >= 17) {
                        tex.SetPixel(x, y, Color.black);
                    } else {
                        tex.SetPixel(x, y, Color.white);
                    }
                } else {
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }

        tex.Apply();

        byte[] data = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/ringCursor.png", data);
    }

    public void MakeColorDisplay() {
        Texture2D tex = new Texture2D(200, 200);

        for (int y = 0; y < 200; y++) {
            for (int x = 0; x < 200; x++) {
                float distance = Mathf.Sqrt(Mathf.Pow(x - 100, 2) + Mathf.Pow(y - 100, 2));

                if (distance < 100) {
                    tex.SetPixel(x, y, Color.white);
                } else {
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }

        tex.Apply();

        byte[] data = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/colorDisplay.png", data);
    }
}