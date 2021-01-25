using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioReference : MonoBehaviour {
    public CustomButton defaultRadio;
    public List<CustomButton> radios = new List<CustomButton>();

    public void Start() {
        if (defaultRadio)
            defaultRadio.Select();
    }

    public void Add(CustomButton button) {
        radios.Add(button);
    }

    public void Next() {
        foreach (CustomButton radio in radios) {
            if (radio.Selected()) {
                //Debug.Log(radio.name + " was selected");
                int index = radios.IndexOf(radio) + 1;

                if (index == radios.Count) {
                    index = 0;
                }

                //Debug.Log(radios[index].name + " is now selected");
                radios[index].Select();

                break;
            }
        }
    }
}
