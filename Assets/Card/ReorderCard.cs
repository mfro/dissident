using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ReorderCard : ActionCard
{
  public override ActionCardArgument[] arguments => new ActionCardArgument[] {
    ActionCardArgument.Row,
    ActionCardArgument.Row,
  };

  public override void Apply(Board board)
  {
    var row1 = (int)values[0];
    var row2 = (int)values[1];

    for (var x = 0; x < board.Width; ++x)
    {
      var value = board.cards[x, row1];
      board.cards[x, row1] = board.cards[x, row2];
      board.cards[x, row2] = value;

      var _1 = board.UpdatePosition(board.cards[x, row1], x, row1, 0, true);
      var _2 = board.UpdatePosition(board.cards[x, row2], x, row2, -0.1f, true);
    }
  }
}
