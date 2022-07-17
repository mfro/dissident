using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTooltipTrigger : MonoBehaviour
{

    private Card card;

    [SerializeField] Sprite portrait;
    [SerializeField] string effectText;

    [SerializeField] float awakeDelay = 0.25f;

    IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(awakeDelay);

        string cardName = card.text.text;
        TooltipSystem.ShowCard(portrait, effectText, card.traits, card.text.text);
    }

    private void Hide()
    {
        TooltipSystem.HideCard();
    }

    public void OnMouseEnter()
    {
        StartCoroutine(DelayedShow());
        Hide();
    }

    public void OnMouseExit()
    {
        StopCoroutine(DelayedShow());
        Hide();
    }

    // Start is called before the first frame update
    void Awake()
    {
        card = GetComponent<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
