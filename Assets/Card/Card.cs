using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Card : MonoBehaviour
{
  public PixelText text;

  public List<CardTrait> traits;
  public bool facing;
  public int counter;

  public float MoveAnimationTime;

  void Start()
  {
  }

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
