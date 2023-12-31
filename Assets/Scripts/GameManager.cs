using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Start,
    Won,
    Lost,
    Player1,
    Player2,
    Player3,
    Player4
}

public class GameManager : MonoBehaviour
{
    private List<Vector2> gridCoords = new List<Vector2>(); //Hardcoding of all the points to spawn tiles
    public List<GameObject> islandTileDeck = new List<GameObject>(); //A list of all of the island tiles (This served as the deck)
    private List<GameObject> floodDeck = new List<GameObject>(); //Same as above but for the flooding cards
    public Stack<GameObject> floodStack = new Stack<GameObject>(); //The actual deck representation for the flood deck. Stack to use Pop and Push (LIFO - for a deck)
    public List<GameObject> discardedFloodCards = new List<GameObject>(); //List of cards that have been pulled from the flooding deck (Popped) gets shuffled and pushed back onto the stack
    public GameState gameState;



   

    public enum PointState //An enum to define the different states a point can be in
    {
        Empty,
        Available,
        Filled,
        Sunk
    }

    public enum PlayerPawnHere
    {
        Red,
        White,
        Yellow,
        Blue,
        Black,
        Green,
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

        gameState = GameState.Start;

        createIslandDeck();
        shuffleIslands();
        populateBoard();
        createFloodDeck();
        shuffleFlood();
        createFloodStack();

        //DisplayBoard(Board);
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

    public void flood() //Prototype, all flooding related logic goes here
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
}
