using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickArea : MonoBehaviour
{
  public Action callback;

  public void Enable(bool enabeld)
  {
    gameObject.SetActive(enabeld);

    var sprite = GetComponent<SpriteRenderer>();
    var color = sprite.color;
    color.a = 0f;
    sprite.color = color;
  }

  void OnMouseDown()
  {
    callback?.Invoke();
  }

  void OnMouseEnter()
  {
    var sprite = GetComponent<SpriteRenderer>();
    var color = sprite.color;
    color.a = 0.2f;
    sprite.color = color;
  }

  void OnMouseExit()
  {
    var sprite = GetComponent<SpriteRenderer>();
    var color = sprite.color;
    color.a = 0f;
    sprite.color = color;
  }
}
