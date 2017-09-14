using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowDecisionPrompt : MonoBehaviour {

    public Text message;
    public Button yes, no;

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null, null);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetMessage(string msg) {
        message.text = msg;
    }
}
