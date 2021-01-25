using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ShortcutController : MonoBehaviour {
    [Serializable]
    public class Shortcut {
        public KeyCode keycode;
        public bool shiftCombo;
        public UnityEvent actions;
    }

    public List<Shortcut> Shortcuts;

    public void Update() {
        foreach (Shortcut shortcut in Shortcuts) {
            if (shortcut.shiftCombo) {
                if (Input.GetKeyDown(shortcut.keycode) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
                    shortcut.actions.Invoke();
                }

            } else {
                if (Input.GetKeyDown(shortcut.keycode)) {
                    shortcut.actions.Invoke();
                }
            }
        }
    }
}
