using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeController : MonoBehaviour {
    public PointerController pointerController;
    public MeshController meshController;
    public SelectController selectController;

    public string state;
    public float originalHitDistance;
    public Vector3 extrudeNormal;
    public Vector3 voxelPosition;

    public void Update() {
        if (state == "extruding") {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, originalHitDistance));
            Vector3 difference = worldPosition - voxelPosition;
            Vector3 span = new Vector3(difference.x * extrudeNormal.x, difference.y * extrudeNormal.y, difference.z * extrudeNormal.z);

            if ((span.x > 1f) || (span.y > 1f) || (span.z > 1f)) {
                voxelPosition += extrudeNormal;
                originalHitDistance = Vector3.Distance(voxelPosition, Camera.main.transform.position);

                if (selectController.selectData.Count > 0) {
                    selectController.Add(Vector3Int.FloorToInt(extrudeNormal), true);

                } else {
                    meshController.AddVoxel(Vector3Int.FloorToInt(voxelPosition));
                }

            } else if ((span.x < -1f) || (span.y < -1f) || (span.z < -1f)) {
                if (selectController.selectData.Count > 0) {
                    List<Vector3Int> tmp = new List<Vector3Int>();

                    foreach (Vector3Int pos in selectController.selectData) {
                        meshController.RemoveVoxel(pos);
                        Vector3Int newPos = Vector3Int.FloorToInt(pos - extrudeNormal);
                        tmp.Add(newPos);
                    }

                    selectController.Clear();
                    foreach (Vector3Int pos in tmp) {
                        selectController.Select(pos);
                    }

                } else {
                    meshController.RemoveVoxel(Vector3Int.FloorToInt(voxelPosition));
                }

                voxelPosition -= extrudeNormal;
                originalHitDistance = Vector3.Distance(voxelPosition, Camera.main.transform.position);
            }
        }
    }

    public void StartExtrude() {
        Vector3 position = pointerController.normalPosition;
        if (((position.x >= -1) && (position.x <= meshController.data.GetLength(0))) && ((position.y >= -1) && (position.y <= meshController.data.GetLength(1))) && ((position.z >= -1) && (position.z <= meshController.data.GetLength(2)))) {
            state = "extruding";
            extrudeNormal = pointerController.normal;
            voxelPosition = pointerController.position + new Vector3(.5f, .5f, .5f);
            originalHitDistance = pointerController.hitDistance;
        }
    }

    public void StopExtrude() {
        state = "";
    }
}
