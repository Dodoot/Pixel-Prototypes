using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BT_DialogueManager : MonoBehaviour
{
    [SerializeField] float textSpeed = 5;

    [Header("UI")]
    [SerializeField] GameObject dialogueBubbleSprite = null;
    [SerializeField] GameObject arrowNext = null;
    [SerializeField] GameObject arrowChoice1 = null;
    [SerializeField] GameObject arrowChoice2 = null;
    [SerializeField] Text textDialogue = null;
    [SerializeField] Text textChoice1 = null;
    [SerializeField] Text textChoice2 = null;

    BT_Talker currentTalker;
    BT_Dialogue currentDialogue;
    int dialogueIndex = 0;
    bool playerCanContinue = false;
    bool playerCanChoose = false;

    // choices work as follow : 0 - not answered / 1 - choice 1 / 2 - choice 2
    int currentChoice = 1;

    private void Update()
    {
        if (playerCanContinue)
        {
            if (Input.GetButtonDown("Action1"))
            {
                playerCanContinue = false;
                NextBubble();
            }
        }
        else if (playerCanChoose)
        {
            if (Input.GetButtonDown("Action1"))
            {
                playerCanChoose = false;
                // send choice to world memory
                NextBubble();
            }
            if (Input.GetAxis("Vertical") > Mathf.Epsilon && currentChoice == 2)
            {
                currentChoice = 1;
                arrowChoice1.SetActive(true);
                arrowChoice2.SetActive(false);
            }
            if (Input.GetAxis("Vertical") < - Mathf.Epsilon && currentChoice == 1)
            {
                currentChoice = 2;
                arrowChoice1.SetActive(false);
                arrowChoice2.SetActive(true);
            }
        }
    }

    public void StartDialogue(BT_Dialogue dialogue, BT_Talker talker)
    {
        dialogueBubbleSprite.SetActive(true);
        CleanBubbleUI();

        currentTalker = talker;
        currentDialogue = dialogue;
        dialogueIndex = 0;

        ExecuteBubble();
    }

    public void CloseDialogue()
    {
        CleanBubbleUI();
        dialogueBubbleSprite.SetActive(false);

        currentTalker.FinishTalking();

        currentTalker = null;
        currentDialogue = null;
    }

    private void NextBubble()
    {
        CleanBubbleUI();

        dialogueIndex += 1;
        if (dialogueIndex < currentDialogue.dialogueBubbles.Length)
        {
            ExecuteBubble();
        }
        else
        {
            CloseDialogue();
        }
    }

    private void CleanBubbleUI()
    {
        arrowNext.SetActive(false);
        arrowChoice1.SetActive(false);
        arrowChoice2.SetActive(false);
        textDialogue.gameObject.SetActive(false);
        textChoice1.gameObject.SetActive(false);
        textChoice2.gameObject.SetActive(false);
    }

    private void ExecuteBubble()
    {
        BT_Bubble bubble = currentDialogue.dialogueBubbles[dialogueIndex];
        if (!bubble.isChoice)
        {
            textDialogue.text = "";
            textDialogue.gameObject.SetActive(true);
            StartCoroutine(TypeDialogueBubble(bubble));
        }
        else
        {
            textChoice1.text = bubble.choice1Text;
            textChoice1.gameObject.SetActive(true);
            textChoice2.text = bubble.choice2Text;
            textChoice2.gameObject.SetActive(true);

            playerCanChoose = true;
            currentChoice = 1;
            arrowChoice1.SetActive(true);
        }
    }

    private IEnumerator TypeDialogueBubble(BT_Bubble bubble)
    {
        foreach(char letter in bubble.dialogueText.ToCharArray())
        {
            textDialogue.text += letter;
            yield return new WaitForSeconds(1f / textSpeed);
        }
        playerCanContinue = true;
        arrowNext.SetActive(true);
    }
}
