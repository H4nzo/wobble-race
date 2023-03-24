using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageUtils
{
    public static Sprite CreateSprite(Texture2D texture, Rect rect, Vector2 pivot)
    {
        try
        {
            return Sprite.Create(texture, rect, pivot);
        }
        catch
        {
            return Sprite.Create(null, Rect.zero, Vector2.zero);
        }
    }
}