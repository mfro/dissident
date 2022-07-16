using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Card : MonoBehaviour
{
  public SpriteRenderer sprite;

  public CardTrait[] traits;
  public bool facing;

  public float MoveAnimationTime;

  public bool Has(CardTrait trait)
  {
    return traits.Contains(trait);
  }

  public async Task AnimateMove(Vector3 to)
  {
    var p0 = this.transform.localPosition;

    var t0 = Time.time;

    while (Time.time < t0 + MoveAnimationTime)
    {
      var t = (Time.time - t0) / MoveAnimationTime;

      this.transform.position = p0;
      this.transform.localPosition = Vector3.Lerp(p0, to, t);

      await Util.NextFrame();
    }

    this.transform.localPosition = to;
  }
}
