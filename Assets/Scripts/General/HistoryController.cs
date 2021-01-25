using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;




public enum AT
{
    ADD,
    RMV,
    BSH

}

public struct HistoryEvent 
{
    public AT actiontype;
    public Vector3Int pos;
    public Color color1;
    public Color color2;
    public HistoryEvent(AT actionType,Vector3Int post, Color clr1, Color clr2){actiontype = actionType; pos = post; color1 = clr1; color2 = clr2;}
}



public class HistoryController : MonoBehaviour {
    public SelectController selectController;
    
    public int linenumber;
    public bool initialcreation = true;
    public string filePath;
    public List<HistoryEvent> recentEvent = new List<HistoryEvent>();
    public List<HistoryEvent> Actions = new List<HistoryEvent>();

    public void Awake()
    {
        createHistory();
    }
    
  public string TOString(List<HistoryEvent> historyevents)
   {
        string returnString ="";
        for(int i = 0; i < historyevents.Count(); i++)
        {
            string color1 = historyevents[i].color1.ToString();
            color1 = color1.Substring(4);

            string color2;
            if(historyevents[i].color2 == Color.clear)
            {
                color2 = "NULL";
            }

            else
            {
                color2 = historyevents[i].color2.ToString();
                color2 = color2.Substring(4);
            }
            
            returnString += "{" + Enum.GetName(typeof(AT), historyevents[i].actiontype) + " " + historyevents[i].pos + " " + color1 + " " + color2  + "}";
       }
       return returnString;
   }

   public List<HistoryEvent> FROMString(string str)
   {
        //Debug.Log(str.Count());
        string[] items = str.Split('}');
        List<HistoryEvent> returnstring = new List<HistoryEvent>();
        for (int i = 0; i < items.Count()-1; i++)
        {
            items[i] = items[i] + "}";
           // Debug.Log(i + ": " + items[i]);
            returnstring.Add(subitem(items[i]));
        }
        
        return returnstring;
   }

      HistoryEvent subitem(string str)
   {
        string[] items = str.Split(' ');
        
        items[0] = items[0].Substring(1);
        items[1] = items[1].Replace("(", string.Empty).Replace(",", string.Empty);
        items[2] = items[2].Replace(",", string.Empty);
        items[3] = items[3].Replace(")", string.Empty);
        items[4] = items[4].Substring(1,5);
        items[5] = items[5].Substring(0,5);
        items[6] = items[6].Substring(0,5);
        items[7] = items[7].Substring(0,5);
        
        if(items[8] != "NULL}")
            {    
                items[8] = items[8].Substring(1,5);
                items[9] = items[9].Substring(0,5);
                items[10] = items[10].Substring(0,5);
                items[11] = items[11].Substring(0,5);
            }
        else
            {
                string[] items2 = items;
                items = new string[12];
                Array.Copy(items2, items, 8);

                items[8] = "0.000";
                items[9] = "0.000";
                items[10] = "0.000";
                items[11] = "0.000";
            }

       return new HistoryEvent( (AT) Enum.Parse(typeof(AT), items[0]), new Vector3Int(int.Parse(items[1]), int.Parse(items[2]), int.Parse(items[3])) , new Color(float.Parse(items[4]), float.Parse(items[5]), float.Parse(items[6]), float.Parse(items[7])) , new Color(float.Parse(items[8]), float.Parse(items[9]), float.Parse(items[10]), float.Parse(items[11])));
   }
   
    public void createHistory() 
    {
        linenumber= -1;
        filePath = Path.GetTempFileName();
        File.Create(filePath).Close();
         //Debug.Log(filePath);
    }

     public void saveHistory(string path) 
    {
        if(!(File.Exists(path)))
        {
            File.Create(path).Close();
        }
        
        File.Copy (filePath, path, true);
        initialcreation = false;
    }
    
    public void loadHistory(string path) 
    {
        if(File.Exists(path))
        {   
            initialcreation = false;
            File.Delete(filePath);

            var transfer = File.ReadAllLines(path);
            recentEvent = FROMString(transfer.Last());
            linenumber = (transfer.Count()-1);
            
            filePath = Path.GetTempFileName();
            File.WriteAllLines(filePath, transfer);

        }
        else
        {
            initialcreation = false;
            File.Delete(filePath);
            createHistory();

        }

    }
     
    public void AddToList(HistoryEvent Action) 
    {
        Actions.Add(Action);       
    }

    public void recordAction() 
    {
        if (Actions.Any())
       { 
            linenumber++;  
        
            if(linenumber != File.ReadLines(filePath).Count())
            {
                var tempFile = Path.GetTempFileName();
                File.Create(tempFile).Dispose();
                IEnumerable<string> linesToKeep;
                if(linenumber == -1)
                    linesToKeep = File.ReadAllLines(filePath).Skip(1).Take(0);
                else
                    linesToKeep = File.ReadAllLines(filePath).Skip(0).Take(linenumber);
                
                File.WriteAllLines(tempFile, linesToKeep);
                File.Delete(filePath);
                File.Copy(tempFile, filePath);
                filePath = tempFile;
            }
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath,true))
            {
                file.WriteLine(TOString(Actions));
                file.Close();
            } 
            
            Actions.Clear(); 
            recentEvent = FROMString(File.ReadAllLines(filePath).Skip(linenumber).Take(1).First());;
        }   
    }


     public void undo() 
    {
        MeshController meshController = transform.GetComponent<MeshController>();
        if(linenumber>=0) 
        {  
           for (int i = 0; i < recentEvent.Count(); i++)
            {
                switch (recentEvent[i].actiontype)
                {
                    case((AT)0): meshController.RemoveVoxel(recentEvent[i]);
                    break;
                    
                    case((AT)1): meshController.AddVoxel(recentEvent[i]);
                    break;

                    case((AT)2): selectController.brushColor(recentEvent[i], false);
                    break;
                    
                    default: Debug.Log("how did we get here?");
                    break;

                }
        }
            
            recentEvent = FROMString(File.ReadAllLines(filePath).Skip(linenumber-1).Take(1).First());
            linenumber--;  
        }
        
    }
     public void redo() 
    {
        MeshController meshController = transform.GetComponent<MeshController>();

        if (linenumber +1 < File.ReadLines(filePath).Count()) 
        {  
            linenumber++;
            recentEvent = FROMString(File.ReadLines(filePath).Skip(linenumber).Take(1).First()); 
            
            for (int i = 0; i < recentEvent.Count(); i++)
            {
                switch (recentEvent[i].actiontype)
                {
                    case((AT)0): meshController.AddVoxel(recentEvent[i]);
                    break;
                    
                    case((AT)1): meshController.RemoveVoxel(recentEvent[i]);
                    break;

                    case((AT)2): selectController.brushColor(recentEvent[i], true);
                    break;
                    
                    default: Debug.Log("how did we get here?");
                    break;
                }
            }

        }
    }
}
