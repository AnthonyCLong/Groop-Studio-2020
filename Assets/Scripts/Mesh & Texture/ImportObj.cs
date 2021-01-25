using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using SFB;
public class ImportObj : MonoBehaviour {
    public MeshController meshController;
    public TextureController textureController;
    public HistoryController historyController;
    public Variables variables;
    public CanvasController canvas;

    public void Load() 
    {
       
        var check = StandaloneFileBrowser.OpenFolderPanel("", "Voxels", false);//StandaloneFileBrowser.OpenFilePanel("Open File", "Voxels", "obj", false)[0];
        
        if(check.Length != 0)
        {    
            var path = check[0];
            path = path + "\\" + Path.GetFileNameWithoutExtension(path);

            var obj = Path.ChangeExtension(path, "obj");
            var png = Path.ChangeExtension(path, "png");
            //var txt = Path.ChangeExtension(path, "txt");
            // Debug.Log(Path.GetFileNameWithoutExtension(path));
            
            if(File.Exists(obj) && File.Exists(png))
            {
                using (StreamReader streamReader = File.OpenText(obj)) {

                    string name = string.Empty;
                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector3> normals = new List<Vector3>();
                    List<Vector2> uvs = new List<Vector2>();
                    List<int> triangles = new List<int>();
                    ArrayData[,,] data = null;
                    string line = string.Empty;
                    int data_y = -1;
                    int data_z = 0;
                    int data_x = 0;

                    while ((line = streamReader.ReadLine()) != null) {
                        string[] items = line.Split(null);

                        //Get Name
                        if (items[0] == "g") {
                            name = items[1];
                        }

                        //Get Vertices
                        else if (items[0] == "v") {
                            float x = float.Parse(items[1], NumberStyles.Float);
                            float y = float.Parse(items[2], NumberStyles.Float);
                            float z = float.Parse(items[3], NumberStyles.Float);
                            vertices.Add(new Vector3(x, y, z));
                        }

                        //Get Normals
                        else if (items[0] == "vn") {
                            float x = float.Parse(items[1], NumberStyles.Float);
                            float y = float.Parse(items[2], NumberStyles.Float);
                            float z = float.Parse(items[3], NumberStyles.Float);
                            normals.Add(new Vector3(x, y, z));
                        }
                        
                        //Get Uvs
                        else if (items[0] == "vt") {
                            float u = float.Parse(items[1], NumberStyles.Float);
                            float v = float.Parse(items[2], NumberStyles.Float);
                            uvs.Add(new Vector2(u, v));
                        }
                        
                        //Get Triangles
                        else if (items[0] == "f") {
                            int vertex1 = int.Parse(items[1].Split('/')[0]) - 1;
                            int vertex2 = int.Parse(items[2].Split('/')[0]) - 1;
                            int vertex3 = int.Parse(items[3].Split('/')[0]) - 1;
                            triangles.Add(vertex1);
                            triangles.Add(vertex2);
                            triangles.Add(vertex3);
                        }

                        //Get Array Data
                        else if (items[0] == "arrdata") {
                            data = new ArrayData[int.Parse(items[1]), int.Parse(items[2]), int.Parse(items[3])];
                        }

                        else if (items[0] == "arr") {
                            data_y++;
                            
                            if (data_y == data.GetLength(1)) {
                                data_y = 0;
                                data_z++;
                            }

                            for (data_x = 1; data_x < items.Length - 1; data_x++) {
                                int x = data_x - 1;
                                string[] colors = items[data_x].Split(',');
                                float[] colorfloats = new float[colors.Length];
                                for(int j = 0; j < colorfloats.Length; j++)
                                    colorfloats[j] = float.Parse(colors[j]);                            
                                Color color = new Color(colorfloats[0],colorfloats[1],colorfloats[2],colorfloats[3]);
                                data[x, data_y, data_z].color = color;
                            }
                        }
                    }

                    Mesh mesh = new Mesh();
                    mesh.name = name;
                    mesh.vertices = vertices.ToArray();
                    mesh.normals = normals.ToArray();
                    mesh.uv = uvs.ToArray();
                    mesh.triangles = triangles.ToArray();

                    canvas.Create();

                    meshController.data = data;
                    meshController.transform.GetComponent<MeshFilter>().mesh = mesh;
                    meshController.transform.GetComponent<MeshCollider>().sharedMesh = mesh;

                    canvas.Change(meshController.gameObject);
                }
                
                textureController.loadTexture(Path.ChangeExtension(path, "png"));
                historyController.loadHistory(Path.ChangeExtension(path, "txt"));
                variables.currentpath = Path.ChangeExtension(path, "obj");
            } 
            else Debug.Log("ERROR: FOLDER DOES NOT CONTAIN OBJ, PNG, AND JSON FILES");
        }
    }
    

}