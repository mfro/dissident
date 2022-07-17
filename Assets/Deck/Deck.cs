using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    [SerializeField] public List<Card> allCards;

    [SerializeField] Sprite[] deckSprites;

    private Stack<Card> currentDeck;
    private Stack<Card> currentDiscard;

    private SpriteRenderer sr;

    public void ShuffleEntireDeck()
    {
        currentDeck.Clear();
        currentDiscard.Clear();

        List<Card> tempCards = new List<Card>(allCards);

        while(tempCards.Count > 0)
        {
            int index = Random.Range(0, tempCards.Count);

            currentDeck.Push(tempCards[index]);
            tempCards.RemoveAt(index);
        }

        Debug.Assert(currentDeck.Count == allCards.Count);
        UpdateSprite();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentDeck = new Stack<Card>();
        currentDiscard = new Stack<Card>();
        sr = GetComponent<SpriteRenderer>();
        ShuffleEntireDeck();
    }

    public int GetDeckSize()
    {
        return currentDeck.Count;
    }

    public int GetDiscardSize()
    {
        return currentDiscard.Count;
    }

    public void DiscardCard(Card card)
    {
        currentDiscard.Push(card);
    }

    public Card DrawCard()
    {
        if (currentDeck.Count == 0)
        {
            return null;
        }

        UpdateSprite();
        return currentDeck.Pop();
    }

    public void UpdateSprite()
    {
        if(currentDeck.Count >= deckSprites.Length)
        {
            sr.sprite = deckSprites[deckSprites.Length - 1];
        } 
        else
        {
            sr.sprite = deckSprites[currentDeck.Count];
        }
    }
}
