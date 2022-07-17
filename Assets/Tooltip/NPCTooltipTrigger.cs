using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCTooltipTrigger : MonoBehaviour
{

    [SerializeField] string nameString;
    [Multiline()]
    [SerializeField] string contentString;
    [SerializeField] string[] traits;


    [SerializeField] float awakeDelay = 0.25f;

    private bool stillHovering = false;

    IEnumerator DelayedShow()
    {
        yield return new WaitForSeconds(awakeDelay);
        if (stillHovering) TooltipSystem.ShowNPC(contentString, nameString);
    }

    private void Hide()
    {
        TooltipSystem.HideNPC();
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
    void Start()
    {
        stillHovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
