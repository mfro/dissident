using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SwapCard : ActionCard
{
  public override ActionCardArgument[] arguments => new ActionCardArgument[] {
    ActionCardArgument.Cell,
    ActionCardArgument.Cell,
  };

  public override void Apply(Board board)
  {
    var cell1 = (Vector2Int)values[0];
    var cell2 = (Vector2Int)values[1];

    var value = board.cards[cell1.x, cell1.y];
    board.cards[cell1.x, cell1.y] = board.cards[cell2.x, cell2.y];
    board.cards[cell2.x, cell2.y] = value;

    var _1 = board.UpdatePosition(board.cards[cell1.x, cell1.y], cell1.x, cell1.y, 0, true);
    var _2 = board.UpdatePosition(board.cards[cell2.x, cell2.y], cell2.x, cell2.y, -0.1f, true);
  }
}
