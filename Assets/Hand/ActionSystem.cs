using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{

    [SerializeField]
    private int maxActions;
    [SerializeField]
    private int currentActions;

    [SerializeField] private PixelText label; 

    public void AdjustActions(int amount)
    {
        currentActions += amount;
        UpdateUI();
    }

    public void RefreshAllActions()
    {
        AdjustActions(maxActions - currentActions);
    }

    public int GetCurrentActions()
    {
        return currentActions;
    }

    public int GetMaxActions()
    {
        return maxActions;
    }

    private void UpdateUI()
    {
        label.text = currentActions.ToString() + " / " + maxActions.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshAllActions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
