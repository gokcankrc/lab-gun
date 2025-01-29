using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
    [SerializeField]BlackOverlay fadeToBlack;
    [SerializeField]Image background, leftCharacter, rightCharacter, textBubble;
    [SerializeField]TMPro.TMP_Text title, text;
    [SerializeField]GameObject button1, button2;
    [SerializeField]TMPro.TMP_Text button1Text, button2Text;
    string currentText;
    int currentLetters, maxLetters;
    bool willEnd;
    [SerializeField]float letterDelay;
    float timeTillNextLetter;
    [SerializeField]Color notHihlightedColor;
    [SerializeField]Dialogue[] dialogueList;
    public static int selectedDialogue;
    Controls inputScheme;
    bool endsWithChoices;
    
    void Update ()
    {
        if (currentLetters<maxLetters)
        {
            timeTillNextLetter -= Time.deltaTime;
            if (timeTillNextLetter <= 0)
            {
                timeTillNextLetter = letterDelay;
                //add new letter
                AddLetter();
            }
        }
    }
    void OnDestroy()
    {
        inputScheme.Dialogue.Disable();
        inputScheme.Dialogue.Next.performed -= NextLine;
    }
    void Start()
    {
        
        inputScheme = new Controls();
        inputScheme.Dialogue.Enable();
        inputScheme.Dialogue.Next.performed += NextLine;
        if (selectedDialogue < dialogueList.Length)
        {
            StartDialogue(selectedDialogue);
            //the first time you have to use ReturnFirstLine cause we are modifying the marker forever for some reason (should be a struct)
            
        }
        
        else 
        {
            
            print("Error, Dialogue index out of bounds (selected "+selectedDialogue+" out of a maximum of "+dialogueList.Length+")");
            SceneManager.LoadScene("Main Scene");
        }
    }
    public void ChoiceSelected (bool first)
    {
        if (first)
        {
            StartDialogue(dialogueList[selectedDialogue].choìice1ID);
        }
        else 
        {
            StartDialogue(dialogueList[selectedDialogue].choice2ID);
        }
    }
    public void StartDialogue (int selected)
    {
        endsWithChoices=dialogueList[selected].showChoicesAtEnd;
        selectedDialogue = selected;
        willEnd = dialogueList[selected].isEnding;
        button1.SetActive(false);
        button2.SetActive(false);
        button1Text.SetText(dialogueList[selected].choiceText1);
        button2Text.SetText(dialogueList[selected].choiceText2);
        SetupLine(dialogueList[selected].ReturnFirstLine());
    }
    void NextLine (UnityEngine.InputSystem.InputAction.CallbackContext input){
        SetupLine(dialogueList[selectedDialogue].ReturnNextLine());
    }
    void SetupText(string newText)
    {
        currentText = newText;
        maxLetters = newText.Length;
        currentLetters = 1;
        timeTillNextLetter = letterDelay;
        text.SetText(currentText.Remove(currentLetters,maxLetters-currentLetters));
        
    }
    void AddLetter()
    {
        currentLetters++;
        text.SetText(currentText.Remove(currentLetters,maxLetters-currentLetters));
    }
    public static void StartDialogueScene (int index)
    {
        DialogueManager.selectedDialogue = index;
        SceneManager.LoadScene("Dialogues");
    }
    void SetupLine(DialogueLine line)
    {
        if (line == null)
        {
            //TODO end of conversation
            if (endsWithChoices)
            {
                button1.SetActive(true);
                button2.SetActive(true);
                return;
            }
            else 
            {
                if (willEnd)
                {
                    SceneManager.LoadScene("Main Menu");
                }
                else 
                {
                    print ("Switching to main");
                    SceneManager.LoadScene("Main Scene");
                }
                
                return;    
            }
            
        }
        if (line.textShowing)
        {
            title.SetText(line.title);
            //text.SetText(line.text);
            SetupText(line.text);
            title.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            textBubble.gameObject.SetActive(true);
        }
        else 
        {
            title.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            textBubble.gameObject.SetActive(false);
        }
        
        leftCharacter.sprite = line.leftCharacter;
        if (leftCharacter.sprite == null)
        {
            leftCharacter.color = Color.clear;
        }
        if (line.leftHighlighted)
        {
            leftCharacter.color = Color.white;
        }
        else 
        {
            leftCharacter.color = notHihlightedColor;
        }
        

        rightCharacter.sprite = line.rightCharacter;
        if (rightCharacter.sprite == null)
        {
            rightCharacter.color = Color.clear;
        }
        else if (line.rightHighlighted)
        {
            rightCharacter.color = Color.white;
        }
        else 
        {
            rightCharacter.color = notHihlightedColor;
        }

        background.sprite = line.background;
        if (line.backgroundFadeIn)
        {
            fadeToBlack.FadeIn(line.fadeTime);
        }
        if (line.backgroundFadeOut)
        {
            fadeToBlack.FadeOut(line.fadeTime);
        }

    }
    
}
[Serializable]
public class DialogueLine
{
    public Sprite leftCharacter,rightCharacter, background;    
    public bool leftHighlighted, rightHighlighted, backgroundFadeIn,backgroundFadeOut, textShowing;
    public string title, text;
    public float fadeTime = 1f;
}