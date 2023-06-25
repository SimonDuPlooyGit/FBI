using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Button MoveButton;
    public Button SpecialButton;
    public Button GiveCardButton;
    public Button EndTurnButton;
    public TextMeshProUGUI ActionsLeft;
    public int Actions = 3;
    public GameObject Player1Objects;
    public GameObject Player2Objects;



    // Start is called before the first frame update
    void Start()
    {
        Player2Objects.SetActive(false);
        moveAndShoreDiagonally = false;
        moveToAnyTile = false;
        moveOtherPlayerTwoSpaces = false;
        canDive = false;
        canShoreTwice = false;
        if (Actions >= 1)
        {
            MoveButton.onClick.AddListener(TaskOnClick);
            SpecialButton.onClick.AddListener(TaskOnClick);
            GiveCardButton.onClick.AddListener(TaskOnClick);
            EndTurnButton.onClick.AddListener(EndTurn);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActionsLeft.text = "Actions Left: " + Actions.ToString();
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
            Actions = 0;
        }
        if (Actions == 0)
        {
            EndTurn();
            //Next Player turn
        }

    }

    void TaskOnClick()
    {
        Actions = Actions - 1;
    }
    void EndTurn()
    {
        Player1Objects.SetActive(false);
        Player2Objects.SetActive(true);
        Actions = 3;
    }
}
