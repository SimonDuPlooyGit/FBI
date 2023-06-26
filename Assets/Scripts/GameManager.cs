using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

/*
 (If there is a space between points it means it's a different system)
 What needs to still be done: (In order of importance/what relies on one another)

-Difficulty Selection
-Flooding: Needs to reflect on the watermeter and it needs to flood/sink the correct tile/flooding deck needs to work
^Will have a branch

-Spawning the player pawns and the players representation
^Will have a branch

-A bool to keep track of whether player 1 is playing or player 2 (only 2 players)
 and the working player turn system
^Will have a branch

-Movement options/system: (Higly unlikely but arrow movement).
                   Click a pawn and be able to see where they are allowed to move (highlight the next possible moves).
                   Click pawn then click where you want it to go.
^Will have a branch

Misc:
-The treasure deck needs to be made
-Treasure card functions
-Waters rise functions
-Special character functions
-Win and lose conditions
 */

public class GameManager : MonoBehaviour
{
    private List<Vector2> gridCoords = new List<Vector2>(); //Hardcoding of all the points to spawn tiles
    public List<GameObject> islandTileDeck = new List<GameObject>(); //A list of all of the island tiles (This served as the deck)
    private List<GameObject> floodDeck = new List<GameObject>(); //Same as above but for the flooding cards
    public Stack<GameObject> floodStack = new Stack<GameObject>(); //The actual deck representation for the flood deck. Stack to use Pop and Push (LIFO - for a deck)
    public List<GameObject> discardedFloodCards = new List<GameObject>(); //List of cards that have been pulled from the flooding deck (Popped) gets shuffled and pushed back onto the stack
    public List<GameObject> playerPawns = new List<GameObject>(); //List to hold all of the possible player pawns that can be handed out/assigned
    public List<GameObject> treasureCards; //List to hold all of the treasure cards
    public Stack<GameObject> treasureStack = new Stack<GameObject>(); //Actual deck, same idea as for floodstack

    public GameObject player1;
    public GameObject player2;

    public enum PointState //An enum to define the different states a point can be in
    {
        Empty,
        Available,
        Filled,
        Sunk
    }

    public enum PlayerPawnHere //Currently is actually incorrect because more than one pawn can be on a tile at once
    {
        Red,
        White,
        Yellow,
        Blue,
        Black,
        Green,
        Double,
        None
    }

    public struct BoardPoint //A struct that holds a card and its position (Idea is to make a 6x6 array of this)
    {
        public PointState pointState; //An enum variable to hold the point's state
        public PlayerPawnHere playerPawnHere;
        public GameObject cardAtPoint; //First argument of the struct which is the card object
        public Vector2 cardPosition; //Second argument of the struct which is that card's position

        public BoardPoint(PointState state, PlayerPawnHere pawn, GameObject obj, Vector2 pos) //Struct constructor
        {
            playerPawnHere = pawn;
            pointState = state;
            cardAtPoint = obj;
            cardPosition = pos;
        }
    }

    public BoardPoint[,] Board; //A 2D array that holds BoardPoint structs. This is the representation of the board

    void Start() //Just testing stuff
    {
        Board = new BoardPoint[6, 6]; //Initializes the Board to be a 2D array of 6 rows and 6 columns

        createIslandDeck();
        shuffleIslands();
        populateBoard();
        createFloodDeck();
        shuffleFlood();
        createFloodStack();
        //DisplayBoard(Board);
        getAndAssignPlayerPawns();
        spawnPawns();
        createTreasureDeck();
        shuffleTreasure();
        createTreasureStack();
        player1.GetComponent<Player1>().player1AssignHandSlots();
        player2.GetComponent<Player2>().player2AssignHandSlots();
        handOutTwoTreasureCardsStart();
    }

    
    void Update() //Not needed yet
    {
        
    }

    public void createIslandDeck() //Loads all of the prefab tiles and adds it into island deck list
    {
        var resources = Resources.LoadAll("IslandTiles", typeof(GameObject));

        foreach (GameObject obj in resources)
        {
            islandTileDeck.Add(obj);
        }
    }

    public void shuffleIslands() //Shuffles the island deck list
    {
        for (int i = 0; i < islandTileDeck.Count; i++)
        {
            GameObject temporary = islandTileDeck[i];
            int randInd = Random.Range(i, islandTileDeck.Count);
            islandTileDeck[i] = islandTileDeck[randInd];
            islandTileDeck[randInd] = temporary;
        }
    }

    public void createFloodDeck() //Repeat for the flood list
    {
        var resources = Resources.LoadAll("IslandTiles", typeof(GameObject));

        foreach (GameObject obj in resources)
        {
            floodDeck.Add(obj);
        }
    }

    public void shuffleFlood() //Repeat for the flood list
    {
        for (int i = 0; i < floodDeck.Count; i++)
        {
            GameObject temporary = floodDeck[i];
            int randInd = Random.Range(i, floodDeck.Count);
            floodDeck[i] = floodDeck[randInd];
            floodDeck[randInd] = temporary;
        }
    }

    public void createFloodStack() //For however many items are in the flood list, push it sequentially onto the flood stack
    {
        for (int i = 0; i < floodDeck.Count; i++)
        {
            floodStack.Push(floodDeck[i]);
        }
    }

    public void flood() //Prototype, all flooding related logic goes here. Has to still actually flood the corresponding tile.
    {
        discardedFloodCards.Add(floodStack.Pop()); //Only pops from the stack and adds to the discard list so far
    }


    public void shuffleFloodDiscard() //Repeat of she shuffle functions but for the discard pile of the flooding cards
    {
        for (int i = 0; i < discardedFloodCards.Count; i++)
        {
            GameObject temporary = discardedFloodCards[i];
            int randInd = Random.Range(i, discardedFloodCards.Count);
            discardedFloodCards[i] = discardedFloodCards[randInd];
            discardedFloodCards[randInd] = temporary;
        }
    }

    public void createTreasureDeck()
    {
        var resources = Resources.LoadAll("TreasureCards", typeof(GameObject));

        foreach (GameObject obj in resources)
        {
            treasureCards.Add(obj);
        }
    }

    public void shuffleTreasure()
    {
        for (int i = 0; i < treasureCards.Count; i++)
        {
            GameObject temporary = treasureCards[i];
            int randInd = Random.Range(i, treasureCards.Count);
            treasureCards[i] = treasureCards[randInd];
            treasureCards[randInd] = temporary;
        }
    }

    public void createTreasureStack()
    {
        for (int i = 0; i < treasureCards.Count; i++)
        {
            treasureStack.Push(treasureCards[i]);
        }
    }

    public void reAddDiscardToFlood() //Adds the shuffled discarded flood cards back onto the stack and clears she discard list
    {
        shuffleFloodDiscard();

        for (int i = 0; i < discardedFloodCards.Count; i++)
        {
            floodStack.Push(discardedFloodCards[i]);
        }

        discardedFloodCards.Clear();
    }

    public void populateBoard() //Spawns all of the island tiles
    {
        gridCoords.Add(new Vector2(-0.5f, 2.5f));
        gridCoords.Add(new Vector2(0.5f, 2.5f));
        gridCoords.Add(new Vector2(-1.5f, 1.5f));
        gridCoords.Add(new Vector2(-0.5f, 1.5f));
        gridCoords.Add(new Vector2(0.5f, 1.5f));
        gridCoords.Add(new Vector2(1.5f, 1.5f));
        gridCoords.Add(new Vector2(-2.5f, 0.5f));
        gridCoords.Add(new Vector2(-1.5f, 0.5f));
        gridCoords.Add(new Vector2(-0.5f, 0.5f));
        gridCoords.Add(new Vector2(0.5f, 0.5f));
        gridCoords.Add(new Vector2(1.5f, 0.5f));
        gridCoords.Add(new Vector2(2.5f, 0.5f));
        gridCoords.Add(new Vector2(-2.5f, -0.5f));
        gridCoords.Add(new Vector2(-1.5f, -0.5f));
        gridCoords.Add(new Vector2(-0.5f, -0.5f));
        gridCoords.Add(new Vector2(0.5f, -0.5f));
        gridCoords.Add(new Vector2(1.5f, -0.5f));
        gridCoords.Add(new Vector2(2.5f, -0.5f));
        gridCoords.Add(new Vector2(-1.5f, -1.5f));
        gridCoords.Add(new Vector2(-0.5f, -1.5f));
        gridCoords.Add(new Vector2(0.5f, -1.5f));
        gridCoords.Add(new Vector2(1.5f, -1.5f));
        gridCoords.Add(new Vector2(-0.5f, -2.5f));
        gridCoords.Add(new Vector2(0.5f, -2.5f));

        for (int i = 0; i < islandTileDeck.Count; i++)
        {
            islandTileDeck[i].transform.position = gridCoords[i];
            Instantiate(islandTileDeck[i], this.transform, true);
        }

        for (int i = 0; i < 6; i++) //Setting all of the board point states to available first with no info
        {
            for (int j = 0; j < 6; j++)
            {
                Board[i, j].pointState = PointState.Available;
                Board[i, j].cardAtPoint = null;
                Board[i, j].cardPosition = Vector2.zero;
            }
        }

        //Empty spots
        //0,0 | 0,1 | 0,4 | 0,5
        //1,0 | 1,5
        //4,0 | 4,5
        //5,0 | 5,1 | 5,4 | 5,5

        Board[0, 0] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(-2.5f, 2.5f)); //Manually setting all the empty spots of the board after everything was set to available
        Board[0, 1] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(-1.5f, 2.5f));
        Board[0, 4] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(1.5f, 2.5f));
        Board[0, 5] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(2.5f, 2.5f));
        Board[1, 0] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(-2.5f, 1.5f));
        Board[1, 5] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(2.5f, 1.5f));
        Board[4, 0] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(-2.5f, -1.5f));
        Board[4, 5] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(2.5f, -1.5f));
        Board[5, 0] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(-2.5f, -2.5f));
        Board[5, 1] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(-1.5f, -2.5f));
        Board[5, 4] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(1.5f, -2.5f));
        Board[5, 5] = new BoardPoint(PointState.Empty, PlayerPawnHere.None, null, new Vector2(2.5f, -2.5f));

        int placedTiles = 0;
        for (int i = 0; i < 6; i++) //Function so set all of the board points equal to the island tiles accordingly
        {
            for (int j = 0; j < 6; j++)
            {
                if (Board[i, j].pointState == PointState.Available)
                {
                    Board[i, j].pointState = PointState.Filled;
                    Board[i, j].playerPawnHere = PlayerPawnHere.None;
                    Board[i, j].cardAtPoint = islandTileDeck[placedTiles];
                    Board[i, j].cardPosition = islandTileDeck[placedTiles].transform.position;
                    placedTiles++;
                }
            }
        }
    }

    public void DisplayBoard(BoardPoint[,] board) //Debugging function to see the board and if it was initialized correctly
    {
        int rows = board.GetLength(0);
        int columns = board.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                BoardPoint point = board[row, column];
                string stateString = point.pointState.ToString();
                string statePawn = point.playerPawnHere.ToString();
                string cardName = point.cardAtPoint != null ? point.cardAtPoint.name : "None";
                Vector2 position = point.cardPosition;

                string displayText = string.Format("State: {0}, Pawn: {1}, Card: {2}, Position: {3}", stateString, statePawn, cardName, position);
                Debug.Log(displayText);
            }
        }
    }

    public void getAndAssignPlayerPawns() //Loads all of the prefab tiles and adds it into island deck list
    {
        var resources = Resources.LoadAll("PlayerPawns", typeof(GameObject));

        foreach (GameObject obj in resources)
        {
            playerPawns.Add(obj);
        }

        int rand = Random.Range(0, playerPawns.Count-1);
        player1.GetComponent<Player1>().pawn = playerPawns[rand]; //Randomly assigns a pawn to player 1

        rand = Random.Range(0, playerPawns.Count-1);
        player2.GetComponent<Player2>().pawn = playerPawns[rand];

        if (player1.GetComponent<Player1>().pawn == player2.GetComponent<Player2>().pawn) //Checks to make sure that player 2 does not get the same pawn as player 1
        {
            if (rand == playerPawns.Count-1)
            {
                player2.GetComponent<Player2>().pawn = playerPawns[playerPawns.Count-1];
            } else if (rand == 0) 
            {
                player2.GetComponent<Player2>().pawn = playerPawns[1];
            }
        }

    }

    public void spawnPawns() //Loops through the board array and finds the filled point that has the same player spawn string value of the pawn to spawn's name
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (Board[i,j].pointState == PointState.Filled && Board[i,j].cardAtPoint.GetComponent<CardS>().card.playerSpawn == player1.GetComponent<Player1>().pawn.name)
                {
                    Instantiate(player1.GetComponent<Player1>().pawn, new Vector3(Board[i,j].cardPosition.x, Board[i, j].cardPosition.y, -1), Quaternion.identity);
                }
            }
        }

        for (int i = 0; i < 6; i++) //Repeat the same for player 2
        {
            for (int j = 0; j < 6; j++)
            {
                if (Board[i, j].pointState == PointState.Filled && Board[i, j].cardAtPoint.GetComponent<CardS>().card.playerSpawn == player2.GetComponent<Player2>().pawn.name)
                {
                    Instantiate(player2.GetComponent<Player2>().pawn, new Vector3(Board[i, j].cardPosition.x, Board[i, j].cardPosition.y, -1), Quaternion.identity);
                }
            }
        }
    }

    public void handOutTwoTreasureCardsStart() 
    {
        Vector2 firstPlaceSpot = player1.GetComponent<Player1>().treasureCardSlots[player1.GetComponent<Player1>().amountOfTreasureCards];
        Instantiate(treasureStack.Pop(), new Vector3(firstPlaceSpot.x, firstPlaceSpot.y, 0), Quaternion.identity);
        player1.GetComponent<Player1>().amountOfTreasureCards++;

        Vector2 secondPlaceSpot = player1.GetComponent<Player1>().treasureCardSlots[player1.GetComponent<Player1>().amountOfTreasureCards];
        Instantiate(treasureStack.Pop(), new Vector3(secondPlaceSpot.x, secondPlaceSpot.y, 0), Quaternion.identity);
        player1.GetComponent<Player1>().amountOfTreasureCards++;

        firstPlaceSpot = player2.GetComponent<Player2>().treasureCardSlots[player2.GetComponent<Player2>().amountOfTreasureCards];
        Instantiate(treasureStack.Pop(), new Vector3(firstPlaceSpot.x, firstPlaceSpot.y, 0), Quaternion.identity);
        player2.GetComponent<Player2>().amountOfTreasureCards++;

        secondPlaceSpot = player2.GetComponent<Player2>().treasureCardSlots[player2.GetComponent<Player2>().amountOfTreasureCards];
        Instantiate(treasureStack.Pop(), new Vector3(secondPlaceSpot.x, secondPlaceSpot.y, 0), Quaternion.identity);
        player2.GetComponent<Player2>().amountOfTreasureCards++;
    }
}
