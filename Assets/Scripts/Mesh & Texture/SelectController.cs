using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;



public class SelectController : MonoBehaviour {
    public PointerController pointerController;
    public ColorController colorController;

    public string selectState;
    public List<Vector3Int> selectData = new List<Vector3Int>();

    public void Awake() {
        GetComponent<MeshFilter>().mesh = new Mesh();
    }

    public void Update() {
        if (selectState == "selecting")
            Select(pointerController.position);
        else if (selectState == "deselecting")
            Deselect(pointerController.position);
    }

    //SELECTING
    public void StartSelect(Vector3Int position) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();
        if(((position.x >= 0)&&(position.x < meshController.data.GetLength(0)))  &&  ((position.y >= 0)&&(position.y < meshController.data.GetLength(1)))  &&  ((position.z >= 0)&&(position.z < meshController.data.GetLength(2)))  &&  (meshController.data[position.x, position.y, position.z].color != Color.clear))
        {
            if (meshController.data[position.x, position.y, position.z].selected == false)
                selectState = "selecting";
            else
                selectState = "deselecting";
        }
    }

    public void StopSelect() {
        selectState = "";
    }

    public void Select(Vector3Int position) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();
        if (((position.x >= 0)&&(position.x < meshController.data.GetLength(0)))  &&  ((position.y >= 0)&&(position.y < meshController.data.GetLength(1)))  &&  ((position.z >= 0)&&(position.z < meshController.data.GetLength(2)))  &&  (meshController.data[position.x, position.y, position.z].color != Color.clear))
        {
            if (meshController.data[position.x, position.y, position.z].selected == false) 
            {
                selectData.Add(new Vector3Int(position.x, position.y, position.z));
                AddVoxel(position);
                meshController.data[position.x, position.y, position.z].selected = true;
            }
        }
    }

    public void SelectColor(Vector3Int pos) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();

        for (int x = 0; x < meshController.data.GetLength(0); x++) 
        {
            for (int y = 0; y < meshController.data.GetLength(1); y++) 
            {
               for (int z = 0; z < meshController.data.GetLength(2); z++) 
                {
                    if ( meshController.data[x, y, z].color == meshController.data[pos.x, pos.y, pos.z].color)
                    Select(new Vector3Int(x, y, z));    
                }    
            }    
        }
    }
    public void Selectlayer() 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();
        Clear();
        //hardcoded
        int x = -1;
        int y = -1;
        int z = 0;

        if(x == -1 && z == -1)
        {
            for (int i = 0; i < meshController.data.GetLength(0); i++)
            {
                for (int j = 0; j < meshController.data.GetLength(2); j++)
                {
                    if (meshController.data[i, y, j].selected == false) 
                    {
                        selectData.Add(new Vector3Int(i,y,j));
                        AddVoxel(new Vector3Int(i,y,j));
                        meshController.data[i, y, j].selected = true;
                    }

                }
            }
        }
        
        else if(y == -1 && z == -1)
        {
            for (int i = 0; i < meshController.data.GetLength(1); i++)
            {
                for (int j = 0; j < meshController.data.GetLength(2); j++)
                {
                    if (meshController.data[x, i, j].selected == false) 
                    {
                        selectData.Add(new Vector3Int(x,i,j));
                        AddVoxel(new Vector3Int(x,i,j));
                        meshController.data[x, i, j].selected = true;
                    }

                }
            }
        }
        
        else if(x == -1 && y == -1)
        {
            for (int i = 0; i < meshController.data.GetLength(0); i++)
            {
                for (int j = 0; j < meshController.data.GetLength(1); j++)
                {
                    if (meshController.data[i, j, z].selected == false) 
                    {
                        selectData.Add(new Vector3Int(i,j,z));
                        AddVoxel(new Vector3Int(i,j,z));
                        meshController.data[i, j, z].selected = true;
                    }

                }
            }
        }
        else Debug.Log("Error occured");
    }
    
    public void Deselect(Vector3Int position) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();

        if (((position.x >= 0)&&(position.x < meshController.data.GetLength(0)))  &&  ((position.y >= 0)&&(position.y < meshController.data.GetLength(1)))  &&  ((position.z >= 0)&&(position.z < meshController.data.GetLength(2)))  &&  (meshController.data[position.x, position.y, position.z].color != Color.clear))
        {
            if (meshController.data[position.x, position.y, position.z].selected == true) 
            {
                selectData.Remove(new Vector3Int(position.x, position.y, position.z));
                RemoveVoxel(position);
                meshController.data[position.x, position.y, position.z].selected = false;
            }
        }
    }

    public void Clear() 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();

        foreach (Vector3Int pos in selectData) {
            meshController.data[pos.x, pos.y, pos.z].selected = false;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        selectData.Clear();
    }

    public void pickColor(Vector3Int pos) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();

        if (((pos.x >= 0)&&(pos.x < meshController.data.GetLength(0)))  &&  ((pos.y >= 0)&&(pos.y < meshController.data.GetLength(1)))  &&  ((pos.z >= 0)&&(pos.z < meshController.data.GetLength(2)))  &&  (meshController.data[pos.x, pos.y, pos.z].color != ColorController.primaryColor))
        {
            ColorController.primaryColor = meshController.data[pos.x, pos.y, pos.z].color;
            Color.RGBToHSV(ColorController.primaryColor, out float h, out float s, out float v);
            colorController.SetRingCursor(h);
            colorController.SetSquareCursor(s, v);
        }
    }

        public void brushColor(Vector3Int pos) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();
        TextureController textureController = transform.GetComponentInParent<TextureController>();
        HistoryController historyController = transform.GetComponentInParent<HistoryController>();

        if (((pos.x >= 0)&&(pos.x < meshController.data.GetLength(0)))  &&  ((pos.y >= 0)&&(pos.y < meshController.data.GetLength(1)))  &&  ((pos.z >= 0)&&(pos.z < meshController.data.GetLength(2)))  &&  (meshController.data[pos.x, pos.y, pos.z].color != ColorController.primaryColor))
            {
                if (historyController.Actions.Exists(x => x.pos == new Vector3Int(pos.x, pos.y, pos.z)))
                     {
                        int index = historyController.Actions.FindIndex(x => x.pos == new Vector3Int(pos.x, pos.y, pos.z));
                        historyController.Actions[index] = new HistoryEvent(AT.BSH, pos, historyController.Actions[index].color1, ColorController.primaryColor);
                     }
                else
                    historyController.AddToList(new HistoryEvent(AT.BSH, pos, meshController.data[pos.x, pos.y, pos.z].color, ColorController.primaryColor));
                
                meshController.data[pos.x, pos.y, pos.z].color = ColorController.primaryColor;
                textureController.SetPixel(pos, ColorController.primaryColor);
            }
    }
    
    public void brushColor(HistoryEvent his, bool undoRedo) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();
        TextureController textureController = transform.GetComponentInParent<TextureController>();

        if (((his.pos.x >= 0)&&(his.pos.x < meshController.data.GetLength(0)))  &&  ((his.pos.y >= 0)&&(his.pos.y < meshController.data.GetLength(1)))  &&  ((his.pos.z >= 0)&&(his.pos.z < meshController.data.GetLength(2))))
            {
                Color set;

                if (undoRedo)
                    set = his.color2;
                else
                    set = his.color1;
                meshController.data[his.pos.x, his.pos.y, his.pos.z].color = set;
                textureController.SetPixel(his.pos, set);
            }
    }




//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //MODIFIERS
    public void colorSelection(Color color) {
        MeshController meshController = transform.GetComponentInParent<MeshController>();
        TextureController textureController = transform.GetComponentInParent<TextureController>();

        foreach (Vector3Int pos in selectData) {
            if(meshController.data[pos.x, pos.y, pos.z].color != Color.clear)
            {
                brushColor(pos);
            }
        }
    }

    public void Add(Vector3Int normal, bool updateSelection = false) {
        MeshController meshController = transform.GetComponentInParent<MeshController>();

        if (selectData.Count > 0) {
            if (updateSelection) {
                List<Vector3Int> tmp = new List<Vector3Int>();
                foreach (Vector3Int item in selectData)
                    tmp.Add(item);

                foreach (Vector3Int pos in tmp) {
                    meshController.AddVoxel(pos + normal);
                    Deselect(pos);
                    Select(pos + normal);
                }

            } else {
                foreach (Vector3Int pos in selectData) {
                    meshController.AddVoxel(pos);
                }
            }
        }
    }

    public void Remove() {
        MeshController meshController = transform.GetComponentInParent<MeshController>();

        if (selectData.Count > 0) {
            foreach (Vector3Int pos in selectData) {
                meshController.data[pos.x, pos.y, pos.z].selected = false;
                meshController.RemoveVoxel(pos);
                RemoveVoxel(pos);
            }

            selectData.Clear();
        }
    }




    //VISUALS
    public void RemoveVoxel(Vector3Int position) 
    {
        MeshController meshController = transform.GetComponentInParent<MeshController>();
        ArrayData[,,] data = meshController.data;

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
        data[position.x, position.y, position.z].selected = false;

        if (position.x < (data.GetLength(0) - 1)) {
            if (data[position.x + 1, position.y, position.z].selected != false) {
                AddFaces(position + Vector3Int.right, new List<Direction>() { Direction.negX });
            }
        }

        if (position.y < (data.GetLength(1) - 1)) {
            if (data[position.x, position.y + 1, position.z].selected != false) {
                AddFaces(position + Vector3Int.up, new List<Direction>() { Direction.negY });
            }
        }

        if (position.z < (data.GetLength(2) - 1)) {
            if (data[position.x, position.y, position.z + 1].selected != false) {
                AddFaces(position + new Vector3Int(0, 0, 1), new List<Direction>() { Direction.negZ });
            }
        }

        if (position.x > 0) {
            if (data[position.x - 1, position.y, position.z].selected != false) {
                AddFaces(position + Vector3Int.left, new List<Direction>() { Direction.posX });
            }
        }

        if (position.y > 0) {
            if (data[position.x, position.y - 1, position.z].selected != false) {
                AddFaces(position + Vector3Int.down, new List<Direction>() { Direction.posY });
            }
        }

        if (position.z > 0) {
            if (data[position.x, position.y, position.z - 1].selected != false) {
                AddFaces(position + new Vector3Int(0, 0, -1), new List<Direction>() { Direction.posZ });
            }
        }
    }

    public void AddVoxel(Vector3Int position) {

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

        RemoveFaces(facesToRemove);
        AddFaces(position, facesToAdd);
    }

    public void RemoveFaces(List<int> index) {
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);

        for (int i = 0; i < index.Count; i++) {
            vertices.RemoveAt(index[i]);
            vertices.RemoveAt(index[i]);
            vertices.RemoveAt(index[i]);
            vertices.RemoveAt(index[i]);

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

        mesh.RecalculateNormals();
    }

    public void AddFaces(Vector3Int position, List<Direction> directions) {

        foreach (Direction direction in directions) {
            Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
            List<Vector3> vertices = new List<Vector3>(mesh.vertices);
            List<int> triangles = new List<int>(mesh.triangles);
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

            triangles.Add(numVerts + 0);
            triangles.Add(numVerts + 2);
            triangles.Add(numVerts + 1);
            triangles.Add(numVerts + 0);
            triangles.Add(numVerts + 3);
            triangles.Add(numVerts + 2);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
        }
    }
}
