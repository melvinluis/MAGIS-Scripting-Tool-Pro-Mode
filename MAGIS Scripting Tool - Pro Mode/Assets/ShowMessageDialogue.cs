using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessageDialogue : MonoBehaviour {

    public Image disabler; // Disabler used for pop-up messages (3rd layer)
    public Image prompt; // The dialogue box to be used

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
            disabler.gameObject.SetActive(true); // Disable everything behind the prompt
            prompt.gameObject.SetActive(true); // The dialogue box
            prompt.transform.GetChild(0).GetComponent<Text>().text = errormsg;
        }
        else {
            disabler.gameObject.SetActive(false);
            prompt.gameObject.SetActive(false);
        }
    }
}
