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
  public abstract ActionCardArgument[] arguments { get; }
  public System.Object[] values;

  public abstract void Apply(Board board);

  void Start()
  {
    values = new System.Object[arguments.Length];
  }

  void OnMouseDown()
  {
    var card = this.GetComponent<Card>();
    card.board.Play(this);
  }
}
