using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class ScriptReader : MonoBehaviour
{
    private static ScriptReader instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    // TextAsset _InkJsonFile;
    private Story _StoryScript;

    public TMP_Text nameTag;

    //image char
    public Image characterIcon;

    public bool dialogueIsPlaying{ get; private set; }

    [Header("Choices UI")]//Creates buttons choices
    [SerializeField]
    private GridLayoutGroup choiceHolder;
    [SerializeField]
    private Button choiceBasePrefab;
    
    //Game Manager
    private GameManager GM;

    private void Awake()
    {
        instance = this;
    }

    public static ScriptReader GetInstance()
    {
       // Debug.LogWarning("More than one Dialogue Manager in the scene :(");
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if(GM.GameStatus() == true)
        {
                if (!dialogueIsPlaying)
                {
                    return;
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    DisplayNextLine();
                }
        }
    }

    public void LoadStory(TextAsset inkJSON)
    {
        _StoryScript = new Story(inkJSON.text);
        //activate the visual novel ui
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        _StoryScript.BindExternalFunction("CharName", (string charName) => ChangeName(charName));
        _StoryScript.BindExternalFunction("Icon", (string charName) => CharacterIcon(charName));
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (_StoryScript.canContinue)//Checking if there is content to go through
        {
            string text = _StoryScript.Continue();//Gets next line
            text = text?.Trim();//removes white space from the text
            dialogueText.text = text;//Displays new text
        }
        else if (_StoryScript.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            CloseStory();
        }
    }

    private void CloseStory()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        //dialogueText.text = "";
    }

    private void DisplayChoices()
    {
        if (choiceHolder.GetComponentsInChildren<Button>().Length > 0) return; //Checks if button hold has button in it
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
