using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
  [SerializeField] public List<string> allCards;

  [SerializeField] Sprite[] deckSprites;

  private Queue<string> deck;
  private List<string> discard;

  private SpriteRenderer sr;

  // Start is called before the first frame update
  void Start()
  {
    sr = GetComponent<SpriteRenderer>();

    deck = new Queue<string>();
    discard = new List<string>();

    Shuffle(allCards.ToList());

    Debug.Assert(deck.Count == allCards.Count);
    UpdateSprite();
  }

  //   public int GetDeckSize()
  //   {
  //     return currentDeck.Count;
  //   }

  //   public int GetDiscardSize()
  //   {
  //     return currentDiscard.Count;
  //   }

  private void Shuffle(List<string> pool)
  {
    while (pool.Count > 0)
    {
      int index = Random.Range(0, pool.Count);

      deck.Enqueue(pool[index]);
      pool.RemoveAt(index);
    }
  }

  public void ShuffleDiscard()
  {
    GameManager.gm.PlaySound(GameManager.SoundEffects.cardShuffle);

    Shuffle(discard);
  }

  public void DiscardCard(string name)
  {
    discard.Add(name);
  }

  public string DrawCard()
  {
    if (deck.Count == 0 && discard.Count > 0)
      ShuffleDiscard();

    if (!deck.TryDequeue(out var name))
      return null;

    UpdateSprite();
    return name;
  }

  public void UpdateSprite()
  {
    if (deck.Count >= deckSprites.Length)
    {
      sr.sprite = deckSprites[deckSprites.Length - 1];
    }
    else
    {
      sr.sprite = deckSprites[deck.Count];
    }
  }
}
