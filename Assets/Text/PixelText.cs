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

  public int MaxWidth;

  private const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  private const string LOWER = "abcdefghijklmnopqrstuvwxyz";
  private const string OTHER = "0123456789.,!?'&()/-:";

  private Dictionary<char, Sprite> sprites;
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
    [':'] = 2,
  };

  private void MakeSprites(string alphabet, int y_offset, int height)
  {
    var offset = 0;
    for (var i = 0; i < alphabet.Length; ++i)
    {
      var width = GetWidth(alphabet[i]);
      sprites[alphabet[i]] = Sprite.Create(font, new Rect(offset, y_offset, width, height), new Vector2(0, 0), 32, 0, SpriteMeshType.Tight, Vector4.zero, false);
      offset += width;
    }
  }

  private static IEnumerable<string> GetWords(string src)
  {
    var index = 0;
    var word_start = 0;

    while (index < src.Length)
    {
      if (src[index] == ' ')
      {
        if (word_start < index)
        {
          yield return src.Substring(word_start, index - word_start);
        }

        word_start = ++index;
      }
      else
      {
        ++index;
      }
    }

    if (word_start < index)
    {
      yield return src.Substring(word_start, index - word_start);
    }
  }

  private static int GetWidth(char ch)
  {
    return kerning.ContainsKey(ch) ? kerning[ch]
      : UPPER.Contains(ch) ? 8
      : LOWER.Contains(ch) ? 6
      : OTHER.Contains(ch) ? 6
      : 0;
  }

  private static Vector2 GetOffset(char ch)
  {
    return UPPER.Contains(ch) ? new Vector2(0, -1)
        : LOWER.Contains(ch) ? new Vector2(0, -2)
        : OTHER.Contains(ch) ? new Vector2(0, -1)
        : Vector2.zero;
  }

  private List<GameObject> children;
  private void Draw()
  {
    if (sprites == null)
    {
      sprites = new Dictionary<char, Sprite>();
      MakeSprites(UPPER, 24, 11);
      MakeSprites(LOWER, 12, 12);
      MakeSprites(OTHER, 0, 12);
    }

    var ui = GetComponent<RectTransform>();

    var layout = new List<(int width, List<(int width, string str)> words)>();

    var line_width = 0;
    var current_line = new List<(int width, string str)>();

    foreach (var word in GetWords(text))
    {
      var word_width = word.Sum(w => GetWidth(w) + 1) - 1;

      if (MaxWidth != 0 && line_width + word_width > MaxWidth)
      {
        line_width -= 4;
        if (line_width % 2 == 1) line_width += 1;

        layout.Add((line_width, current_line));
        current_line = new List<(int width, string word)>();
        line_width = 0;
      }

      current_line.Add((word_width, word));
      line_width += word_width + 4;
    }

    if (line_width > 0)
    {
      line_width -= 4;
      if (line_width % 2 == 1) line_width += 1;

      layout.Add((line_width, current_line));
    }

    if (layout.Count == 0)
    {
      return;
    }

    var max_width = layout.Max(l => l.width);

    float current_y = 7 * (layout.Count - 1);
    if (ui) current_y -= 5;

    children = new List<GameObject>();
    foreach (var line in layout)
    {
      float current_x;
      if (alignment == TextAlignment.Left)
        current_x = ui ? ui.rect.xMin : 0;
      else if (alignment == TextAlignment.Center)
        current_x = ui ? ui.rect.center.x - line.width / 2f : -line.width / 2f;
      else if (alignment == TextAlignment.Right)
        current_x = ui ? ui.rect.xMax - line.width : -line.width;
      else
        throw new Exception("invalid alignment");

      foreach (var word in line.words)
      {
        foreach (var ch in word.str)
        {
          var obj = new GameObject("char");
          obj.transform.SetParent(transform, false);
          obj.layer = gameObject.layer;
          obj.hideFlags |= HideFlags.DontSave;

          var sprite = sprites.GetValueOrDefault(ch);
          var offset = GetOffset(ch);

          var width = GetWidth(ch);
          var position = new Vector3(current_x + offset.x, current_y + offset.y, 0); ;

          if (sprite)
          {
            var height = sprite.rect.height;

            if (ui)
            {
              obj.transform.localPosition = position;

              var rect = obj.AddComponent<RectTransform>();
              rect.offsetMax = new Vector2(word.width, height);
              rect.offsetMin = new Vector2(0, 0);
              rect.pivot = new Vector2(0, 0);

              var renderer = obj.AddComponent<Image>();
              renderer.sprite = sprite;
            }
            else
            {
              obj.transform.localPosition = position / 32;

              var renderer = obj.AddComponent<SpriteRenderer>();
              renderer.sprite = sprite;
            }
          }

          current_x += width + 1;

          children.Add(obj);
        }

        current_x += 4;
      }

      current_y -= 14;
    }
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
