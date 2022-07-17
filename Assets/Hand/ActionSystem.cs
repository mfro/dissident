using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{

  public int maxActions;

  private int currentActions;
  public int CurrentActions
  {
    get => currentActions;
    set
    {
      currentActions = value;
      UpdateUI();
    }
  }

  [SerializeField] private PixelText label;

  public void RefreshAllActions()
  {
    CurrentActions = maxActions;
  }

  private void UpdateUI()
  {
    label.text = currentActions.ToString() + " / " + maxActions.ToString();
  }

  void Start()
  {
    RefreshAllActions();
  }
}
