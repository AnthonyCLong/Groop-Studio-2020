using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolController : MonoBehaviour {
    public string currentTool;
    public List<CustomButton> tools;

    public void SetCurrentTool(string tool) {
        DeselectAll();
        currentTool = tool;
    }

    private void DeselectAll() {
        foreach (CustomButton tool in tools)
            tool.Deselect();
    }
}
