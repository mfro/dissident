using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum StepState
{
  Waiting,
  Doing,
  Done,
}

public class Board : MonoBehaviour
{
  public int Width;
  public int LineLength;
  public int CheckpointLength;

  public float LineDelay;

  public Vector2 GridCellSize;

  public Card[,] cards;

  public ClickArea[] rowClickAreas;
  public ClickArea[,] cardClickAreas;

  public GameObject ClickAreaTemplate;

  public ActionCard playing;
  public int playing_index;

  void Start()
  {
    cards = new Card[Width, LineLength + CheckpointLength];

    rowClickAreas = new ClickArea[LineLength + CheckpointLength];
    cardClickAreas = new ClickArea[Width, LineLength + CheckpointLength];

    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      var input_row = y;
      rowClickAreas[y] = Instantiate(ClickAreaTemplate, this.transform).GetComponent<ClickArea>();
      rowClickAreas[y].transform.localPosition = new Vector3(0, ((LineLength + CheckpointLength - 1) / 2f - y) * GridCellSize.y, -2);
      rowClickAreas[y].transform.localScale = new Vector3(Width * GridCellSize.x, GridCellSize.y, 1);
      rowClickAreas[y].GetComponent<ClickArea>().callback = () => ProcessInput(input_row);
      rowClickAreas[y].Enable(false);

      for (int x = 0; x < Width; ++x)
      {
        cardClickAreas[x, y] = Instantiate(ClickAreaTemplate, this.transform).GetComponent<ClickArea>();

        cardClickAreas[x, y].transform.localPosition = new Vector3(
          (-(Width - 1) / 2f + x) * GridCellSize.x,
          ((LineLength + CheckpointLength - 1) / 2f - y) * GridCellSize.y,
          -2
        );

        cardClickAreas[x, y].transform.localScale = new Vector3(GridCellSize.x, GridCellSize.y, 1);

        var input_cell = new Vector2Int(x, y);
        cardClickAreas[x, y].GetComponent<ClickArea>().callback = () => ProcessInput(input_cell);

        cardClickAreas[x, y].Enable(false);

        if (y < CheckpointLength)
        {
          if (Random.value > 0.25f) continue;
          cards[x, y] = GameManager.gm.MakeCard("human guard", gameObject);
          cards[x, y].facing = y == 0 ? CardDirection.Right : CardDirection.Left;
        }
        else
        {
          if (Random.value > 0.4f) continue;
          var name = PickCitizenCard();
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
      ResolveStep();
    }
  }

  void ResolveStep()
  {
    var done = new StepState[Width, LineLength + CheckpointLength];

    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      for (int x = 0; x < Width; ++x)
      {
        ResolveStep(done, x, y);
      }
    }


    for (int x = 0; x < Width; ++x)
    {
      if (cards[x, LineLength + CheckpointLength - 1])
      {
        return;
      }
    }

    for (int x = 0; x < Width; ++x)
    {
      GenerateCard(x);
    }
  }

  string PickCitizenCard()
  {
    var names = new string[] { "passport", "id", "luggage", "fingerprints" };

    var name = names[Random.Range(0, names.Length)];
    var fake = Random.value > 0.6f;

    return $"document {name}{(fake ? " fake" : "")}";
  }

  async void GenerateCard(int x)
  {
    if (Random.value > 0.4f) return;
    var name = PickCitizenCard();
    CreateCard(x, LineLength + CheckpointLength - 1, name);

    await UpdatePosition(cards[x, LineLength + CheckpointLength - 1], Width, LineLength + CheckpointLength, 9 - x, false);
    await Util.Seconds(0.1f * LineLength);
    await UpdatePosition(cards[x, LineLength + CheckpointLength - 1], Width, LineLength + CheckpointLength - 1, 9 - x, true);
    await UpdatePosition(cards[x, LineLength + CheckpointLength - 1], x, LineLength + CheckpointLength - 1, 0, true);
  }

  public void CreateCard(int x, int y, string name)
  {
    cards[x, y] = GameManager.gm.MakeCard(name, gameObject);
    cards[x, y].board = this;

    var _ = UpdatePosition(cards[x, y], x, y, 0, false);
  }

  void ResolveStep(StepState[,] done, int x, int y)
  {
    if (done[x, y] != StepState.Waiting) return;

    var card = cards[x, y];
    if (!card)
    {
      done[x, y] = StepState.Done;
      return;
    }

    done[x, y] = StepState.Doing;

    if (card.Has(CardTrait.Patrol))
    {
      var dir = Card.GetValue(card.facing);
      var wasDone = Clip(x + dir.x, y + dir.y) ? done[x + dir.x, y + dir.y] : StepState.Waiting;

      if (y + dir.y >= CheckpointLength || !ResolveMove(done, x, y, card, x + dir.x, y + dir.y))
      {
        if (Clip(x + dir.x, y + dir.y))
          done[x + dir.x, y + dir.y] = wasDone;

        Vector2Int next;

        while (true)
        {
          card.facing = Card.NextClockwise(card.facing);
          next = Card.GetValue(card.facing);
          if (Clip(x + next.x, y + next.y) && y + next.y < CheckpointLength)
          {
            ResolveMove(done, x, y, card, x + next.x, y + next.y);
            break;
          }
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
      DestroyCard(x, y, card, x, -1);
    }

    done[x, y] = StepState.Done;
  }

  bool Clip(int x, int y)
  {
    return x >= 0 && x < Width && y >= 0 && y < LineLength + CheckpointLength;
  }

  bool ResolveMove(StepState[,] done, int fromX, int fromY, Card card, int toX, int toY)
  {
    if (toX < 0 || toX >= Width || toY < 0 || toY >= LineLength + CheckpointLength)
      return false;

    if (done[toX, toY] == StepState.Doing)
      return false;

    ResolveStep(done, toX, toY);

    var collision = cards[toX, toY];
    if (collision)
    {
      return ResolveCollision(fromX, fromY, card, toX, toY, collision);
    }
    else
    {
      MoveCard(fromX, fromY, card, toX, toY);
      return true;
    }
  }

  bool ResolveCollision(int fromX, int fromY, Card enter, int toX, int toY, Card stand)
  {
    if (enter.Has(CardTrait.Alive) && stand.Has(CardTrait.Anthrax))
    {
      DestroyCard(toX, toY, stand, toX, toY);
      DestroyCard(fromX, fromY, enter, toX, toY);
      return true;
    }
    else if (enter.Has(CardTrait.Anthrax) && stand.Has(CardTrait.Police))
    {
      DestroyCard(toX, toY, stand, toX, toY);
      DestroyCard(fromX, fromY, enter, toX, toY);
      return true;
    }
    else if (stand.Has(CardTrait.Police))
    {
      DestroyCard(fromX, fromY, enter, toX, toY);
      return true;
    }
    else if (enter.Has(CardTrait.Police))
    {
      DestroyCard(toX, toY, stand, toX, toY);
      MoveCard(fromX, fromY, enter, toX, toY);
      return true;
    }

    return false;
  }

  async void MoveCard(int fromX, int fromY, Card card, int toX, int toY)
  {
    cards[fromX, fromY] = null;
    cards[toX, toY] = card;

    if (fromY > CheckpointLength)
    {
      await Util.Seconds(LineDelay * (fromY - CheckpointLength));
    }

    await UpdatePosition(card, toX, toY, 0, true);
  }

  public async Task UpdatePosition(Card card, int x, int y, float z, bool animate)
  {
    var to = new Vector3(
      (-(Width - 1) / 2f + x) * GridCellSize.x,
      ((LineLength + CheckpointLength - 1) / 2f - y) * GridCellSize.y,
      y * 10 + z
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

  public void Play(ActionCard action)
  {
    this.playing = action;
    this.playing_index = 0;
    this.UpdateInput();
  }

  private void UpdateInput()
  {
    if (this.playing_index == this.playing.arguments.Length)
    {
      var action = this.playing;
      this.playing = null;
      action.Apply(this);
      UpdateClickAreas(ActionCardArgument.None);
    }
    else
    {
      var argument = this.playing.arguments[this.playing_index];
      UpdateClickAreas(argument);
    }
  }

  private void UpdateClickAreas(ActionCardArgument arg)
  {
    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      rowClickAreas[y].Enable(arg == ActionCardArgument.Row);

      for (int x = 0; x < Width; ++x)
      {
        if (arg == ActionCardArgument.Cell)
        {
          cardClickAreas[x, y].Enable(true);
        }
        else if (arg == ActionCardArgument.EmptyCell)
        {
          cardClickAreas[x, y].Enable(cards[x, y] == null);
        }
        else
        {
          cardClickAreas[x, y].Enable(false);
        }
      }
    }
  }

  public void ProcessInput(System.Object value)
  {
    this.playing.values[this.playing_index] = value;
    this.playing_index += 1;
    this.UpdateInput();
  }

  private static bool MatchCollision(Card enter, Card stand, CardTrait one, CardTrait two)
  {
    return enter.Has(one) && stand.Has(two) || enter.Has(two) && stand.Has(one);
  }
}
