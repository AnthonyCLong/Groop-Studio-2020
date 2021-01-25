using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontFix : MonoBehaviour {
    public Font pixelFont;

    public void Start() {
        pixelFont.material.mainTexture.filterMode = FilterMode.Point;
    }
}
