using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour {

    public InputField f_inputfield; // Name input when adding flags
    public RectTransform f_management; // Flag management panel
    public Image disabler; // Disables buttons behind popup panel
    public Image prompt; // The object used for error messages

    private ArrayList flags = new ArrayList(); // Store all flags here

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public ArrayList GetFlags() {
        return flags;
    }

    public void AddFlagDialogue() {
        /* Enables the text box and add button when adding new flags 
         */
        f_inputfield.gameObject.SetActive(true);
    }

    bool CheckFlagName(string name) {
        /* Checks whether the submitted flag name is alphanumeric
         * Note that this also allows characters like åäö
         * */
        for(int i = 0; i < name.Length; i++) {
            if (!char.IsLetterOrDigit(name[i])) {
                prompt.GetComponent<ShowMessageDialogue>().RaiseError("Invalid flag name");
                return false;
            }
        }
        return true;
    }

    public void ManageFlags() {
        /* Displays the flags for users to manage.
         * */
        if (!f_management.gameObject.activeInHierarchy) {
            disabler.gameObject.SetActive(true);
            f_management.gameObject.SetActive(true);

            // TODO: figure out how to display all flags and all related flag management functions
            for (int i = 0; i < flags.Count; i++) {
                Debug.Log(flags[i]);
            }
        }
        else {
            disabler.gameObject.SetActive(false);
            f_management.gameObject.SetActive(false);
        }
    }

    public void AddFlag() {
        /* Adds a new flag */
        string newflag = f_inputfield.text;
        // check if the string is valid
        if (CheckFlagName(newflag)) {
            flags.Add(newflag); // add flag
            f_inputfield.gameObject.SetActive(false); // disable
        }
        // reset
        f_inputfield.text = ""; // set default values because this object gets recycled
    }
}
