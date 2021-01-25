using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public enum Direction {
    posX = 0,
    posY = 1,
    posZ = 2,
    negX = 3,
    negY = 4,
    negZ = 5
}

public struct ArrayData {
public Color color;
public bool selected;
}


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshController : MonoBehaviour {
    
    //public HistoryController historyController;
    public GameObject debug;
    public bool destroyChildren;

    public ArrayData[,,] data;

    public void Awake() {
        GetComponent<MeshFilter>().mesh = new Mesh();
        data = new ArrayData[16, 16, 16];
    }

    public void Clear() 
    {
        HistoryController historyController = transform.GetComponent<HistoryController>();
        
        for (int x = 0; x < data.GetLength(0); x++)
        { 
            for (int y = 0; y < data.GetLength(1); y++)
            { 
                for (int z = 0; z < data.GetLength(2); z++)
                { 
                    RemoveVoxel(new Vector3Int(x,y,z));      
                }
                        
            }
                    
        }
        historyController.recordAction();
    }

   public void AddVoxel(Vector3Int position) {

        HistoryController historyController = transform.GetComponent<HistoryController>();
        // if ((position.x >= data.GetLength(0)) || (position.x < 0)) {
        //     Debug.Log("OUT OF BOUNDS X");
        //     return;
        // }

        // if ((position.y >= data.GetLength(1)) || (position.y < 0)) {
        //     Debug.Log("OUT OF BOUNDS Y");
        //     return;
        // }

        // if ((position.z >= data.GetLength(2)) || (position.z < 0)) {
        //     Debug.Log("OUT OF BOUNDS Z");
        //     return;
        // }

        // if (data[position.x, position.y, position.z].color != Color.clear) {
        //     Debug.Log("VOXEL ALREADY EXISTS");
        //     return;
        // }
        if(((position.x >= 0)&&(position.x < data.GetLength(0)))  &&  ((position.y >= 0)&&(position.y < data.GetLength(1)))  &&  ((position.z >= 0)&&(position.z < data.GetLength(2)))  &&  (data[position.x, position.y, position.z].color == Color.clear))
        {
            List<Direction> facesToAdd = new List<Direction>() { Direction.posX, Direction.posY, Direction.posZ, Direction.negX, Direction.negY, Direction.negZ };
            List<int> facesToRemove = new List<int>();
            Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            for (int i = 0; i < vertices.Length; i += 4) {
                if ((vertices[i] == (position)) && (normals[i] == Vector3.right)) {
                    if ((vertices[i + 2] == (position + new Vector3(0, 1, 1))) && (normals[i + 2] == Vector3.right)) {
                        facesToAdd.Remove(Direction.negX);
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(1, 0, 1))) && (normals[i] == Vector3.left)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 1, 0))) && (normals[i + 2] == Vector3.left)) {
                        facesToAdd.Remove(Direction.posX);
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(1, 0, 0))) && (normals[i] == Vector3.forward)) {
                    if ((vertices[i + 2] == (position + new Vector3(0, 1, 0))) && (normals[i + 2] == Vector3.forward)) {
                        facesToAdd.Remove(Direction.negZ);
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(0, 0, 1))) && (normals[i] == Vector3.back)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 1, 1))) && (normals[i + 2] == Vector3.back)) {
                        facesToAdd.Remove(Direction.posZ);
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position)) && (normals[i] == Vector3.up)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 0, 1))) && (normals[i + 2] == Vector3.up)) {
                        facesToAdd.Remove(Direction.negY);
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(0, 1, 1))) && (normals[i] == Vector3.down)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 1, 0))) && (normals[i + 2] == Vector3.down)) {
                        facesToAdd.Remove(Direction.posY);
                        facesToRemove.Add(i);
                    }

                }

            }

            historyController.AddToList(new HistoryEvent(AT.ADD, position, ColorController.primaryColor, Color.clear));
            data[position.x, position.y, position.z].color = ColorController.primaryColor;
            transform.GetComponent<TextureController>().SetPixel(position, ColorController.primaryColor);

            RemoveFaces(facesToRemove);
            AddFaces(position, facesToAdd);
        }
   }
   
    public void RemoveVoxel(Vector3Int position) 
    {
        HistoryController historyController = transform.GetComponent<HistoryController>();
        // if ((position.x >= data.GetLength(0)) || (position.x < 0)) {
        //     Debug.Log("OUT OF BOUNDS X");
        //     return;
        // }

        // if ((position.y >= data.GetLength(1)) || (position.y < 0)) {
        //     Debug.Log("OUT OF BOUNDS Y");
        //     return;
        // }

        // if ((position.z >= data.GetLength(2)) || (position.z < 0)) {
        //     Debug.Log("OUT OF BOUNDS Z");
        //     return;
        // }

        // if (data[position.x, position.y, position.z].color == Color.clear) {
        //     Debug.Log("VOXEL DOES NOT EXIST");
        //     return;
        // }

        if (((position.x >= 0)&&(position.x < data.GetLength(0)))  &&  ((position.y >= 0)&&(position.y < data.GetLength(1)))  &&  ((position.z >= 0)&&(position.z < data.GetLength(2)))  &&  (data[position.x, position.y, position.z].color != Color.clear))
        {   
            List<int> facesToRemove = new List<int>();
            Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            for (int i = 0; i < vertices.Length; i += 4) {
                if ((vertices[i] == (position + new Vector3(1, 0, 0))) && (normals[i] == Vector3.right)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 1, 1))) && (normals[i + 2] == Vector3.right)) {
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(0, 0, 1))) && (normals[i] == Vector3.left)) {
                    if ((vertices[i + 2] == (position + new Vector3(0, 1, 0))) && (normals[i + 2] == Vector3.left)) {
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(1, 0, 1))) && (normals[i] == Vector3.forward)) {
                    if ((vertices[i + 2] == (position + new Vector3(0, 1, 1))) && (normals[i + 2] == Vector3.forward)) {
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position)) && (normals[i] == Vector3.back)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 1, 0))) && (normals[i + 2] == Vector3.back)) {
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(0, 1, 0))) && (normals[i] == Vector3.up)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 1, 1))) && (normals[i + 2] == Vector3.up)) {
                        facesToRemove.Add(i);
                    }

                } else if ((vertices[i] == (position + new Vector3(0, 0, 1))) && (normals[i] == Vector3.down)) {
                    if ((vertices[i + 2] == (position + new Vector3(1, 0, 0))) && (normals[i + 2] == Vector3.down)) {
                        facesToRemove.Add(i);
                    }

                }
            }

            RemoveFaces(facesToRemove);
            data[position.x, position.y, position.z].color = Color.clear;
            transform.GetComponent<TextureController>().SetPixel(position, Color.clear);

            if (position.x < (data.GetLength(0) - 1)) {
                if (data[position.x + 1, position.y, position.z].color != Color.clear) {
                    AddFaces(position + Vector3Int.right, new List<Direction>() { Direction.negX });
                }
            }

            if (position.y < (data.GetLength(1) - 1)) {
                if (data[position.x, position.y + 1, position.z].color != Color.clear) {
                    AddFaces(position + Vector3Int.up, new List<Direction>() { Direction.negY });
                }
            }

            if (position.z < (data.GetLength(2) - 1)) {
                if (data[position.x, position.y, position.z + 1].color != Color.clear) {
                    AddFaces(position + new Vector3Int(0, 0, 1), new List<Direction>() { Direction.negZ });
                }
            }

            if (position.x > 0) {
                if (data[position.x - 1, position.y, position.z].color != Color.clear) {
                    AddFaces(position + Vector3Int.left, new List<Direction>() { Direction.posX });
                }
            }

            if (position.y > 0) {
                if (data[position.x, position.y - 1, position.z].color != Color.clear) {
                    AddFaces(position + Vector3Int.down, new List<Direction>() { Direction.posY });
                }
            }

            if (position.z > 0) {
                if (data[position.x, position.y, position.z - 1].color != Color.clear) {
                    AddFaces(position + new Vector3Int(0, 0, -1), new List<Direction>() { Direction.posZ });
                }
            }
            historyController.AddToList(new HistoryEvent(AT.RMV, position, ColorController.primaryColor, Color.clear));
        }
    }

    public void RemoveFaces(List<int> index) {
        //Debug.Log("Removing " + index.Count + " faces");

        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);
        List<Vector2> uvs = new List<Vector2>(mesh.uv);

        for (int i = 0; i < index.Count; i++) {
            vertices.RemoveAt(index[i]);
            vertices.RemoveAt(index[i]);
            vertices.RemoveAt(index[i]);
            vertices.RemoveAt(index[i]);

            uvs.RemoveAt(index[i]);
            uvs.RemoveAt(index[i]);
            uvs.RemoveAt(index[i]);
            uvs.RemoveAt(index[i]);

            for (int j = 0; j < index.Count; j++) {
                if (index[j] > index[i]) {
                    index[j] -= 4;
                }
            }

            for (int j = 0; j < triangles.Count; j += 6) {
                if (index[i] == triangles[j]) {
                    triangles.RemoveAt(j);
                    triangles.RemoveAt(j);
                    triangles.RemoveAt(j);
                    triangles.RemoveAt(j);
                    triangles.RemoveAt(j);
                    triangles.RemoveAt(j);

                    for (int k = 0; k < triangles.Count; k++) {
                        if (triangles[k] >= index[i]) {
                            triangles[k] -= 4;
                        }
                    }
                }
            }
        }

        mesh.triangles = triangles.ToArray();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        transform.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void AddFaces(Vector3Int position, List<Direction> directions) {
        //Debug.Log("Adding " + directions.Count + " faces to " + position.ToString());

        int x = position.x + (position.y * data.GetLength(0));
        int y = position.z;

        float offsetU = .5f * (1f / (data.GetLength(0) * data.GetLength(1)));
        float offsetV = .5f * (1f / data.GetLength(2));

        float u = ((float)x / (data.GetLength(0) * data.GetLength(1))) + offsetU;
        float v = ((float)y / data.GetLength(2)) + offsetV;

        //Debug.Log(u + ", " + v);

        foreach (Direction direction in directions) {
            Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
            List<Vector3> vertices = new List<Vector3>(mesh.vertices);
            List<int> triangles = new List<int>(mesh.triangles);
            List<Vector2> uvs = new List<Vector2>(mesh.uv);
            int numVerts = vertices.Count;

            switch (direction) {
                case Direction.posX:
                    vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0));
                    vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1));
                    vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1));
                    vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0));
                    break;

                case Direction.negX:
                    vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1));
                    vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0));
                    vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0));
                    vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1));
                    break;

                case Direction.posY:
                    vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0));
                    vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0));
                    vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1));
                    vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1));
                    break;

                case Direction.negY:
                    vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1));
                    vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1));
                    vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0));
                    vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0));
                    break;

                case Direction.posZ:
                    vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1));
                    vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1));
                    vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1));
                    vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1));
                    break;

                case Direction.negZ:
                    vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0));
                    vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0));
                    vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0));
                    vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0));
                    break;
            }

            uvs.Add(new Vector2(u, v));
            uvs.Add(new Vector2(u, v));
            uvs.Add(new Vector2(u, v));
            uvs.Add(new Vector2(u, v));

            triangles.Add(numVerts + 0);
            triangles.Add(numVerts + 2);
            triangles.Add(numVerts + 1);
            triangles.Add(numVerts + 0);
            triangles.Add(numVerts + 3);
            triangles.Add(numVerts + 2);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.RecalculateNormals();
            transform.GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 public void AddVoxel(HistoryEvent his) {
      
  
        List<Direction> facesToAdd = new List<Direction>() { Direction.posX, Direction.posY, Direction.posZ, Direction.negX, Direction.negY, Direction.negZ };
        List<int> facesToRemove = new List<int>();
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < vertices.Length; i += 4) {
            if ((vertices[i] == (his.pos)) && (normals[i] == Vector3.right)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(0, 1, 1))) && (normals[i + 2] == Vector3.right)) {
                    facesToAdd.Remove(Direction.negX);
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(1, 0, 1))) && (normals[i] == Vector3.left)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 1, 0))) && (normals[i + 2] == Vector3.left)) {
                    facesToAdd.Remove(Direction.posX);
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(1, 0, 0))) && (normals[i] == Vector3.forward)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(0, 1, 0))) && (normals[i + 2] == Vector3.forward)) {
                    facesToAdd.Remove(Direction.negZ);
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(0, 0, 1))) && (normals[i] == Vector3.back)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 1, 1))) && (normals[i + 2] == Vector3.back)) {
                    facesToAdd.Remove(Direction.posZ);
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos)) && (normals[i] == Vector3.up)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 0, 1))) && (normals[i + 2] == Vector3.up)) {
                    facesToAdd.Remove(Direction.negY);
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(0, 1, 1))) && (normals[i] == Vector3.down)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 1, 0))) && (normals[i + 2] == Vector3.down)) {
                    facesToAdd.Remove(Direction.posY);
                    facesToRemove.Add(i);
                }

            }

        }
        data[his.pos.x, his.pos.y, his.pos.z].color = his.color1;
        transform.GetComponent<TextureController>().SetPixel(his.pos, ColorController.primaryColor);

        RemoveFaces(facesToRemove);
        AddFaces(his.pos, facesToAdd);
    }

public void RemoveVoxel(HistoryEvent his) 
    {
        List<int> facesToRemove = new List<int>();
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < vertices.Length; i += 4) {
            if ((vertices[i] == (his.pos + new Vector3(1, 0, 0))) && (normals[i] == Vector3.right)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 1, 1))) && (normals[i + 2] == Vector3.right)) {
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(0, 0, 1))) && (normals[i] == Vector3.left)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(0, 1, 0))) && (normals[i + 2] == Vector3.left)) {
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(1, 0, 1))) && (normals[i] == Vector3.forward)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(0, 1, 1))) && (normals[i + 2] == Vector3.forward)) {
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos)) && (normals[i] == Vector3.back)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 1, 0))) && (normals[i + 2] == Vector3.back)) {
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(0, 1, 0))) && (normals[i] == Vector3.up)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 1, 1))) && (normals[i + 2] == Vector3.up)) {
                    facesToRemove.Add(i);
                }

            } else if ((vertices[i] == (his.pos + new Vector3(0, 0, 1))) && (normals[i] == Vector3.down)) {
                if ((vertices[i + 2] == (his.pos + new Vector3(1, 0, 0))) && (normals[i + 2] == Vector3.down)) {
                    facesToRemove.Add(i);
                }

            }
        }

        RemoveFaces(facesToRemove);
        data[his.pos.x, his.pos.y, his.pos.z].color = Color.clear;
        transform.GetComponent<TextureController>().SetPixel(his.pos, Color.clear);

        if (his.pos.x < (data.GetLength(0) - 1)) {
            if (data[his.pos.x + 1, his.pos.y, his.pos.z].color != Color.clear) {
                AddFaces(his.pos + Vector3Int.right, new List<Direction>() { Direction.negX });
            }
        }

        if (his.pos.y < (data.GetLength(1) - 1)) {
            if (data[his.pos.x, his.pos.y + 1, his.pos.z].color != Color.clear) {
                AddFaces(his.pos + Vector3Int.up, new List<Direction>() { Direction.negY });
            }
        }

        if (his.pos.z < (data.GetLength(2) - 1)) {
            if (data[his.pos.x, his.pos.y, his.pos.z + 1].color != Color.clear) {
                AddFaces(his.pos + new Vector3Int(0, 0, 1), new List<Direction>() { Direction.negZ });
            }
        }

        if (his.pos.x > 0) {
            if (data[his.pos.x - 1, his.pos.y, his.pos.z].color != Color.clear) {
                AddFaces(his.pos + Vector3Int.left, new List<Direction>() { Direction.posX });
            }
        }

        if (his.pos.y > 0) {
            if (data[his.pos.x, his.pos.y - 1, his.pos.z].color != Color.clear) {
                AddFaces(his.pos + Vector3Int.down, new List<Direction>() { Direction.posY });
            }
        }

        if (his.pos.z > 0) {
            if (data[his.pos.x, his.pos.y, his.pos.z - 1].color != Color.clear) {
                AddFaces(his.pos + new Vector3Int(0, 0, -1), new List<Direction>() { Direction.posZ });
            }
        }
    }
}
