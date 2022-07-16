using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Hand : MonoBehaviour
{
  public string[] InitialCards;
  public float CardSpacing;

  public Card[] cards;

  // Start is called before the first frame update
  void Start()
  {
    cards = InitialCards.Select(name =>
    {
      return GameManager.gm.MakeCard(name, gameObject);
    }).ToArray();

    for (var i = 0; i < cards.Length; ++i)
    {
      var _ = UpdatePosition(cards[i], i, i, true);
    }
  }

  async Task UpdatePosition(Card card, int index, int z, bool animate)
  {
    var range = cards.Length * CardSpacing;
    var to = new Vector3(
      (-(cards.Length - 1) / 2f + index) * CardSpacing,
      0,
      z
    );

    if (animate)
    {
      await card.AnimateMove(to);
    }
    else
    {
      card.transform.localPosition = to;
    }
  }
}
