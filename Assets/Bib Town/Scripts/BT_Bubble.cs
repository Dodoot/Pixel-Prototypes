using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BT_Bubble")]
public class BT_Bubble : ScriptableObject
{
    [SerializeField] public Sprite CharacterName = null;

    [SerializeField] public bool isChoice = false;

    [Header("Dialogue")]
    [TextArea]
    [SerializeField] public string dialogueText = null;

    [Header("Choice")]
    [SerializeField] public string choice1Text = null;
    [SerializeField] public string choice2Text = null;
    [SerializeField] public string choiceConsequenceVariable = null;
}