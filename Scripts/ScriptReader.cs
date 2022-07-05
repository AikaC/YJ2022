using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class ScriptReader : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story _StoryScript;

    public TMP_Text dialogueBox;
    public TMP_Text nameTag;

    //image char
    public Image characterIcon;

    //Creates buttons choices
    [SerializeField]
    private GridLayoutGroup choiceHolder;
    [SerializeField]
    private Button choiceBasePrefab;

    // Start is called before the first frame update
    void Start()
    {
        LoadStory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            DisplayNextLine();
        }
    }

    void LoadStory()
    {
        _StoryScript = new Story(_InkJsonFile.text);
        _StoryScript.BindExternalFunction("CharName", (string charName) => ChangeName(charName));
        _StoryScript.BindExternalFunction("Icon", (string charName) => CharacterIcon(charName));
    }

    public void DisplayNextLine()
    {
        if (_StoryScript.canContinue)//Checking if there is content to go through
        {
            string text = _StoryScript.Continue();//Gets next line
            text = text?.Trim();//removes white space from the text
            dialogueBox.text = text;//Displays new text
        }
        else if (_StoryScript.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            dialogueBox.text = "Done.";
        }
    }

    private void DisplayChoices()
    {
        if (choiceHolder.GetComponentsInChildren<Button>().Length > 0) return; //Checks if button holdr has button in it
        for (int i = 0; i < _StoryScript.currentChoices.Count; i++)
        {
            var choice = _StoryScript.currentChoices[i];
            var button = CreateChoiceButton(choice.text); //create a choice button
            button.onClick.AddListener(() => OnClickChoiceButton(choice));
        }
    }

    Button CreateChoiceButton(string text)
    {
        //instantiate the button prefab
        var choiceButton = Instantiate(choiceBasePrefab);
        choiceButton.transform.SetParent(choiceHolder.transform, false);

        //change the text in the button prefab
        var buttonText = choiceButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = text;

        return choiceButton;
    }

    void OnClickChoiceButton(Choice choice)
    {
        _StoryScript.ChooseChoiceIndex(choice.index);
        RefreshChoiceView();
        DisplayNextLine();
    }

    void RefreshChoiceView()
    {
        if(choiceHolder != null)
        {
            foreach (var button in choiceHolder.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }
    }

    public void ChangeName(string name)
    {
        string CharName = name;
        nameTag.text = CharName;
    }

    public void CharacterIcon(string name)
    {
        var charIcon = Resources.Load<Sprite>("Char_Icons/" + name);
        characterIcon.sprite = charIcon;
    }
}