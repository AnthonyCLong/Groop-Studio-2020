using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralScripts : MonoBehaviour {
    public void ToggleActive(GameObject obj) {
        if (obj.activeSelf)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }
}
