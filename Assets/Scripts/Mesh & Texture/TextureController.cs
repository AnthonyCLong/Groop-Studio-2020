using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour {

    public void MakeTexture() {
        MeshController meshController = transform.GetComponent<MeshController>();

        Texture2D texture = new Texture2D(meshController.data.GetLength(0) * meshController.data.GetLength(1), meshController.data.GetLength(2), TextureFormat.RGBA32, false);

        Color[] colors = new Color[meshController.data.Length];
        for (int i = 0; i < colors.Length; i++) {
            colors[i] = Color.clear;
        }

        texture.SetPixels(colors);
        texture.Apply();

        transform.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", texture);
    }

    public void SetPixel(Vector3Int position, Color color) {
        Texture2D texture = transform.GetComponent<MeshRenderer>().material.GetTexture("_BaseMap") as Texture2D;
        MeshController meshController = transform.GetComponent<MeshController>();

        int x = position.x + (position.y * meshController.data.GetLength(0));
        int y = position.z;

        texture.SetPixel(x, y, meshController.data[position.x, position.y, position.z].color);
        texture.Apply();
    }

    public void Clear() {
        MeshController meshController = transform.GetComponent<MeshController>();
        Texture2D texture = transform.GetComponent<MeshRenderer>().material.GetTexture("_BaseMap") as Texture2D;

        Color[] colors = new Color[meshController.data.Length];
        for (int i = 0; i < colors.Length; i++) {
            colors[i] = Color.clear;
        }

        texture.SetPixels(colors);
        texture.Apply();
    }

    public void SaveTexture(string path) {
        Texture2D texture = transform.GetComponent<MeshRenderer>().material.GetTexture("_BaseMap") as Texture2D;
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
    }

    public void loadTexture(string path) {
        //Clear();
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        texture.Apply();

        transform.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", texture);
    }

}
