using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour {

    public InputField f_inputfield; // Name input when adding flags
    public InputField searchBar; // Search bar for flags
    public RectTransform f_management; // Flag management panel
    public RectTransform content; // The scrollrect that displays all flags
    public Image disabler; // Disables buttons behind popup panel
    public Image prompt; // The object used for error messages
    public Button flagDisplayButton; // Button used to display flags in Flag Management
    public Button searchButton; // search button

    private List<Button> buttonFlags = new List<Button>(); // list of button objects containing flag names
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
            buttonFlags.Add(temp); // add new button to buttonFlags list, no need to access Content RectTransform children :D :D :D :D

            RearrangeFlags();
            //temp.transform.localPosition = new Vector3(0, -(flags.Count-1)*30, 0); // check for position
        }
        // reset
        f_inputfield.text = ""; // set default values because this object is reusable
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

        for(int i = 0; i<activeFlags.Count; i++) { // display
            Button current = activeFlags[i];
            current.transform.localPosition = new Vector3(0, i * -30, 0);
        }
    }

    public void Search() {
        string query = searchBar.text;
        //check if valid query
        foreach(Button b in buttonFlags) {
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
                searchBar.text = "";
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
}
