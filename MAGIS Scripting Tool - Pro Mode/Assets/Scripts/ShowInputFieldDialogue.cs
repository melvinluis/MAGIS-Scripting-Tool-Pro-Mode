using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowInputFieldDialogue : MonoBehaviour {
    public Text message,
        actualText,
        placeholder;
    public InputField inputField;
    public Button button;
    public Image
        disabler,
        prompt; // self

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null, null);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowInputPrompt(string msg, string pldt, string txt, string btnlbl) {
        prompt.gameObject.SetActive(true);
        disabler.gameObject.SetActive(true);
        message.text = msg;
        placeholder.text = pldt;
        inputField.text = txt;
        button.transform.GetChild(0).GetComponent<Text>().text = btnlbl;
    }

    public void DisableInputPrompt() {
        prompt.gameObject.SetActive(false);
        disabler.gameObject.SetActive(false);
    }
}
