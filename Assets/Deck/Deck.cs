using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    [SerializeField] private List<Card> allCards;

    private Stack<Card> currentDeck;
    private Stack<Card> currentDiscard;

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
    }

    // Start is called before the first frame update
    void Start()
    {
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
        return currentDeck.Pop();
    }

}
