using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public GameObject pawn;
    public Vector2 pawnPosition;
    public bool moveAndShoreDiagonally;
    public bool moveToAnyTile;
    public bool moveOtherPlayerTwoSpaces;
    public bool canDive;
    public bool canShoreTwice;
    public bool canGiveCardsFar;
    public TextMeshProUGUI SpecialAbility;
    public GameObject MoveButton;
    public GameObject SpecialButton;
    public GameObject GiveCardButton;
    public GameObject EndTurnButton;

    // Start is called before the first frame update
    void Start()
    {
        moveAndShoreDiagonally = false;
        moveToAnyTile = false;
        moveOtherPlayerTwoSpaces = false;
        canDive = false;
        canShoreTwice = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pawn.name == "Red")
        {
            canShoreTwice = true;
            SpecialAbility.text = "Shoretwice";

        }
        else if (pawn.name == "Blue")
        {
            moveToAnyTile = true;
            SpecialAbility.text = "MoveToAnyTile";
        }
        else if (pawn.name == "Green")
        {
            moveAndShoreDiagonally = true;
            SpecialAbility.text = "MoveAndshoreDiagonally";
        }
        else if (pawn.name == "White")
        {
            canGiveCardsFar = true;
            SpecialAbility.text = "GiveCardsFar";
        }
        else if (pawn.name == "Yellow")
        {
            moveOtherPlayerTwoSpaces = true;
            SpecialAbility.text = "MoveOtherPlayerTwoSpaces";
        }
        else if (pawn.name == "Black")
        {
            canDive = true;
            SpecialAbility.text = "Dive";
        }
    }
}
