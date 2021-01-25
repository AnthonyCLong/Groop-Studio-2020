using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Popup : MonoBehaviour, IPointerExitHandler {

    public void OnPointerExit(PointerEventData pointerEventData) {
        CustomButton[] buttons = transform.GetComponentsInChildren<CustomButton>();

        foreach (CustomButton button in buttons) {
            button.StopHovering();
        }

        transform.gameObject.SetActive(false);
    }
}
