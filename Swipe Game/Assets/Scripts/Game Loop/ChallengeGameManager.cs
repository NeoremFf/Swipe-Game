using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChallengeGameManager : MonoBehaviour
{
    [Header("Scanes Elements")]
    [SerializeField] private GameObject playScane;
    [SerializeField] private GameObject loseScane;

    [Header("Parents")]
    [SerializeField] private GameObject ParentForSwipeController;
    [Tooltip("Parent for all cards")]
    [SerializeField] private GameObject cardsPool;

    [Header("Cards")]
    [Tooltip("Only this order:\nTop\nDown\nLeft\nRight\nTouch")]
    [SerializeField] private GameObject[] cardPrefabs;
    [SerializeField] private Transform pointForCard;
    [SerializeField] private GameObject swipeController;

    [Header("UI")]
    [SerializeField] private Text timerUI;
    [SerializeField] private Image scaleUI;

    [Header("Time")]
    [Tooltip("Time to loop")]
    [SerializeField] private float timeToGameLoop;
    private float timer;

    [Header("Managers")]
    [SerializeField] private SwipeManager _swipeManager;

    // POOL
    private List<List<GameObject>> cards = new List<List<GameObject>>();
    private List<GameObject> cardsTop = new List<GameObject>();
    private List<GameObject> cardsDown = new List<GameObject>();
    private List<GameObject> cardsLeft = new List<GameObject>();
    private List<GameObject> cardsRight = new List<GameObject>();
    private List<GameObject> cardsTouch = new List<GameObject>();
    private List<GameObject> cardsNotTop = new List<GameObject>();
    private List<GameObject> cardsNotDown = new List<GameObject>();
    private List<GameObject> cardsNotLeft = new List<GameObject>();
    private List<GameObject> cardsNotRight = new List<GameObject>();
    private int currentList;
    private int idOfCardInList = -1;

    private bool firstCard = true;
    private bool loseState = false;

    private void Start()
    {
        Log.WriteLog("Challenge mode.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        Log.WriteLog("Game started.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");

        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        swipeController.transform.SetParent(ParentForSwipeController.transform);

        CreatedAllCards();
        timer = timeToGameLoop;
        NextTurn(true);
    }

    /// <summary>
    /// Check player move that it is correct or not
    /// </summary>
    /// <param name="move"></param>
    public void CheckPlayerMove(bool stateCheck)
    {
        if (stateCheck)
        {
            ScoreManager.UpdateScore();
            NextTurn();
        }
        else
        {
            Lose();
        }
    }

    public void RestartLoop()
    {
        Log.WriteLog("Restart Game.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        ScoreManager.SetScoreToZero();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timer = timeToGameLoop;
        NextTurn(true);
    }

    public void ContinueLoop()
    {
        Log.WriteLog("Continue Game.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        ScoreManager.SetScoreToContinue();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timer += 5;
        NextTurn(true);
    }

    /// <summary>
    /// Create all cards and move it in pool
    /// </summary>
    /// <param name="move"></param>
    private void CreatedAllCards()
    {
        AddItemToList(cardsTop, cardPrefabs[0]);
        AddItemToList(cardsDown, cardPrefabs[1]);
        AddItemToList(cardsLeft, cardPrefabs[2]);
        AddItemToList(cardsRight, cardPrefabs[3]);
        AddItemToList(cardsTouch, cardPrefabs[4]);
        AddItemToList(cardsNotTop, cardPrefabs[5]);
        AddItemToList(cardsNotDown, cardPrefabs[6]);
        AddItemToList(cardsNotLeft, cardPrefabs[7]);
        AddItemToList(cardsNotRight, cardPrefabs[8]);
        cards.Add(cardsTop);
        cards.Add(cardsDown);
        cards.Add(cardsLeft);
        cards.Add(cardsRight);
        cards.Add(cardsTouch);
        cards.Add(cardsNotTop);
        cards.Add(cardsNotDown);
        cards.Add(cardsNotLeft);
        cards.Add(cardsNotRight);
        DeactiveAllCards();
    }

    private void AddItemToList(List<GameObject> list, GameObject pref)
    {

        GameObject newCard = Instantiate(pref);
        list.Add(newCard);
        foreach (var item in list)
        {
            item.transform.SetParent(cardsPool.transform);
            item.transform.position = pointForCard.position;
        }
    }

    private void Update()
    {
        if (!loseState)
            Timer();
        if (timer <= 0) /// time is uot
        {
            if (!loseState)
            {
                Log.WriteLog("Time out.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
                Lose();
            }
        }
    }

    private void NextTurn(bool isFirst = false)
    {
        if (!isFirst)
        {
            StartCoroutine(TurnOffCard(cards[currentList][idOfCardInList]));
            timer += 0.2f;
            if (timer > timeToGameLoop)
                timer = timeToGameLoop;
        }

        currentList = UnityEngine.Random.Range(0, cards.Count);
        idOfCardInList = -1;
        bool hasAccessCard = false; // flag has card for use it
        foreach (var item in cards[currentList])
        {
            if (item.activeSelf == false)
            {
                hasAccessCard = true; // has -> set it
                idOfCardInList++;
                break;
            }
            idOfCardInList++;
        }
        if (!hasAccessCard) // not has -> create it
        {
            AddItemToList(cards[currentList], cards[currentList][idOfCardInList]);
            idOfCardInList++;
        }

        CardManager _cardManager = cards[currentList][idOfCardInList].GetComponent<CardManager>();
        if (!_cardManager)
            Log.WriteLog("Card manager not set.", Log.LevelsOfLogs.ERROR, "ChallengeGameManager");
        _swipeManager.SetCurrentCardManager(_cardManager);

        cards[currentList][idOfCardInList].transform.position = pointForCard.position;
        cards[currentList][idOfCardInList].SetActive(true);
    }

    private void Lose()
    {
        Log.WriteLog("Game ended.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        loseState = true;
        _swipeManager.gameObject.SetActive(false);
        StopAllCoroutines();
        DeactiveAllCards();
        ScoreManager.UpdateLoseUI();
        playScane.SetActive(false);
        loseScane.SetActive(true);
        MenuManager _menu = FindObjectOfType<MenuManager>();
        if (!_menu)
        {
            Log.WriteLog("Can not set MenuManger.", Log.LevelsOfLogs.ERROR, "ChallengeGameManager");
            return;
        }
        _menu.UpdateMoneyUI();
    }

    private void Timer()
    {
        timer -= Time.deltaTime;
        timerUI.text = timer.ToString("0.0");
        scaleUI.transform.localScale = new Vector3(timer / timeToGameLoop, 1, 1);
    }

    private IEnumerator TurnOffCard(GameObject card)
    {
        yield return new WaitForSeconds(0.5f);
        card.SetActive(false);
    }

    private void DeactiveAllCards()
    {
        foreach (var items in cards)
            foreach (var item in items)
                item.SetActive(false);
    }
}
