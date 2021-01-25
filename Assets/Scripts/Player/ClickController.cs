using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickController : MonoBehaviour {
    public CameraController cameraScript;
    public MeshController meshScript;
    public ToolController toolController;
    public PointerController pointerScript;
    public SelectController selectController;
    public ColorController colorcontroller;
    public HistoryController historyController;
    public ExtrudeController extrudeController;

    public bool leftMouseDown = false;
    public List<Vector3Int> pos = new List<Vector3Int>();


    public void Update() 
    {
        
        //WHEN NOT OVER UI
        if (!EventSystem.current.IsPointerOverGameObject()) 
        {
            //CAMERA ROTATE
            if (Input.GetMouseButton(1)) 
            {
                cameraScript.Orbit();
            }

            //CAMERA PAN
            if (Input.GetMouseButton(2)) {
                cameraScript.Pan();
            }

            //SELECTED HANDLER
            if (Input.GetMouseButtonDown(0)) 
            {
                if (selectController.selectData.Count > 0) 
                {
                    if (toolController.currentTool == "pen" && selectController.selectData.Contains(pointerScript.normalPosition)) 
                    {
                        selectController.Add(pointerScript.normal);
                    }
                        
                    else if (toolController.currentTool == "eraser" && selectController.selectData.Contains(pointerScript.position)) 
                    {
                        selectController.Remove();
                    }
                } 
                    
                //TOOLS WHEN THERE IS NO SELECTION, NON DRAG
                else 
                {
                    if (toolController.currentTool == "pen") 
                    {
                        pointerScript.previousPosition = pointerScript.normalPosition;
                        meshScript.AddVoxel(pointerScript.normalPosition);
                    } 
                        
                    else if (toolController.currentTool == "eraser") 
                    {
                        pointerScript.previousPosition = pointerScript.position;
                        meshScript.RemoveVoxel(pointerScript.position);
                    }
                        
                    else if (toolController.currentTool == "picker") 
                    {
                        pointerScript.previousPosition = pointerScript.position;
                        selectController.pickColor(pointerScript.position);
                    }
                        
                    else if (toolController.currentTool == "brush") 
                    {
                        pointerScript.previousPosition = pointerScript.position;
                        selectController.brushColor(pointerScript.position);
                    }

                    else if (toolController.currentTool == "colorselecter")
                    {
                        selectController.SelectColor (pointerScript.position);
                    }
                }

                if (toolController.currentTool == "defaultselecter") {
                    selectController.StartSelect(pointerScript.position);

                } else if (toolController.currentTool == "extrude") {
                    extrudeController.StartExtrude();
                }
                


                // else if (toolController.currentTool == "layerselecter")
                //     selectController.Selectlayer();
            }

            if (Input.GetMouseButtonUp(0)) 
            {
                if (toolController.currentTool == "defaultselecter") {
                    selectController.StopSelect();

                } else if (toolController.currentTool == "extrude") {
                    extrudeController.StopExtrude();
                }

                //    if(toolController.currentTool == "pen" || toolController.currentTool == "eraser")
                //     {
                //         historyController.recordAction();        
                //     }

                //     else if (toolController.currentTool == "clearer")
                //     {
                //         historyController.recordAction();   
                //     }

                historyController.recordAction();                     
            }

            if (Input.GetMouseButton(0)) 
            {
                //leftMouseDown = true;
                if (toolController.currentTool == "pen") 
                {
                    if (pointerScript.previousPosition != pointerScript.position) 
                    {
                        pointerScript.previousPosition = pointerScript.normalPosition;
                        meshScript.AddVoxel(pointerScript.normalPosition);                        
                    }
                }
            
                else if (toolController.currentTool == "brush") 
                {
                    if (pointerScript.previousPosition != pointerScript.position) 
                    {
                        pointerScript.previousPosition = pointerScript.position;
                        selectController.brushColor(pointerScript.position);
                    }
                }

                //issues with this rn
                else if (toolController.currentTool == "eraser") 
                {
                   if (pointerScript.previousPosition != pointerScript.normalPosition) 
                   {
                        pointerScript.previousPosition = pointerScript.position;
                        meshScript.RemoveVoxel(pointerScript.position);
                   }

                }
                
                else if (toolController.currentTool == "picker") 
                {
                    if (pointerScript.previousPosition != pointerScript.position) 
                    {
                        pointerScript.previousPosition = pointerScript.position;
                        selectController.pickColor(pointerScript.position);
                    }
                }

            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0) 
            {
                cameraScript.Zoom();
            }
        
        }
        
        else if (Input.GetMouseButtonUp(0)) 
            {
                historyController.recordAction();
            }
    }

    /*
        public VoxelController voxelController;
        public CameraController cameraController;
        public CursorController cursorController;
        public ColorController colorController;
        public PointerController pointerController;
        public ToolController toolController;
        public string hitTag;

        public void Update() {

            //Camera Inputs
            if (Input.GetMouseButton(1)) {
                cursorController.SetRotate();
                cameraController.Orbit();
            }

            if (Input.GetMouseButton(2)) {
                cursorController.SetPan();
                cameraController.Pan();
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0) {
                cameraController.Zoom();
            }

            if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) {
                cursorController.SetTool();
            }

            //Editing Inputs
            if (Input.GetMouseButtonDown(0)) {

                switch (toolController.currentTool) {
                    case ToolController.Tool.Pencil:
                        if (pointerController.hitObject.tag == "Grid")
                            voxelController.SetVoxel(pointerController.position, color: colorController.primaryColor);
                        else if (pointerController.hitObject.tag == "Voxel")
                            voxelController.SetVoxel(pointerController.position,pointerController.normal, colorController.primaryColor);
                        break;
                    case ToolController.Tool.Eraser:
                        voxelController.EraseVoxel(pointerController.hitObject);
                        break;
                    case ToolController.Tool.Picker:
                        colorController.primaryColor = pointerController.color;
                        break;
                    case ToolController.Tool.Paintbrush:
                        if (pointerController.hitObject.tag == "Voxel")
                            pointerController.hitObject.GetComponent<MeshRenderer>().material.color = colorController.primaryColor;
                        break;
                }
            }
        }
    */
}