using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum CardDirection
{
  Up,
  Down,
  Left,
  Right,
}

public class Card : MonoBehaviour
{
  public PixelText text;

  public List<CardTrait> traits;
  public CardDirection facing;
  public int counter;

  public float MoveAnimationTime;

  public bool Has(CardTrait trait)
  {
    return traits.Contains(trait);
  }

  public async Task AnimateMove(Vector3 to)
  {
    var p0 = this.transform.localPosition;

    var t0 = Time.time;

    var delta = (to - p0);
    delta.z = 0;
    // var duration = MoveAnimationTime * delta.magnitude;
    var duration = MoveAnimationTime;
    // Debug.Log($"{name} {(to - p0).magnitude} {duration} {MoveAnimationTime} {p0} {to}");

    while (Time.time < t0 + duration)
    {
      var t = (Time.time - t0) / duration;

      this.transform.localPosition = Vector3.Lerp(p0, to, t);

      await Util.NextFrame();
    }

    this.transform.localPosition = to;
  }

  public static CardDirection NextClockwise(CardDirection a)
  {
    switch (a)
    {
      case CardDirection.Up: return CardDirection.Right;
      case CardDirection.Right: return CardDirection.Down;
      case CardDirection.Down: return CardDirection.Left;
      case CardDirection.Left: return CardDirection.Up;
      default: throw new System.Exception();
    }
  }

  public static Vector2Int GetValue(CardDirection a)
  {
    switch (a)
    {
      case CardDirection.Up: return new Vector2Int(0, -1);
      case CardDirection.Down: return new Vector2Int(0, 1);
      case CardDirection.Left: return new Vector2Int(-1, 0);
      case CardDirection.Right: return new Vector2Int(1, 0);
      default: throw new System.Exception();
    }
  }
}
