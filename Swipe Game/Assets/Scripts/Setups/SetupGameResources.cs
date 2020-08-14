using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupGameResources : MonoBehaviour
{
    [SerializeField] private GameObject background;

    private void Start()
    {
        SetBackground();
    }

    private void SetBackground()
    {
        string path = "Purchase Items/Backgrounds";
        int id = PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentBackgroundID);
        Sprite[] imgs = Resources.LoadAll<Sprite>(path); ///"Purchase Items/Backgrounds"
        SpriteRenderer _sprite = background.GetComponent<SpriteRenderer>();
        if (!_sprite)
        {
            Log.WriteLog("Can not get Image.", Log.LevelsOfLogs.ERROR, "SetupGameResources");
            return;
        }
        _sprite.sprite = imgs[id];
    }
}
