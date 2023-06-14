using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardS : MonoBehaviour //Script that attaches to card gameobjects/prefabs
{
    public CardSO card; //Variable to hold a scriptable object of a card
    public SpriteRenderer spriteRenderer; //Getting the spriterender so we can alter the sprite for flooding
    public TextMeshProUGUI cardText; //TMP to alter the text of a card

    public void Start()
    {
        card.cardFlooded = false; //Set that the card is not flooded
        cardText = gameObject.GetComponentInChildren<TextMeshProUGUI>(); //Fetch the TMP attached to this card
        cardText.text = card.cardName; //Set the card text on the TMP to the card's name
        spriteRenderer = GetComponent<SpriteRenderer>(); //Get the sprite renderer for the card sprite
    }

    public void flood() //Function containing the logic of what needs to happen to a card if it floodst
    {
        if (card.cardFlooded == false) //If it hasn't been flooded
        {
            card.cardFlooded = true;
            spriteRenderer.color = new Color(0f, 0f, 1f); //Flood it and change its colour to blue

        } else if (card.cardFlooded == true)
        {
            Destroy(gameObject); //If it has been flooded it needs to sink and be destroyed
        }
    }
}
