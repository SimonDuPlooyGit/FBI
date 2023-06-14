using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class CardSO : ScriptableObject //This is the card scripable object
{
    public string playerSpawn; //A card will have a variable to know which player is spawns
    public string treasure; //Which treasure it holds
    public string cardName; //What its name is
    public bool cardFlooded; //What its flooded state is
}
