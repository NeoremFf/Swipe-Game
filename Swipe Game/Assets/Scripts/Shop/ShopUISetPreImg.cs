using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUISetPreImg : MonoBehaviour
{
    public void SetImage(Sprite newSprite)
    {
        Image img = GetComponent<Image>();
        if (!img)
        {
            Log.WriteLog("Can not set Image.", Log.LevelsOfLogs.ERROR, "ShopUISetPreImg");
            return;
        }
        img.sprite = newSprite;
    }
}
