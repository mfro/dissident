using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Hand : MonoBehaviour
{
  public string[] InitialCards;
  public float CardSpacing;

  public List<Card> cards;

  public Board board;
  public Deck deck;

  // Start is called before the first frame update
  void Start()
  {
    for (var i = 0; i < 4; ++i)
    {
      Draw1();
    }
  }

  public void Draw1()
  {
    var name = deck.DrawCard();
    if (name == null) return;

    var card = GameManager.gm.MakeCard(name, gameObject);
    card.board = board;
    card.hand = this;
    cards.Add(card);

    for (var i = 0; i < cards.Count; ++i)
    {
      var _ = UpdatePosition(cards[i], i, i, true);
    }
  }

  public void Play(ActionCard action)
  {
    var card = action.GetComponent<Card>();
    cards.Remove(card);
    card.board.Play(action);

    deck.DiscardCard(card.name);
    Destroy(card.gameObject);

    for (var i = 0; i < cards.Count; ++i)
    {
      var _ = UpdatePosition(cards[i], i, i, true);
    }
  }

  async Task UpdatePosition(Card card, int index, int z, bool animate)
  {
    var range = cards.Count * CardSpacing;
    var to = new Vector3(
      (-(cards.Count - 1) / 2f + index) * CardSpacing,
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
