using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setup : MonoBehaviour
{
    [SerializeField] private LocalizationManager localization;
    private Setup instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Log.Setup();
        Log.WriteLog("Game entering.", Log.LevelsOfLogs.INFO, "Setup");
        Log.WriteLog("Start setup.", Log.LevelsOfLogs.INFO, "Setup");
        localization.LoadLocalizedText();
        StartupManager.Startup();
        StartCoroutine(LoadMainMenu());
        Log.WriteLog("Setup ends.", Log.LevelsOfLogs.INFO, "Setup");
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(1f);
        while (!localization.IsInitialize)
        {
            yield return null;
        }

        Log.WriteLog("Load main menu.", Log.LevelsOfLogs.INFO, "Setup");
        SceneManager.LoadScene(1);
    }
}
