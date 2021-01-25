using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointerController : MonoBehaviour {

    public Vector3Int position;
    public Vector3Int normalPosition;
    public Vector3Int normal;
    public Vector3Int previousPosition;
    public float hitDistance;

    public void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.tag != null) {
                position = Vector3Int.FloorToInt(hit.point - hit.normal / 2);
                normal = Vector3Int.FloorToInt(hit.normal);
                normalPosition = position + normal;
                hitDistance = hit.distance;
            }

        } else {
            position = new Vector3Int(-10000, -10000, -10000);
            normal = new Vector3Int(0, 1, 0);
            normalPosition = position + normal;
        }

        Visualize();
    }

    public void Visualize() {
        transform.position = normalPosition + new Vector3(.5f, .5f, .5f) - Vector3.Scale(normal, new Vector3(.4999f, .4999f, .4999f));
        transform.rotation = Quaternion.LookRotation(normal * -1);
    }
}
