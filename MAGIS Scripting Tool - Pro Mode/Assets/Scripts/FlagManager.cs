using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour {

    public InputField f_inputfield; // Name input when adding flags
    public RectTransform f_management; // Flag management panel
    public Image disabler; // Disables buttons behind popup panel
    public Image prompt; // The object used for error messages
    public RectTransform content; // The scrollrect that displays all flags
    public Button flagDisplayButton; // Button used to display flags in Flag Management

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
        bool valid = true;

        // empty string
        if(name == "") valid = false;

        // special characters
        for (int i = 0; i < name.Length; i++) {
            if (!char.IsLetterOrDigit(name[i])) {
                valid = false;
                break;
            }
        }

        // existing flags
        for(int i = 0; i < flags.Count; i++) {
            if(name == (string) flags[i]) valid = false;
        }

        if (!valid) {
            prompt.GetComponent<ShowMessageDialogue>().RaiseError("Invalid flag name");
            return false;
        }

        return true;
    }

    public void ManageFlags() {
        /* Displays the flags for users to manage.
         * */
        if (!f_management.gameObject.activeInHierarchy) {
            disabler.gameObject.SetActive(true);
            f_management.gameObject.SetActive(true);
            content.parent.parent.gameObject.transform.GetChild(1).GetComponent<Scrollbar>().value = 1; // reset the scrollbar scroll amount
            DisplayFlags();
        }
        else {
            disabler.gameObject.SetActive(false);
            f_management.gameObject.SetActive(false);
            f_inputfield.gameObject.SetActive(false);
        }
    }

    public void AddFlag() {
        /* Adds a new flag */
        string newflag = f_inputfield.text;
        // check if the string is valid
        if (CheckFlagName(newflag)) {
            flags.Add(newflag); // add flag
            f_inputfield.gameObject.SetActive(false); // disable input field and button

            Button temp = (Button) Instantiate(flagDisplayButton, content); // add new button to content list
            temp.transform.GetChild(0).GetComponent<Text>().text = newflag; // set the button label
            
            temp.transform.localPosition = new Vector3(0, -(flags.Count-1)*30, 0); // check for position
            content.sizeDelta = new Vector2(0, flags.Count * 30); // resize content rect
        }
        // reset
        f_inputfield.text = ""; // set default values because this object gets recycled
    }

    void DisplayFlags() {
        /*
         * Add flag
         * Change Search bar
         * Rename
         * Delete
         * Click manage flags XX
         * Scrolling (auto) XX
         * */

        for(int i = 0; i < flags.Count; i++) {
            
        }
    }
}
