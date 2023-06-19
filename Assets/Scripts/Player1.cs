using System.Collections;
using System.Collections.Generic;
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
        
    }
}
