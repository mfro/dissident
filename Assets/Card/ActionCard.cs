using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum ActionCardArgument
{
  None,
  Row,
  Cell,
  EmptyCell,
}

public abstract class ActionCard : MonoBehaviour
{
  public abstract (ActionCardArgument, Func<Board, System.Object, bool>)[] arguments { get; }
  public System.Object[] values;

  public abstract void Apply(Board board);

  void Start()
  {
    values = new System.Object[arguments.Length];
  }

  void OnMouseDown()
  {
    var card = this.GetComponent<Card>();
    if (card.board.playing == this)
    {
      card.board.Cancel();
    }
    else if (CanPlay())
    {
      card.hand.Play(this);
    }
  }

  void OnMouseEnter()
  {
    var card = this.GetComponent<Card>();
    this.GetComponent<Card>().highlight = card.board.playing == this || CanPlay();
  }

  void OnMouseExit()
  {
    var card = this.GetComponent<Card>();
    this.GetComponent<Card>().highlight = card.board.playing == this;
  }

  bool CanPlay()
  {
    var card = this.GetComponent<Card>();
    if (card.board.actionSystem.CurrentActions == 0)
      return false;

    if (card.board.playing)
      return false;

    if (!card.hand)
      return false;

    return true;
  }
}
