using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ActionSuggestUI", menuName = "ScriptableObjects/ActionSuggestUI", order = 1)]
public class ActionSuggestUI : ScriptableObject
{
    public string suggestion;
    public SpriteRenderer icon;
}