using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AnthraxCard : ActionCard
{
  public override ActionCardArgument[] arguments => new ActionCardArgument[] {
    ActionCardArgument.EmptyCell,
  };

  public override void Apply(Board board)
  {
    var space = (Vector2Int)values[0];

    Debug.Log(space);

    board.CreateCard(space.x, space.y, "action anthrax");
  }
}
