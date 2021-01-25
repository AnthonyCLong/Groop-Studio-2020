using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    //Button Types
    [Serializable]
    public enum ButtonType {
        Normal,
        Toggle,
        Radio
    }

    //Display Options
    [Serializable]
    public class Display {
        public Image background;
        public Sprite defaultSprite;
        public Sprite hoverSprite;
        public Sprite selectSprite;
        public GameObject toolTip;
    }

    [Serializable]
    public class Option {
        public RadioReference radioReference;
        public bool startToggled;
    }

    public ButtonType buttonType;
    public Display displaySettings = new Display();
    public Option buttonOptions = new Option();
    public UnityEvent onLeftClick;
    public UnityEvent onRightClick;
    public UnityEvent onDeselect;

    private bool hovering = false;
    private bool selected = false;
    private bool showTooltip = false;

    public void Start() {
        if (buttonOptions.startToggled) {
            Select();
        }
    }

    //When pointer enters
    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (pointerEventData.pointerEnter.gameObject == gameObject) {
            hovering = true;
            UpdateColors();
            ShowToolTip();
        }
    }

    //When pointer exits
    public void OnPointerExit(PointerEventData pointerEventData) {
        StopHovering();
    }

    //When pointer clicks
    public void OnPointerClick(PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
            Select();
        if (pointerEventData.button == PointerEventData.InputButton.Right)
            RightClick();
    }

    //Select
    public void Select() {
        HideToolTip();

        if (buttonType == ButtonType.Normal) {
            LeftClick();

        } else if (buttonType == ButtonType.Toggle) {
            if (selected) {
                Deselect();

            } else {
                if (buttonOptions.radioReference) {
                    foreach (CustomButton radio in buttonOptions.radioReference.radios) {
                        radio.Deselect();
                    }
                }

                selected = true;
                LeftClick();
            }

        } else if (buttonType == ButtonType.Radio) {
            if (!selected) {
                foreach (CustomButton radio in buttonOptions.radioReference.radios) {
                    radio.Deselect();
                }

                selected = true;
                LeftClick();
            }
        }

        UpdateColors();
    }

    //Deselect
    public void Deselect() {
        HideToolTip();
        selected = false;
        UpdateColors();
        onDeselect.Invoke();
    }

    public void LeftClick() {
        onLeftClick.Invoke();
    }

    public void RightClick() {
        HideToolTip();
        onRightClick.Invoke();
    }

    public void UpdateColors() {
        if (selected) {
            if (hovering) {
                displaySettings.background.sprite = displaySettings.hoverSprite;
            } else {
                displaySettings.background.sprite = displaySettings.selectSprite;
            }
        } else {
            if (hovering) {
                displaySettings.background.sprite = displaySettings.hoverSprite;
            } else {
                displaySettings.background.sprite = displaySettings.defaultSprite;
            }
        }
    }

    public void Swap(CustomButton button) 
    {
        onLeftClick = button.onLeftClick;
        Select();
    }

    IEnumerator ToolTipCounter() {
        yield return new WaitForSeconds(.5f);
        
        if (showTooltip) {
            displaySettings.toolTip.SetActive(true);
        }
    }

    public void ShowToolTip() {
        if (displaySettings.toolTip) {
            showTooltip = true;
            StartCoroutine(ToolTipCounter());
        }
    }

    public void HideToolTip() {
        if (displaySettings.toolTip) {
            showTooltip = false;
            displaySettings.toolTip.SetActive(false);
        }
    }

    public void StopHovering() {
        hovering = false;
        UpdateColors();
        HideToolTip();
    }

    public bool Selected() {
        return selected;
    }
}
