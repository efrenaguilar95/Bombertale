using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueBoxManager : MonoBehaviour
{

    public GameObject textBox;
    public GameObject dialoguePortrait;
    public Text theText; 

    public TextAsset textFile;
    public string[] textLines;
    private string[] textInfo;
    private string displayedText;
    private Image portraitSprite;
    private string characterName;
    private string characterMood;
    private string portraitPath;

    public int currentLine;
    public int endAtLine;

    public bool isActive;
    public bool justPressed; //used for interactable dialogues, since return would be called twice in the same frame

    private bool isTyping = false;
    private bool cancelTyping = false;

    public float typeSpeed;
    private float initialSpeed;

    public AudioSource textSound;

    // Use this for initialization
    void Start()
    {
        initialSpeed = typeSpeed;
        portraitSprite = dialoguePortrait.GetComponent<Image>();
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
            enableDialogue();
        else
            disableDialogue();
    }

    //Called every frame
    void Update()
    {

        if (!isActive)
            return;
        if(justPressed)
        {
            justPressed = false;
            return;
        }

        //if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && typeSpeed != initialSpeed/1000)
        //{
        //    typeSpeed /= 1000;
        //    Debug.Log("AY");
        //}

        if (Input.anyKeyDown)
        {
            if (textBox.activeSelf)
            {
                    
                if (!isTyping)
                {
                    //typeSpeed = initialSpeed;
                    currentLine++;
                    if (currentLine > endAtLine)
                    {
                        disableDialogue();
                    }
                    else
                    {
                        textInfo = textLines[currentLine].Split(':');
                        if (textInfo.Length > 1)
                        {
                            dialoguePortrait.SetActive(true);
                            characterName = textInfo[0];
                            characterMood = textInfo[1];
                            portraitPath = "DialoguePortraits/" + characterName + "/" + characterName + "_" + characterMood;
                            portraitSprite.sprite = Resources.Load<Sprite>(portraitPath);
                            displayedText = textInfo[2];
                        }
                        else
                        {
                            //dialoguePortrait.SetActive(false);
                            displayedText = textLines[currentLine];
                        }
                        StartCoroutine(TextScroll(displayedText));
                    }
                }
                else if (isTyping && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }
        }

    }

    private IEnumerator TextScroll (string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        textInfo = lineOfText.Split(new string[] { "/P" }, System.StringSplitOptions.None);
        string textLine = textInfo[0];
        int i = 0;
        while (isTyping && !cancelTyping && (letter < textLine.Length - 1))
        {
            theText.text += textLine[letter];
            letter += 1;
            if (textSound != null && !char.IsPunctuation(textLine[letter]) && textLine[letter] != ' ')
                textSound.Play();
            if(letter == textLine.Length-1 && i != textInfo.Length-1)
            {
                theText.text += textLine[letter];
                letter = 0;
                i++;
                textLine = textInfo[i];
                theText.text += "\n";
            }
            yield return new WaitForSeconds(typeSpeed);
        }
        theText.text = "";
        foreach (string textLine2 in textInfo)
        {
            theText.text += textLine2 + "\n";
        }
        isTyping = false;
        cancelTyping = false;
        //typeSpeed = initialSpeed;
    }

    public void enableDialogue()
    {
        textBox.SetActive(true);
        dialoguePortrait.SetActive(true);  
        isActive = true;
        textInfo = textLines[currentLine].Split(':');
        if (textInfo.Length > 1)
        {
            dialoguePortrait.SetActive(true);
            characterName = textInfo[0];
            characterMood = textInfo[1];
            portraitPath = "DialoguePortraits/" + characterName + "/" + characterName + "_" + characterMood;
            portraitSprite.sprite = Resources.Load<Sprite>(portraitPath);
            displayedText = textInfo[2];
        }
        else
        {
            //dialoguePortrait.SetActive(false);
            displayedText = textLines[currentLine];
        }
        StartCoroutine(TextScroll(displayedText));

    }

    public void disableDialogue()
    {
        //textBox.SetActive(false);
        //dialoguePortrait.SetActive(false);
        isActive = false;
    }

    public void reloadDialogue(TextAsset theText)
    {
        if(theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }

    public void setTextSound(AudioClip newClip)
    {
        textSound.clip = newClip;
    }
}
