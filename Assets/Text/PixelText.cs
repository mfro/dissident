using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum TextAlignment
{
  Left,
  Center,
  Right,
}

[ExecuteAlways]
public class PixelText : MonoBehaviour
{
  [SerializeField]
  private string _text;

  public Texture2D font;
  public string text
  {
    get => _text;
    set { _text = value; ReDraw(); }
  }

  public TextAlignment alignment = TextAlignment.Left;

  private const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  private const string LOWER = "abcdefghijklmnopqrstuvwxyz";
  private const string OTHER = "0123456789.,!?'&()/-";
  private Sprite[] upper;
  private Sprite[] lower;
  private Sprite[] other;

  private static Dictionary<char, int> kerning = new Dictionary<char, int>
  {
    [' '] = 3,

    ['E'] = 7,
    ['F'] = 7,
    ['I'] = 2,
    ['J'] = 6,
    ['L'] = 7,
    ['M'] = 9,
    ['O'] = 9,
    ['Q'] = 9,
    ['W'] = 9,
    ['Z'] = 7,

    ['b'] = 7,
    ['d'] = 7,
    ['f'] = 4,
    ['g'] = 7,
    ['h'] = 7,
    ['i'] = 2,
    ['j'] = 2,
    ['l'] = 2,
    ['m'] = 9,
    ['n'] = 7,
    ['p'] = 7,
    ['q'] = 7,
    ['r'] = 4,
    ['t'] = 3,
    ['u'] = 7,
    ['w'] = 8,

    ['1'] = 4,
    ['.'] = 2,
    [','] = 3,
    ['!'] = 2,
    ['\''] = 2,
    ['&'] = 8,
    ['('] = 3,
    [')'] = 3,
    ['/'] = 3,
    ['-'] = 4,
  };

  private Sprite[] MakeSprites(string alphabet, int y_offset, int default_width, int height)
  {
    var offset = 0;
    var sprites = new Sprite[alphabet.Length];
    for (var i = 0; i < alphabet.Length; ++i)
    {
      var width = kerning.GetValueOrDefault(alphabet[i], default_width);
      sprites[i] = Sprite.Create(font, new Rect(offset, y_offset, width, height), new Vector2(0, 0), 32, 0, SpriteMeshType.Tight, Vector4.zero, false);
      offset += width;
    }

    return sprites;
  }

  private List<GameObject> children;
  private void Draw()
  {
    if (upper == null || upper[0] == null)
    {
      upper = MakeSprites(UPPER, 24, 8, 11);
      lower = MakeSprites(LOWER, 12, 6, 12);
      other = MakeSprites(OTHER, 0, 6, 12);
    }

    var ui = GetComponent<RectTransform>();

    var layout = text.Select(ch =>
    {
      var sprite = UPPER.Contains(ch) ? upper[UPPER.IndexOf(ch)]
        : LOWER.Contains(ch) ? lower[LOWER.IndexOf(ch)]
        : OTHER.Contains(ch) ? other[OTHER.IndexOf(ch)]
        : null;

      var offset = UPPER.Contains(ch) ? new Vector2(0, -1)
        : LOWER.Contains(ch) ? new Vector2(0, -2)
        : OTHER.Contains(ch) ? new Vector2(0, -1)
        : Vector2.zero;

      var width = (int)(sprite?.rect.width ?? 2);

      return (width, sprite, offset);
    });

    var width = layout.Sum(s => s.width + 1);
    if (width > 0) width -= 1;
    if (width % 2 == 1) width += 1;

    float origin;
    if (alignment == TextAlignment.Left)
      origin = ui ? ui.rect.xMin : 0;
    else if (alignment == TextAlignment.Center)
      origin = ui ? ui.rect.center.x - width / 2f : -width / 2f;
    else if (alignment == TextAlignment.Right)
      origin = ui ? ui.rect.xMax - width : -width;
    else
      throw new Exception("invalid alignment");

    var x = 0;
    children = layout.Select(pair =>
    {
      var obj = new GameObject("char");
      obj.transform.SetParent(transform, false);
      obj.layer = gameObject.layer;
      obj.hideFlags |= HideFlags.DontSave;

      var height = pair.sprite?.rect.height ?? 0;

      if (ui)
      {
        var rect = obj.AddComponent<RectTransform>();
        rect.offsetMax = new Vector2(pair.width, height);
        rect.offsetMin = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);

        obj.transform.localPosition = new Vector3(origin + x, pair.offset.y - 5, 0);

        var renderer = obj.AddComponent<Image>();
        renderer.sprite = pair.sprite;
        // renderer.color = color;
      }
      else
      {
        obj.transform.localPosition = new Vector3((origin + x) / 32f, pair.offset.y / 32f, 0);

        var renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = pair.sprite;
        // renderer.color = color;
      }

      x += pair.width + 1;

      return obj;
    }).ToList();
  }

  private void Clean()
  {
    if (children != null)
    {
      foreach (var child in children)
      {
        DestroyImmediate(child);
      }
      children = null;
    }
  }

  public void ReDraw()
  {
    if (this == null || !enabled) return;

    Clean();
    Draw();
  }

  void OnEnable()
  {
    Clean();
    Draw();
  }

  void OnDisable()
  {
    Clean();
  }

#if UNITY_EDITOR
  void OnValidate()
  {
    if (!enabled || UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this)) return;

    UnityEditor.EditorApplication.delayCall += () => ReDraw();
  }
#endif
}
