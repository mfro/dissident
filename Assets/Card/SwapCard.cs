using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SwapCard : ActionCard
{
  // public override ActionCardArgument[] arguments => new ActionCardArgument[] {
  //   ActionCardArgument.Cell,
  //   ActionCardArgument.Cell,
  // };
  public override (ActionCardArgument, Func<Board, System.Object, bool>)[] arguments => new (ActionCardArgument, Func<Board, System.Object, bool>)[] {
    (ActionCardArgument.Cell, (board, o) => {
      var space = (Vector2Int)o;
      return space.y >= board.CheckpointLength
        && board.cards[space.x, space.y]
        && board.cards[space.x, space.y].Has(CardTrait.Document);
    }),

    (ActionCardArgument.Cell, (board, o) => {
      var space = (Vector2Int)o;
      var other = (Vector2Int)values[0];
      return space.y >= board.CheckpointLength
        && board.cards[space.x, space.y]
        && board.cards[space.x, space.y].Has(CardTrait.Document)
        && (board.cards[space.x, space.y].name.Contains(board.cards[other.x, other.y].name) || board.cards[other.x, other.y].name.Contains(board.cards[space.x, space.y].name));
    }),
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
