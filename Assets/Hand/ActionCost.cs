using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCost : MonoBehaviour
{
    // Start is called before the first frame update

    public int actionCost;

    [SerializeField] private Sprite[] actionCostSprites;

    [SerializeField] private SpriteRenderer sr;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Assert(actionCost >= 0 && actionCost <= 5);

        if (actionCost > 0)
        {
            sr.enabled = true;
            sr.sprite = actionCostSprites[actionCost - 1];
        }
        else
        {
            sr.enabled = false;
        }
    }
}
