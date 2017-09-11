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
        p_yesorno, // Yes or no prompt
        p_inputField; // The input field prompt

    public Button 
        b_flagButtonPrefab, // Button used to display flags in Flag Management; prefabs
        b_renameFlag, // function buttons
        b_deleteFlag, 
        b_addFlag;

    private List<Button> buttonFlags = new List<Button>(); // list of button objects containing flag names
    private ArrayList flags = new ArrayList(); // Store all flags here
    private int
        processStage = 0, // which stage of the process it's in
        flagManagementState = 0;
    /* Flag Management States:
     * 0 -> Home
     * 1 -> Add
     * 2 -> Rename
     * 3 -> Delete
     * 4 -> Any State
     * */

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(flagManagementState);
        // D E T E C T   I N P U T
        if (rt_flagManagement.gameObject.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                flagManagementState = 0;
                //EventSystem.current.SetSelectedGameObject(if_searchBar.gameObject, null);
                SetFocus(if_searchBar.gameObject);
            }
                
            if (if_addFlag.isFocused) flagManagementState = 1;

            if (!if_searchBar.isFocused) {
                switch (flagManagementState) {
                    case 0: { // Home
                            if (Input.GetKeyDown(KeyCode.Escape)) { // close management panel
                                if (if_addFlag.gameObject.activeInHierarchy) AddFlagDialogue();
                                else ManageFlags();
                            }
                            if (Input.GetKeyDown(KeyCode.A)) { // add flags
                                if (!if_addFlag.gameObject.activeInHierarchy) AddFlagDialogue();
                            }
                            if (Input.GetKeyDown(KeyCode.R)) { // rename flags
                                Rename(null);
                            }
                            if (Input.GetKeyDown(KeyCode.D)) { // delete flags
                                Delete(null);
                            }
                            break;
                        }
                    case 1: { // Add
                              // Return -> Adds flag
                            if (Input.GetKeyDown(KeyCode.Return)) {
                                AddFlag();
                            }
                            // Escape -> Cancel, disable input field
                            if (Input.GetKeyDown(KeyCode.Escape)) {
                                AddFlagDialogue();
                            }
                            break;
                        }
                    case 2: { // Rename
                              // Choosing flag
                              // Esc -> cancel
                              // Renaming Dialogue Box
                              // Return -> 
                              // submit rename if message box inactive
                              // disable message box if active
                              // Esc -> 
                              // cancel rename if message box inactive
                              // disable message box if active
                            break;
                        }
                    case 3: { // Delete
                              // Choosing flag
                              // Esc -> cancel
                              // Deleteing decision prmpt
                              // Enter -> confirm delete
                              // Esc -> cancel delete
                            break;
                        }
                    default: {

                            break;
                        }
                }
            }
        }
    }

    public ArrayList GetFlags() {
        return flags;
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
            ResetToDefault();
            flagManagementState = 0;
            b_disabler.gameObject.SetActive(true);
            rt_flagManagement.gameObject.SetActive(true);
            rt_content.parent.parent.gameObject.transform.GetChild(1).GetComponent<Scrollbar>().value = 1; // reset the scrollbar scroll amount
        }
        else {
            b_disabler.gameObject.SetActive(false);
            rt_flagManagement.gameObject.SetActive(false);
        }
    }

    void ResetToDefault() {
        // Assume EVERYTHING is default
        b_addFlag.interactable = true;
        b_renameFlag.interactable = true;
        b_deleteFlag.interactable = true;
        if_addFlag.gameObject.SetActive(false);
        if_addFlag.text = "";
        if_searchBar.text = "";
        processStage = 0;
        SetInteractable(false);
        foreach (Button b in buttonFlags) {
            b.onClick.RemoveAllListeners();
        }
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

    public void AddFlagDialogue() {
        /* Enables the text box and add button when adding new flags 
         */
        flagManagementState = 1;
        if (if_addFlag.gameObject.activeInHierarchy) {
            flagManagementState = 0;
            if_addFlag.gameObject.SetActive(false);
        }
        else {
            if_addFlag.gameObject.SetActive(true);
            SetFocus(if_addFlag.gameObject);
        }
    }

    public void AddFlag() {
        /* Adds a new flag */
        string newflag = if_addFlag.text;
        if (CheckFlagName(newflag)) {// check if the string is valid
            flags.Add(newflag); // add flag
            if_addFlag.gameObject.SetActive(false); // disable input field and button

            Button temp = (Button)Instantiate(b_flagButtonPrefab, rt_content); // add new button to rt_content list
            temp.transform.GetChild(0).GetComponent<Text>().text = newflag; // set the button label
            buttonFlags.Add(temp); // add new button to buttonFlags list, no need to access Content RectTransform children :D :D :D :D

            RearrangeFlags();
            //temp.transform.localPosition = new Vector3(0, -(flags.Count-1)*30, 0); // check for position
        }
        // reset
        flagManagementState = 0;
        if_addFlag.text = ""; // set default values because this object is reusable
    }

    public void Rename(Button renamedButton) {
        /* Enables the user to rename existing flags
         * */
        if (flags.Count == 0) return;
        flagManagementState = 2;
        switch (processStage) {
            case 0: { 
                    // click rename
                    if(buttonFlags.Count != 0) {
                        SetInteractable(true);
                        foreach (Button b in buttonFlags) {
                            b.onClick.AddListener(delegate { Rename(b); });
                        }
                        b_addFlag.interactable = false;
                        if_addFlag.gameObject.SetActive(false);
                        b_renameFlag.interactable = false;
                        b_deleteFlag.interactable = false;
                        processStage = 1;
                    }
                    break;
                }
            case 1: { 
                    // click Flag to rename
                    // inputField Dialogue appears
                    if(renamedButton.gameObject.name != "Rename") { // if click rename again
                        p_inputField.GetComponent<ShowInputFieldDialogue>().button.onClick.AddListener(delegate { Rename(renamedButton); }); // change the function of the prompt button
                        p_inputField.GetComponent<ShowInputFieldDialogue>().ShowInputPrompt("Renaming Flag " + renamedButton.transform.GetChild(0).GetComponent<Text>().text, "Enter new name...", "", "Rename");
                        processStage = 2;
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
                        p_inputField.GetComponent<ShowInputFieldDialogue>().DisableInputPrompt();
                        p_inputField.GetComponent<ShowInputFieldDialogue>().button.onClick.RemoveAllListeners();
                        ResetToDefault();
                        flagManagementState = 0;
                    }
                    break;
                }
            default:
                break;
        }
    }

    public void Delete(Button deletedButton) {
        if (flags.Count == 0) return;
        flagManagementState = 3;
        switch (processStage) {
            case 0: {
                    // click delete
                    if (buttonFlags.Count != 0) {
                        SetInteractable(true);
                        foreach (Button b in buttonFlags) {
                            b.onClick.AddListener(delegate { Delete(b); });
                        }
                        b_addFlag.interactable = false;
                        b_renameFlag.interactable = false;
                        b_deleteFlag.interactable = false;
                        if_addFlag.gameObject.SetActive(false);
                        processStage = 1;
                    }
                    break;
                }
            case 1: {
                    string flagToBeDeleted = deletedButton.transform.GetChild(0).GetComponent<Text>().text;
                    //check for dependencies here, show message dialogue (unable to delete used flags)
                    //p_message.GetComponent<ShowMessageDialogue>().RaiseError("Unable to delete " + flagToBeDeleted + ".");

                    p_disabler.gameObject.SetActive(true);
                    p_yesorno.gameObject.SetActive(true);
                    p_yesorno.GetComponent<ShowDecisionPrompt>().yes.onClick.AddListener(delegate { Delete(deletedButton); });
                    p_yesorno.GetComponent<ShowDecisionPrompt>().no.onClick.AddListener(delegate { Delete(null); });

                    ShowDecisionPrompt sdc = p_yesorno.GetComponent<ShowDecisionPrompt>();
                    sdc.SetMessage("Are you sure you want to delete " + flagToBeDeleted + "?");
                    processStage = 2;
                    break;
                }
            case 2: {
                    if (deletedButton != null) {
                        string name = deletedButton.transform.GetChild(0).GetComponent<Text>().text;
                        for(int i = 0; i < flags.Count; i++) {
                            if(flags[i] == name) {
                                flags.RemoveAt(i);
                                buttonFlags.Remove(deletedButton);
                                Destroy(deletedButton.gameObject);
                                RearrangeFlags();
                                break;
                            }
                        }
                    }

                    p_yesorno.gameObject.SetActive(false);
                    p_disabler.gameObject.SetActive(false);
                    p_yesorno.GetComponent<ShowDecisionPrompt>().yes.onClick.RemoveAllListeners();
                    p_yesorno.GetComponent<ShowDecisionPrompt>().no.onClick.RemoveAllListeners();
                    ResetToDefault();
                    flagManagementState = 0;
                    break;
                }
            default: break;
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

    void SetFocus(GameObject obj) {
        EventSystem.current.SetSelectedGameObject(obj, null);
    }
}
