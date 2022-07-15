using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
  public static GameObject[] CardPrefabs;

  public int width;
  public int line_length;
  public int checkpoint_length;

  public Card[,] cards;

  // Start is called before the first frame update
  void Start()
  {
    CardPrefabs = Resources.LoadAll<GameObject>("Cards");

    cards = new Card[width, line_length + checkpoint_length];

    cards[0, 0] = MakeCard("human guard");

    cards[2, 2] = MakeCard("passport");
    cards[3, 3] = MakeCard("fake passport");

    StepAllCards();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      StepAllCards();
    }
  }

  void StepAllCards()
  {
    var done = new bool[width, line_length + checkpoint_length];

    for (int y = 0; y < line_length + checkpoint_length; ++y)
    {
      for (int x = 0; x < width; ++x)
      {

        if (card)
        {
          ResolveStep(done, x, y, card);
        }
      }
    }
  }

  void ResolveStep(bool[,] done, int x, int y)
  {
    if (done[x, y]) return;

    var card = cards[x, y];
    if (!card) return;

    if (card.traits.Contains(CardTrait.Patrol))
    {
      if (card.facing)
      {
        if (x + 1 == width)
        {
          card.facing = !card.facing;
          ResolveStep(done, x - 1, y);
          ResolveMove(card, x - 1, y);
        }
        else
        {
          ResolveStep(done, x - 1, y);
          ResolveMove(card, x + 1, y);
        }
      }
      else
      {
        if (x == 0)
        {
          card.facing = !card.facing;
          ResolveStep(done, x - 1, y);
          ResolveMove(card, x + 1, y);
        }
        else
        {
          ResolveStep(done, x - 1, y);
          ResolveMove(card, x - 1, y);
        }
      }
    }

    else if (y > 0)
    {
      ResolveStep(done, x - 1, y);
      ResolveMove(card, x, y);
    }
    else
    {
      Destroy(card.gameObject);
    }
  }

  void ResolveMove(Card card, int x, int y)
  {
    var collision = cards[x, y];
    if (collision)
    {
      ResolveCollision(collision, card);
    }
    else
    {
      MoveCard(card, x, y);
    }
  }

  void ResolveCollision(Card stand, Card enter)
  {

  }

  void MoveCard(Card card, int x, int y)
  {
    Debug.Log($"{card.name} {x} {y}");

    cards[x, y] = card;

    card.gameObject.transform.localPosition = new Vector2(x, y);
  }

  public Card MakeCard(string name)
  {
    var prefab = CardPrefabs.First(o => o.name == name);
    var instance = Instantiate(prefab, this.transform);

    return instance.GetComponent<Card>();
  }
}
