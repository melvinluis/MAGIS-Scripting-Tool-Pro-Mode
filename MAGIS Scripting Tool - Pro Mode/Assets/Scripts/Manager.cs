/**
 * This class handles all operations between M_Object, M_Interaction, M_Dialogue, etc.
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    /* Canvas Objects */
    public Canvas c_editing; // Main canvas screen where all the editing happens

    /* Prefab References */

    /* Other Objects */
    public Image disabler; // Disables the buttons on the main editing page by overlaying an image object

    /* Data Management */
    private ArrayList flags = new ArrayList(); // ArrayList that will contain all of the flags in string format

    /* Flag Management */
    public InputField f_inputfield; // Name input when adding flags
    public RectTransform f_management; // Flag management panel

    // Use this for initialization
    void Start () {

    }

	// Update is called once per frame
	void Update () {
		
	}

    /* Flags Management */
    public void AddFlag() {
        /* Adds a new flag */

        // check if the string is valid

        // if valid, add
        flags.Add(f_inputfield.text);
        // reset
        f_inputfield.text = ""; // set default values because this object gets recycled
        f_inputfield.gameObject.SetActive(false); // disable
    }

    bool CheckFlagName(string name) {
        // TODO: do checks here
        return true;
    }

    public void AddFlagDialogue() {
        /* Enables the text box and add button when adding new flags 
         */
        f_inputfield.gameObject.SetActive(true);
    }

    public void ManageFlags() {
        /* Displays the flags for users to manage.
         * */

        if (!f_management.gameObject.activeInHierarchy) {
            disabler.gameObject.SetActive(true);
            f_management.gameObject.SetActive(true);

            // TODO: figure out how to display flag management
            for(int i = 0; i < flags.Count; i++) {
                Debug.Log(flags[i]);
            }
        }
        else {
            disabler.gameObject.SetActive(false);
            f_management.gameObject.SetActive(false);
        }
    }
}