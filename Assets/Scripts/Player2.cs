using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public GameObject pawn;
    public Vector2 pawnPosition;
    public bool moveAndShoreDiagonally;
    public bool moveToAnyTile;
    public bool moveOtherPlayerTwoSpaces;
    public bool canDive;
    public bool canShoreTwice;
    public bool canGiveCardsFar;

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

        } else if (pawn.name == "Blue")
        {
            moveToAnyTile = true;

        } else if (pawn.name == "Green")
        {
            moveAndShoreDiagonally = true;

        } else if (pawn.name == "White")
        {
            canGiveCardsFar = true;

        } else if (pawn.name == "Yellow")
        {
            moveOtherPlayerTwoSpaces = true;

        } else if (pawn.name == "Black")
        {
            canDive = true;
        }
    }
}
