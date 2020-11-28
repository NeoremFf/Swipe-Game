using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BuyItemManager : MonoBehaviour
{
    [System.Serializable]
    private class ItemConfig
    {
        public string name;
        public int cost;
        public bool hasItem;
    }

    [Header("Resources Path")]
    [SerializeField] private string resourcesPath = string.Empty;
    [SerializeField] private string jsonNameFile = string.Empty;
    private string pathForJson;

    [Header("Buttons list")]
    [SerializeField] private GameObject buyButtonPref = null;
    [SerializeField] private GameObject content = null;
    private Button currentBackgroundButton = null;

    [Header("Values")]
    [SerializeField] private Text valueOfGemsText;

    [Header("Managers")]
    [SerializeField] private MoneyManager _resourcesManager = null;

    private ItemConfig[] _itemsConfig = null;


    private void Start()
    {
        if (resourcesPath == string.Empty || jsonNameFile == string.Empty)
        {
            Log.WriteLog("Path value is empty!", Log.LevelsOfLogs.ERROR, "BuyItemManager");
            return;
        }

        // Get all backgrounds
        Sprite[] imgs = Resources.LoadAll<Sprite>(resourcesPath); ///"Purchase Items/Backgrounds"
        SetItemsConfig(imgs.Length);
        SetAllButtons(imgs);
    }

    private void SetItemsConfig(int count)
    {
        Log.WriteLog("Set backgrounds config.", Log.LevelsOfLogs.INFO, "ShopBackgrounds");
        _itemsConfig = new ItemConfig[count];
        Log.WriteLog(string.Format("Count of current backgrounds: {0}.", _itemsConfig.Length), Log.LevelsOfLogs.INFO, "ShopBackgrounds");
        pathForJson = Application.dataPath + "/Resources/" + resourcesPath + jsonNameFile;
        if (!File.Exists(pathForJson))
        {
            Log.WriteLog("File not exists.", Log.LevelsOfLogs.ERROR, "BuyItemManager");
            return;
        }
        string json = File.ReadAllText(pathForJson);
        if (json.Length == 0) Log.WriteLog("Can not set json.", Log.LevelsOfLogs.ERROR, "ShopBackgrounds");
        _itemsConfig = JsonArrayParser.FromJson<ItemConfig>(json);
    }

    private void SetAllButtons(Sprite[] imgs)
    {
        int currentBackgroundID = PlayerPrefs.GetInt(PlayerPrefsKeys.CurrentBackgroundID);
        int id = 0;
        foreach (var item in imgs)
        {
            GameObject obj = Instantiate(buyButtonPref, content.transform);
            Button btn = obj.GetComponentInChildren<Button>();
            int index = id;
            btn.onClick.AddListener(() => { ChooseBackground(index, btn); });
            if (id == currentBackgroundID)
            {
                btn.image.color = Color.green;
                currentBackgroundButton = btn;
            }
            else if (_itemsConfig[id].hasItem) btn.image.color = Color.white;
            else btn.image.color = Color.red;
            btn.GetComponentInChildren<Text>().text = string.Format("<color=black><size=70>{0}</size></color>\n<color=grey><size=50>{1}</size></color>", _itemsConfig[id].name, _itemsConfig[id].cost);
            obj.GetComponentInChildren<ShopUISetPreImg>().SetImage(item);
            id++;
        }
    }

    public void ChooseBackground(int id, Button button)
    {
        if (_itemsConfig[id].hasItem)
        {
            currentBackgroundButton.image.color = Color.white;
            button.image.color = Color.green;
            currentBackgroundButton = button;
            SetBackground(id);
            //MainMenuManager _menu = FindObjectOfType<MainMenuManager>();
            //_menu.SeccessSetAsBackgroundInfoPanel(_itemsConfig[id].name);
        }
        else
        {
            TryToBuyBackground(id, button);
        }
    }

    private void SetBackground(int id)
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.CurrentBackgroundID, id);
    }

    private void TryToBuyBackground(int id, Button button)
    {
        Debug.Log("Cost: " + _itemsConfig[id].cost);
        if (_resourcesManager.BuyItem(_itemsConfig[id].cost))
        {
            Debug.Log("Buyed!");
            button.image.color = Color.white;
            SetBackground(id);
            _itemsConfig[id].hasItem = true;
            //File.WriteAllText(pathForJson, JsonArrayParser.ToJson(_itemsConfig));
            //MainMenuManager _menu = FindObjectOfType<MainMenuManager>();
            //_menu.SuccessPurchasingInfoPanel();
            //_menu.UpdateGemsValue();
        }
        else
        {
            //MainMenuManager _menu = FindObjectOfType<MainMenuManager>();
            //if (!_menu)
            //{
            //    Log.WriteLog("Can not set MainMenuManager.", Log.LevelsOfLogs.ERROR, "BuyItemManager");
            //    return;
            //}
            //_menu.NeedMoreGemsInfoPanel();
        }
    }
}
