using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CoordinateController {
    
    public static Vector3Int WorldToGrid(Vector3 position, Vector3 normal) {
        Vector3 fixedPosition = position - (normal / 1000);

        return new Vector3Int((int)fixedPosition.x, (int)fixedPosition.y, (int)fixedPosition.z);
    }

    public static Vector3 GridToWorld(Vector3Int position) {
        return new Vector3(position.x + .5f, position.y + .5f, position.z + .5f);
    }

    public static Vector3 GridNormal(Vector3 position, Vector3 normal) {
        return GridToWorld(WorldToGrid(position, normal));
    }

    public static Vector3Int Vector3ToVector3Int(Vector3 vector) {
        return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
    }
}
