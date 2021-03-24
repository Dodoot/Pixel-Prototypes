using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BT_Dialogue")]
public class BT_Dialogue : ScriptableObject
{
    [SerializeField] public BT_Bubble[] dialogueBubbles = null;
}