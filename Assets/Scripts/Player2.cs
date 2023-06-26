using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
    public GameObject pawn; //GameObject variable for a player's pawn
    public Vector2 pawnPosition; //Vector 2 for the player's pawn position
    public bool moveAndShoreDiagonally;
    public bool moveToAnyTile;
    public bool moveOtherPlayerTwoSpaces;
    public bool canDive;
    public bool canShoreTwice;
    public bool canGiveCardsFar; //Booleans to keep track of the special things a player can do

    public Vector2[] treasureCardSlots = new Vector2[5]; //Array to store the points of where to display cards in a player's hand
    public int amountOfTreasureCards = 0; //Int to keep track of the player's hand size

    //Ako
    public TextMeshProUGUI SpecialAbility;
    public Button MoveButton;
    public Button SpecialButton;
    public Button GiveCardButton;
    public Button EndTurnButton;
    public TextMeshProUGUI ActionsLeft;
    public int Actions = 3;
    public GameObject Player1Objects;
    public GameObject Player2Objects;
    //Ako

    void Start()
    {
        moveAndShoreDiagonally = false;
        moveToAnyTile = false;
        moveOtherPlayerTwoSpaces = false;
        canDive = false;
        canShoreTwice = false;

        //Ako
        if (Actions >= 1)
        {
            MoveButton.onClick.AddListener(TaskOnClick);
            SpecialButton.onClick.AddListener(TaskOnClick);
            GiveCardButton.onClick.AddListener(TaskOnClick);
            EndTurnButton.onClick.AddListener(EndTurn);
        }
        //Ako
    }

    public void player2AssignHandSlots()
    {
        treasureCardSlots[0] = new Vector2(-8f, -2.40f);
        treasureCardSlots[1] = new Vector2(-7f, -2.40f);
        treasureCardSlots[2] = new Vector2(-6f, -2.40f);
        treasureCardSlots[3] = new Vector2(-5f, -2.40f);
        treasureCardSlots[4] = new Vector2(-4f, -2.40f);
    }

    //Ako
    void Update()
    {
        ActionsLeft.text ="Actions Left: "+ Actions.ToString();
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
       
        if (Actions > 3)
        {
            Actions = 3;
        }
        if (Actions <= 0)
        {
            Actions= 0;
        }
        if (Actions == 0)
        {
            EndTurn();
            //Next Player turn
        }
    }

    void TaskOnClick()
    {
        Actions = Actions - 1 ;
    }
    void EndTurn()
    {
        Player1Objects.SetActive(true);
        Player2Objects.SetActive(false);
        Actions = 3;
    }
    //Ako
}
