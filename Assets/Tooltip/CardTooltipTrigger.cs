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

    private bool stillHovering;

    IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(awakeDelay);

        string cardName = card.text.text;
        if(stillHovering) TooltipSystem.ShowCard(portrait, effectText, card.traits, card.text.text);
    }

    private void Hide()
    {
        TooltipSystem.HideCard();
    }

    public void OnMouseEnter()
    {
        stillHovering = true;
        StartCoroutine(DelayedShow());
    }

    public void OnMouseExit()
    {
        stillHovering = false;
        Hide();
    }

    // Start is called before the first frame update
    void Awake()
    {
        card = GetComponent<Card>();
        stillHovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
