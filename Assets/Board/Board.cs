using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

using Random = UnityEngine.Random;

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
  public float GuardRowOffset;

  public Card[,] cards;

  public ClickArea[] rowClickAreas;
  public ClickArea[,] cardClickAreas;

  public GameObject ClickAreaTemplate;

  public ActionSystem actionSystem;
  public Hand hand;
  public GuardController guard;
  public ScoreLifeSystem score;

  public ActionCard playing;
  public int playing_index;

  public int level;

  private bool[] clearRows;

  private int animations;

  PeopleManager peopleManager;

  public static (System.Action<Board>, System.Action<Board>)[] levels = {
    (board => {
      board.CheckpointLength = 1;
    },
    board => {
      board.CreateCard(0, 0, "human guard");
      board.CreateCard(1, 0, "human guard");
    }),

    (board => {
      board.CheckpointLength = 2;
    },
    board => {
      board.CreateCard(0, 0, "human guard");
      board.CreateCard(1, 0, "human guard");
      board.CreateCard(3, 1, "human guard");
      board.CreateCard(4, 1, "human guard");
    }),

    (board => {
      board.CheckpointLength = 2;
    },
    board => {
      board.CreateCard(0, 0, "human guard");
      board.CreateCard(0, 1, "human guard");
      board.CreateCard(1, 1, "human guard");
      board.CreateCard(3, 1, "human guard");
      board.CreateCard(4, 1, "human guard");
    })
  };

  void Start()
  {
    peopleManager = FindObjectOfType<PeopleManager>();

    levels[level].Item1(this);

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

        if (y >= CheckpointLength)
        {
          if (Random.value > 0.6f) continue;
          var name = PickCitizenCard();
          CreateCard(x, y, name);
        }
      }
    }

    levels[level].Item2(this);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space) && animations == 0)
    {
      ResolveStep();
    }
  }

  void ResolveStep()
  {
    GameManager.gm.PlaySound(GameManager.SoundEffects.guardAnnouncement);

    GameManager.gm.PlaySound(GameManager.SoundEffects.peopleShuffle);

    peopleManager.MoveLine();

    var done = new StepState[Width, LineLength + CheckpointLength];

    clearRows = rowClickAreas.Select(r => false).ToArray();

    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      for (int x = 0; x < Width; ++x)
      {
        ResolveStep(done, x, y);
      }
    }

    if (clearRows.Any(o => o))
    {
      score.currentLives -= 1;
      guard.GuardDisapproval();
    }
    else
    {
      score.score += 1;
      guard.GuardApproval();
    }

    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      if (clearRows[y])
      {
        for (int x = 0; x < Width; ++x)
        {
          if (cards[x, y] && !cards[x, y].Has(CardTrait.Police))
          {
            DestroyCard(x, y, cards[x, y], x, y);
          }
        }
      }
    }

    for (int x = 0; x < Width; ++x)
    {
      if (cards[x, LineLength + CheckpointLength - 1])
      {
        continue;
      }
    }

    for (int x = 0; x < Width; ++x)
    {
      GenerateCard(x);
    }

    hand.Draw1();
    actionSystem.RefreshAllActions();
  }

  string PickCitizenCard()
  {
    GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);

    var names = new string[] { "passport", "id", "luggage", "fingerprints" };

    var name = names[Random.Range(0, names.Length)];
    var fake = Random.value > 0.45f;

    return $"document {name}{(fake ? " fake" : "")}";
  }

  async void GenerateCard(int x)
  {
    if (Random.value > 0.6f) return;
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
      for (var i = 0; i < 4; ++i)
      {
        var dir = Card.GetValue(card.facing);

        if (Clip(x + dir.x, y + dir.y) && y + dir.y < CheckpointLength)
        {
          var wasDone = done[x + dir.x, y + dir.y];

          if (ResolveMove(done, x, y, card, x + dir.x, y + dir.y))
          {
            break;
          }

          done[x + dir.x, y + dir.y] = wasDone;
        }

        card.facing = Card.NextClockwise(card.facing);
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
      clearRows[toY] = true;
      DestroyCard(toX, toY, stand, toX, toY);
      DestroyCard(fromX, fromY, enter, toX, toY);
      return true;
    }
    else if (enter.Has(CardTrait.Anthrax) && stand.Has(CardTrait.Police))
    {
      clearRows[toY] = true;
      DestroyCard(toX, toY, stand, toX, toY);
      DestroyCard(fromX, fromY, enter, toX, toY);
      return true;
    }
    else if (stand.Has(CardTrait.Police) && enter.Has(CardTrait.Document))
    {
      if (enter.Has(CardTrait.Suspicious))
      {
        clearRows[toY] = true;
      }

      DestroyCard(fromX, fromY, enter, toX, toY);
      return true;
    }
    else if (enter.Has(CardTrait.Police) && stand.Has(CardTrait.Document))
    {
      if (stand.Has(CardTrait.Suspicious))
      {
        clearRows[toY] = true;
      }

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

    if (y < CheckpointLength)
    {
      to.y -= GuardRowOffset;
    }

    if (animate)
    {
      animations += 1;
      await card.AnimateMove(to);
      animations -= 1;
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
    actionSystem.CurrentActions -= 1;

    this.playing = action;
    this.playing.GetComponent<Card>().highlight = true;
    this.playing_index = 0;
    this.UpdateInput();
  }

  public void Cancel()
  {
    actionSystem.CurrentActions += 1;

    this.playing.GetComponent<Card>().highlight = true;
    this.playing = null;
    this.UpdateInput();
  }

  private void UpdateInput()
  {
    if (this.playing && this.playing_index == this.playing.arguments.Length)
    {
      var action = this.playing;
      var card = action.GetComponent<Card>();

      hand.cards.Remove(card);
      hand.deck.DiscardCard(card.name);
      Destroy(card.gameObject);

      this.playing = null;

      for (var i = 0; i < hand.cards.Count; ++i)
      {
        var _ = hand.UpdatePosition(hand.cards[i], i, i, true);
      }

      var cost = 0;
      for (var i = 0; i < action.values.Length; ++i)
      {
        if (action.values[i] is Vector2Int v)
        {
          cost += GetCost(v.y);
        }
        else if (action.values[i] is int y)
        {
          cost += GetCost(y);
        }
      }

      actionSystem.CurrentActions -= cost;

      action.Apply(this);
      UpdateClickAreas((ActionCardArgument.None, (board, o) => false));
    }
    else if (this.playing)
    {
      var argument = this.playing.arguments[this.playing_index];
      UpdateClickAreas(argument);
    }
    else
    {
      UpdateClickAreas((ActionCardArgument.None, (board, o) => false));
    }
  }

  public int GetCost(int row)
  {
    if (row == CheckpointLength)
      return 2;

    if (row == CheckpointLength + 1)
      return 1;

    return 0;
  }

  private void UpdateClickAreas((ActionCardArgument, Func<Board, System.Object, bool>) arg)
  {
    for (int y = 0; y < LineLength + CheckpointLength; ++y)
    {
      rowClickAreas[y].Enable(arg.Item1 == ActionCardArgument.Row && arg.Item2(this, y));

      for (int x = 0; x < Width; ++x)
      {
        if (arg.Item1 == ActionCardArgument.Cell)
        {
          cardClickAreas[x, y].Enable(arg.Item2(this, new Vector2Int(x, y)));
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
