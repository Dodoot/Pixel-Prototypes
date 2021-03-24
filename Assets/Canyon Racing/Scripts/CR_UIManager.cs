using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CR_UIManager : MonoBehaviour
{
    [SerializeField] Text turnText = null;
    [SerializeField] Text positionText = null;

    public void UpdateTurnText(int newTurn, int maxTurn)
    {
        turnText.text = "TURN  " + newTurn.ToString() + " / " + maxTurn.ToString();
    }

    public void UpdatePositionText(int newPosition)
    {
        positionText.text = newPosition.ToString();
        if(newPosition == 1) { positionText.text += " st"; }
        else if (newPosition == 2) { positionText.text += " nd"; }
        else if (newPosition == 3) { positionText.text += " rd"; }
        else { positionText.text += " th"; }
    }
}
