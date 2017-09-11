using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowMessageDialogue : MonoBehaviour {

    public Image disabler; // Disabler used for pop-up messages (3rd layer)
    public Image prompt; // The dialogue box to be used

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null, null);
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RaiseError(string errormsg) {
        /* Raises an error using the dialogue box
         * */
        if (!prompt.gameObject.activeInHierarchy) {
            disabler.gameObject.SetActive(true);
            prompt.gameObject.SetActive(true); // The dialogue box
            prompt.transform.GetChild(0).GetComponent<Text>().text = errormsg;
        }
        else {
            disabler.gameObject.SetActive(false);
            prompt.gameObject.SetActive(false);
        }
    }
}
