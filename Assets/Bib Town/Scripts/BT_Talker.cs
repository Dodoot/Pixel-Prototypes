using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Talker : MonoBehaviour
{
    // could be talkable only if it has dialogue ?

    [SerializeField] BT_Dialogue dialogue = null;
    [SerializeField] GameObject talkArrow = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BT_Player myPlayer = collision.GetComponent<BT_Player>();
        if (myPlayer)
        {
            myPlayer.setCurrentTalker(this);
        }

        talkArrow.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BT_Player myPlayer = collision.GetComponent<BT_Player>();
        if (myPlayer)
        {
            myPlayer.setCurrentTalker(null);
        }

        talkArrow.SetActive(false);
    }

    public void Talk()
    {
        FindObjectOfType<BT_DialogueManager>().StartDialogue(dialogue, this);

        FindObjectOfType<BT_Player>().isTalking = true;
        talkArrow.SetActive(false);
    }

    public void FinishTalking()
    {
        // Check if next dialogue should be auto launched.

        FindObjectOfType<BT_Player>().isTalking = false;
        talkArrow.SetActive(true);
    }
}
