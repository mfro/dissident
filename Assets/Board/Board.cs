using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
  public int Width;
  public int LineLength;
  public int CheckpointLength;

  public Vector2 GridCellSize;

  public Card[,] cards;

  void Start()
  {
    cards = new Card[Width, LineLength + CheckpointLength];

    // cards[0, 0] = MakeCard("human guard");

    // cards[2, 2] = MakeCard("passport");
    // cards[3, 3] = MakeCard("fake passport");

    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      for (int x = 0; x < Width; ++x)
      {
        if (y == 0)
        {
          cards[x, y] = GameManager.gm.MakeCard("human guard", gameObject);
        }
        else
        {
          var name = Random.value > 0.5f ? "passport" : "fake passport";
          cards[x, y] = GameManager.gm.MakeCard(name, gameObject);
        }

        if (!cards[x, y]) continue;

        var _ = UpdatePosition(cards[x, y], x, y, 0, false);
      }
    }
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
    var done = new bool[Width, LineLength + CheckpointLength];

    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      for (int x = 0; x < Width; ++x)
      {
        ResolveStep(done, x, y);
      }
    }
  }

  void ResolveStep(bool[,] done, int x, int y)
  {
    if (done[x, y]) return;
    done[x, y] = true;

    var card = cards[x, y];
    if (!card) return;

    if (card.Has(CardTrait.Patrol))
    {
      if (card.facing)
      {
        if (x + 1 == Width)
        {
          card.facing = !card.facing;
          ResolveMove(done, x, y, card, x - 1, y);
        }
        else
        {
          ResolveMove(done, x, y, card, x + 1, y);
        }
      }
      else
      {
        if (x == 0)
        {
          card.facing = !card.facing;
          ResolveMove(done, x, y, card, x + 1, y);
        }
        else
        {
          ResolveMove(done, x, y, card, x - 1, y);
        }
      }
    }
    else if (card.Has(CardTrait.Static))
    {
      if (card.counter > 0)
      {
        card.counter -= 1;
        if (card.counter == 0)
        {
          card.traits.Remove(CardTrait.Static);
        }
      }
    }
    else if (y > 0)
    {
      ResolveMove(done, x, y, card, x, y - 1);
    }
    else
    {
      Destroy(card.gameObject);
    }
  }

  void ResolveMove(bool[,] done, int fromX, int fromY, Card card, int toX, int toY)
  {
    ResolveStep(done, toX, toY);

    var collision = cards[toX, toY];
    if (collision)
    {
      ResolveCollision(fromX, fromY, collision, toX, toY, card);
    }
    else
    {
      MoveCard(fromX, fromY, card, toX, toY);
    }
  }

  void ResolveCollision(int fromX, int fromY, Card enter, int toX, int toY, Card stand)
  {
    Debug.Log($"collide {enter.name} into {stand.name}");

    if (enter.Has(CardTrait.Document) && stand.Has(CardTrait.Police))
    {
      DestroyCard(fromX, fromY, enter, toX, toY);
    }
    else if (enter.Has(CardTrait.Police) && stand.Has(CardTrait.Document))
    {
      DestroyCard(toX, toY, stand, toX, toY);
      MoveCard(fromX, fromY, enter, toX, toY);
    }
  }

  async void MoveCard(int fromX, int fromY, Card card, int toX, int toY)
  {
    cards[fromX, fromY] = null;
    cards[toX, toY] = card;

    await Util.Seconds(0.1f * fromY);

    await UpdatePosition(card, toX, toY, 0, true);
  }

  async Task UpdatePosition(Card card, int x, int y, int z, bool animate)
  {
    var to = new Vector3(
      (-(Width - 1) / 2f + x) * GridCellSize.x,
      ((LineLength + CheckpointLength - 1) / 2f - y) * GridCellSize.y,
      z
    );

    if (animate)
    {
      await card.AnimateMove(to);
    }
    else
    {
      card.transform.localPosition = to;
    }
  }

  async void DestroyCard(int fromX, int fromY, Card card, int toX, int toY)
  {
    cards[fromX, fromY] = null;

    await UpdatePosition(card, toX, toY, 1, true);

    Destroy(card.gameObject);
  }

  private static bool MatchCollision(Card enter, Card stand, CardTrait one, CardTrait two)
  {
    return enter.Has(one) && stand.Has(two) || enter.Has(two) && stand.Has(one);
  }
}
