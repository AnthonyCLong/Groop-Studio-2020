using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using SFB;

public class ExportObj : MonoBehaviour {
    public MeshController meshController;
    public TextureController textureController;
    public Variables variables;
    public HistoryController historyController;

    public void Save() 
    {
        
        string fileName, path, folder;

        //SAVE AS
        if (variables.currentpath == "" || variables.currentpath == "\\")
        {
            fileName = "Untitled_Voxel";
            path = StandaloneFileBrowser.SaveFilePanel("Save File", "Assets/Voxels", fileName, "obj");
            folder = Path.ChangeExtension(path, null);
            fileName = Path.GetFileName(path);
            path = folder + "\\" + fileName; 
            variables.currentpath = path;
        }
        //SAVE
        else
        {
            path = variables.currentpath;
            folder = Path.GetDirectoryName(Path.ChangeExtension(path, null));
        }
        
        //if we do save
        if(path != "\\")
        {   
           try
           {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }

            using (StreamWriter streamWriter = new StreamWriter(path)) {

                Mesh mesh = meshController.GetComponent<MeshFilter>().mesh;
                Material material = meshController.GetComponent<Renderer>().material;
                int[] triangles = mesh.triangles;
                StringBuilder stringBuilder = new StringBuilder();

                //Write Vertices
                stringBuilder.Append("g ").Append(mesh.name).Append("\n");
                foreach (Vector3 vertex in mesh.vertices) {
                    stringBuilder.Append(string.Format("v {0} {1} {2}\n", vertex.x, vertex.y, vertex.z));
                }

                //Write Normals
                stringBuilder.Append("\n");
                foreach (Vector3 normal in mesh.normals) {
                    stringBuilder.Append(string.Format("vn {0} {1} {2}\n", normal.x, normal.y, normal.z));
                }

                //Write UVs
                stringBuilder.Append("\n");
                foreach (Vector3 uv in mesh.uv) {
                    stringBuilder.Append(string.Format("vt {0} {1}\n", uv.x, uv.y));
                }

                //Write Triangles
                stringBuilder.Append("\n");
                for (int i = 0; i < triangles.Length; i += 3) {
                    stringBuilder.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
                }

                //Write Array Data
                stringBuilder.Append("\n");
                stringBuilder.Append(string.Format("arrdata {0} {1} {2}", meshController.data.GetLength(0), meshController.data.GetLength(1), meshController.data.GetLength(2)));
                stringBuilder.Append("\n");

                for (int z = 0; z < meshController.data.GetLength(2); z++) {
                    for (int y = 0; y < meshController.data.GetLength(1); y++) {
                        stringBuilder.Append(string.Format("arr "));
                        for (int x = 0; x < meshController.data.GetLength(0); x++) {
                            stringBuilder.Append(string.Format("{0},{1},{2},{3} ", meshController.data[x, y, z].color.r, meshController.data[x, y, z].color.g, meshController.data[x, y, z].color.b, meshController.data[x, y, z].color.a));
                        }
                        stringBuilder.Append(string.Format("\n"));
                    }
                }

                streamWriter.Write(stringBuilder.ToString());
                textureController.SaveTexture(Path.ChangeExtension(path, "png"));
                historyController.saveHistory(Path.ChangeExtension(path, "txt"));

            }
        }
    }



}
