using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FlagManager : MonoBehaviour {

    public InputField 
        if_addFlag, // Name input when adding flags
        if_searchBar; // Search bar for flags

    public RectTransform 
        rt_flagManagement, // Flag management panel
        rt_content; // The scrollrect that displays all flags

    public Image 
        b_disabler, // Main layer disabler
        p_disabler, // Disables buttons behind popup panel
        p_message, // The object used for error messages
        p_inputField; // The input field prompt

    public Button 
        b_flagButtonPrefab, // Button used to display flags in Flag Management; prefabs
        b_renameFlag, // function buttons
        b_deleteFlag, 
        b_addFlag; 

    private List<Button> buttonFlags = new List<Button>(); // list of button objects containing flag names
    private ArrayList flags = new ArrayList(); // Store all flags here
    private int renamingStage = 0; // which stage of renaming

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
        if_addFlag.gameObject.SetActive(true);
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
            p_message.GetComponent<ShowMessageDialogue>().RaiseError("Invalid flag name");
            return false;
        }

        return true;
    }

    bool CheckSearchQuery(string query) {
        /* Checks whether the submitted search query is a valid query
         * Note that this also allows characters like åäö
         * Empty string query displays all flags
         * */
        bool valid = true;

        // special characters
        for (int i = 0; i < name.Length; i++) {
            if (!char.IsLetterOrDigit(name[i])) {
                valid = false;
                break;
            }
        }

        if (!valid) {
            p_message.GetComponent<ShowMessageDialogue>().RaiseError("Invalid flag name");
            return false;
        }

        return true;
    }

    public void ManageFlags() {
        /* Displays the flags for users to manage.
         * */
        if (!rt_flagManagement.gameObject.activeInHierarchy) {
            // Assume EVERYTHING is default
            b_addFlag.interactable = true;
            b_deleteFlag.interactable = true;
            if_addFlag.text = "";
            renamingStage = 0;
            SetInteractable(false);
            foreach (Button b in buttonFlags) {
                b.onClick.RemoveAllListeners();
            }
            if_searchBar.text = "";

            b_disabler.gameObject.SetActive(true);
            rt_flagManagement.gameObject.SetActive(true);
            rt_content.parent.parent.gameObject.transform.GetChild(1).GetComponent<Scrollbar>().value = 1; // reset the scrollbar scroll amount
        }
        else {
            b_disabler.gameObject.SetActive(false);
            rt_flagManagement.gameObject.SetActive(false);
            if_addFlag.gameObject.SetActive(false);
        }
    }

    public void AddFlag() {
        /* Adds a new flag */
        string newflag = if_addFlag.text;
        // check if the string is valid
        if (CheckFlagName(newflag)) {
            flags.Add(newflag); // add flag
            if_addFlag.gameObject.SetActive(false); // disable input field and button

            Button temp = (Button) Instantiate(b_flagButtonPrefab, rt_content); // add new button to rt_content list
            temp.transform.GetChild(0).GetComponent<Text>().text = newflag; // set the button label
            buttonFlags.Add(temp); // add new button to buttonFlags list, no need to access Content RectTransform children :D :D :D :D

            RearrangeFlags();
            //temp.transform.localPosition = new Vector3(0, -(flags.Count-1)*30, 0); // check for position
        }
        // reset
        if_addFlag.text = ""; // set default values because this object is reusable
    }
    
    private class ButtonSorter : IComparer<Button> {
        /* Used to sort Button objects based on their text child
         * */
        int IComparer<Button>.Compare(Button x, Button y) {
            return string.Compare(x.transform.GetChild(0).GetComponent<Text>().text, y.transform.GetChild(0).GetComponent<Text>().text);
        }
    }

    private void RearrangeFlags() {
        /* Rearranges buttonFlags displayed in Content alphabetically
         * */
        List<Button> activeFlags= new List<Button>(); // only sort the active flags; useful for search function
        
        foreach(Button b in buttonFlags) { // determine which flags are active
            if (b.gameObject.activeInHierarchy) activeFlags.Add(b);
        }

        activeFlags.Sort(new ButtonSorter()); // sort according to comparator
        rt_content.sizeDelta = new Vector2(rt_content.sizeDelta.x, 30 * activeFlags.Count);

        for(int i = 0; i<activeFlags.Count; i++) { // display
            Button current = activeFlags[i];
            current.transform.localPosition = new Vector3(0, i * -30, 0);
        }
    }

    public void Rename(Button renamedButton) {
        /* Enables the user to rename existing flags
         * */
        switch (renamingStage) {
            case 0: { 
                    // click rename
                    if(buttonFlags.Count != 0) {
                        SetInteractable(true);
                        foreach (Button b in buttonFlags) {
                            b.onClick.AddListener(delegate { Rename(b); });
                        }
                        b_addFlag.interactable = false;
                        if_addFlag.gameObject.SetActive(false);
                        if_addFlag.text = "";
                        b_deleteFlag.interactable = false;
                        renamingStage = 1;
                    }
                    break;
                }
            case 1: { 
                    // click Flag to rename
                    // inputField Dialogue appears
                    if(renamedButton.gameObject.name != "Rename") { // if click rename again
                        p_inputField.GetComponent<ShowInputFieldDialogue>().button.onClick.AddListener(delegate { Rename(renamedButton); }); // change the function of the prompt button
                        p_disabler.gameObject.SetActive(true);
                        p_inputField.gameObject.SetActive(true);

                        ShowInputFieldDialogue sfd = p_inputField.GetComponent<ShowInputFieldDialogue>();
                        sfd.SetButtonLabel("Rename");
                        sfd.SetMessage("Renaming Flag " + renamedButton.transform.GetChild(0).GetComponent<Text>().text);
                        sfd.SetPlaceholderText("Enter new name...");
                        sfd.SetActualText("");
                        renamingStage = 2;
                    }
                    
                    break;
                }
            case 2: {
                    // check if valid name
                    string newname = p_inputField.transform.GetChild(1).GetComponent<InputField>().text;
                    if (CheckFlagName(newname)) {
                        
                        // find and rename flag
                        for (int i = 0; i < flags.Count; i++) {
                            string oldname = renamedButton.gameObject.transform.GetChild(0).GetComponent<Text>().text;
                            if ((string)flags[i] == oldname) {
                                flags[i] = newname; // rename the internal flag
                                renamedButton.gameObject.transform.GetChild(0).GetComponent<Text>().text = newname; // rename button as well
                                break;
                            }
                        }

                        // reset everything and exit rename status
                        p_disabler.gameObject.SetActive(false);
                        p_inputField.gameObject.SetActive(false);
                        renamingStage = 0;
                        SetInteractable(false);
                        foreach (Button b in buttonFlags) {
                            b.onClick.RemoveAllListeners();
                        }
                        p_inputField.GetComponent<ShowInputFieldDialogue>().button.onClick.RemoveAllListeners();
                        b_addFlag.interactable = true;
                        b_deleteFlag.interactable = true;
                    }
                    break;
                }
            default:
                break;
        }
    }

    public void Search() {
        string query = if_searchBar.text;
        //check if valid query
        foreach (Button b in buttonFlags) {
            b.gameObject.SetActive(true);
        }

        if(query == "") {
            foreach(Button b in buttonFlags) {
                if (!b.gameObject.activeInHierarchy) {
                    b.gameObject.SetActive(true);
                }
            }
        }
        else {
            if (!CheckSearchQuery(query)) {
                if_searchBar.text = "";
                return;
            }

            foreach (Button b in buttonFlags) {
                string buttonLabel = b.transform.GetChild(0).GetComponent<Text>().text;
                if (buttonLabel.Length < query.Length || buttonLabel.Substring(0, query.Length) != query) {
                    b.gameObject.SetActive(false);
                }
            }
        }
        RearrangeFlags();
    }

    void SetInteractable(bool status) {
        /* Set whether buttons are interactable or not
         * */
         foreach(Button b in buttonFlags) {
            b.gameObject.GetComponent<Button>().interactable = status;
        }
    }
}
