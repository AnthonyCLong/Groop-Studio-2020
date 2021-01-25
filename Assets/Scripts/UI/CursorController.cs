using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {
    public Texture2D _default;
    public Texture2D selectDefault;
    public Texture2D selectLayer;
    public Texture2D selectColor;
    public Texture2D pen;
    public Texture2D eraser;
    public Texture2D brush;
    public Texture2D picker;
    public Texture2D extrude;

    public CursorMode cursorMode = CursorMode.ForceSoftware;

    public void SetDefault() {
        Cursor.SetCursor(_default, new Vector2(0, 0), cursorMode);
    }

    public void SetDefaultSelect() {
        Cursor.SetCursor(selectDefault, new Vector2(0, 0), cursorMode);
    }

    public void SetLayerSelect() {
        Cursor.SetCursor(selectLayer, new Vector2(0, 0), cursorMode);
    }
    public void SetColorSelect() {
        Cursor.SetCursor(selectColor, new Vector2(0, 0), cursorMode);
    }

    public void SetPen() {
        Cursor.SetCursor(pen, new Vector2(0, 0), cursorMode);
    }

    public void SetEraser() {
        Cursor.SetCursor(eraser, new Vector2(0, 0), cursorMode);
    }

    public void SetBrush() {
        Cursor.SetCursor(brush, new Vector2(0, 0), cursorMode);
    }

    public void SetPicker() {
        Cursor.SetCursor(picker, new Vector2(0, 0), cursorMode);
    }

    public void SetExtrude() {
        Cursor.SetCursor(extrude, new Vector2(0, 0), cursorMode);
    }
}