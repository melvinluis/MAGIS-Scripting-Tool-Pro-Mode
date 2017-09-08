using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInputFieldDialogue : MonoBehaviour {
    public Text message,
        actualText,
        placeholder;
    public InputField inputField;
    public Button button;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetButtonLabel(string name) {
        button.transform.GetChild(0).GetComponent<Text>().text = name;
    }

    public void SetPlaceholderText(string pldt) {
        placeholder.text = pldt;
    }

    public void SetActualText(string txt) {
        inputField.text = txt;
    }

    public void SetMessage(string msg) {
        message.text = msg;
    }
}
