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
    string currentText;
    int currentLetters, maxLetters;
    [SerializeField]float letterDelay;
    float timeTillNextLetter;
    [SerializeField]Color notHihlightedColor;
    [SerializeField]Dialogue[] dialogueList;
    public static int selectedDialogue;
    Controls inputScheme;
    
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
    void Start()
    {
        inputScheme = new Controls();
        inputScheme.Dialogue.Enable();
        inputScheme.Dialogue.Next.performed += NextLine;
        if (selectedDialogue < dialogueList.Length)
        {
            //the first time you have to use ReturnFirstLine cause we are modifying the marker forever for some reason (should be a struct)
            SetupLine(dialogueList[selectedDialogue].ReturnFirstLine());
        }
        else 
        {
            print("Error, Dialogue index out of bounds (selected "+selectedDialogue+" out of a maximum of "+dialogueList.Length+")");
        }
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
    void SetupLine(DialogueLine line)
    {
        if (line == null)
        {
            //TODO end of conversation
            SceneManager.LoadScene("Main Scene");
            return;    
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