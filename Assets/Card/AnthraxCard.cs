using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AnthraxCard : ActionCard
{
  public override (ActionCardArgument, Func<Board, System.Object, bool>)[] arguments => new (ActionCardArgument, Func<Board, System.Object, bool>)[] {
    (ActionCardArgument.Cell, (board, o) => {
      var space = (Vector2Int)o;
      return space.y >= board.CheckpointLength
        && board.cards[space.x, space.y] == null;
    }),
  };

  public override void Apply(Board board)
  {
    var space = (Vector2Int)values[0];

    Debug.Log(space);

    board.CreateCard(space.x, space.y, "action anthrax");
  }
}
