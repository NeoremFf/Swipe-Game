using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClassicGameManager : MonoBehaviour
{
    [Header("Scanes Elements")]
    [SerializeField] private GameObject playScane;
    [SerializeField] private GameObject loseScane;

    [Header("Parents")]
    [SerializeField] private GameObject ParentForSwipeController;
    [Tooltip("Parent for all cards")]
    [SerializeField] private GameObject cardsPool;

    [Header("Cards")]
    [Tooltip("Only this order:\nTop\nDown\nLeft\nRight\nTouch\nSkip\nNOT in the same order")]
    [SerializeField] private GameObject[] cardPrefabs;
    [SerializeField] private Transform pointForCard;

    [Header("UI")]
    [SerializeField] private Text timerUI;
    [SerializeField] private Image scaleUI;

    [Header("Time")]
    [Tooltip("Start time to do move")]
    [SerializeField] private float startTimeToMove;
    [Tooltip("How many time will be remove at one time")]
    [SerializeField] private float timeRemoveValue;
    [Tooltip("How many moves need to remove time")]
    [SerializeField] private int timeRemoveCountMoves;
    [Tooltip("Min value of time (not more removes)")]
    [SerializeField] private float timeMinRemove;
    private float timeToMove;
    private float timer;

    [Header("Managers")]
    [SerializeField] private SwipeManager _swipeManager;
    private CardManager _cardManager;

    // POOL
    private List<List<GameObject>> cards = new List<List<GameObject>>();
    private List<GameObject> cardsTop = new List<GameObject>();
    private List<GameObject> cardsDown = new List<GameObject>();
    private List<GameObject> cardsLeft = new List<GameObject>();
    private List<GameObject> cardsRight = new List<GameObject>();
    private List<GameObject> cardsTouch = new List<GameObject>();
    private List<GameObject> cardsSkip = new List<GameObject>();
    private List<GameObject> cardsNotTop = new List<GameObject>();
    private List<GameObject> cardsNotDown = new List<GameObject>();
    private List<GameObject> cardsNotLeft = new List<GameObject>();
    private List<GameObject> cardsNotRight = new List<GameObject>();
    private List<GameObject> cardsNotSkip = new List<GameObject>();
    private int currentList;
    private int idOfCardInList = -1;

    private bool firstCard = true;
    private bool loseState = false;

    private void Start()
    {
        Log.WriteLog("Classic mode.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
        Log.WriteLog("Game started.", Log.LevelsOfLogs.INFO, "ClassicGameManager");

        timeToMove = startTimeToMove;
        loseState = false;
        _swipeManager.gameObject.SetActive(true);

        CreatedAllCards();
        timer = timeToMove;
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
            firstCard = false;
            NextTurn();
        }
        else
        {
            Lose();
        }
    }

    public void RestartLoop()
    {
        Log.WriteLog("Restart Game.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
        ScoreManager.SetScoreToZero();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timeToMove = startTimeToMove;
        timer = timeToMove;
        firstCard = true;
        NextTurn(true);
    }

    public void ContinueLoop()
    {
        Log.WriteLog("Continue Game.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        ScoreManager.SetScoreToContinue();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timer = timeToMove;
        firstCard = true;
        NextTurn(true);
    }

    /// <summary>
    /// Create all cards and move it in GO pool
    /// </summary>
    /// <param name="move"></param>
    private void CreatedAllCards()
    {
        AddItemToList(cardsTop, cardPrefabs[0]);
        AddItemToList(cardsDown, cardPrefabs[1]);
        AddItemToList(cardsLeft, cardPrefabs[2]);
        AddItemToList(cardsRight, cardPrefabs[3]);
        AddItemToList(cardsTouch, cardPrefabs[4]);
        AddItemToList(cardsSkip, cardPrefabs[5]);
        AddItemToList(cardsNotTop, cardPrefabs[6]);
        AddItemToList(cardsNotDown, cardPrefabs[7]);
        AddItemToList(cardsNotLeft, cardPrefabs[8]);
        AddItemToList(cardsNotRight, cardPrefabs[9]);
        AddItemToList(cardsNotSkip, cardPrefabs[10]);
        cards.Add(cardsTop);
        cards.Add(cardsDown);
        cards.Add(cardsLeft);
        cards.Add(cardsRight);
        cards.Add(cardsTouch);
        cards.Add(cardsSkip);
        cards.Add(cardsNotTop);
        cards.Add(cardsNotDown);
        cards.Add(cardsNotLeft);
        cards.Add(cardsNotRight);
        cards.Add(cardsNotSkip);
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
        Timer();
        if (timer <= 0) /// time is uot
        {
            if (_cardManager.NeedSkip()) /// player have to do nothing
            {
                ScoreManager.UpdateScore();
                _cardManager.MoveCard();
                NextTurn();
            }
            else /// player have to do move
            {
                if (!loseState)
                {
                    Log.WriteLog("Time out.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
                    Lose();
                }
            }
        }
    }


    private void NextTurn(bool isFirst = false)
    {
        if (!isFirst)
            StartCoroutine(TurnOffCard(cards[currentList][idOfCardInList]));
        if (ScoreManager.GetCurrentScore() % timeRemoveCountMoves == 0 && timeToMove > timeMinRemove)
            timeToMove -= timeRemoveValue;
        timer = timeToMove;
        _cardManager = null;

        currentList = UnityEngine.Random.Range(0, cards.Count);
        if (isFirst && currentList == 5) /// 5 its number of Skip Card
            do
            {
                currentList = UnityEngine.Random.Range(0, cards.Count);
            } while (currentList == 5);
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

        _cardManager = cards[currentList][idOfCardInList].GetComponent<CardManager>();
        if (!_cardManager)
            Log.WriteLog("Card manager not set.", Log.LevelsOfLogs.ERROR, "ClassicGameManager");
        _swipeManager.SetCurrentCardManager(_cardManager);

        cards[currentList][idOfCardInList].transform.position = pointForCard.position;
        cards[currentList][idOfCardInList].SetActive(true);
    }

    private void Lose()
    {
        Log.WriteLog("Game ended.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
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
            Log.WriteLog("Can not set MenuManger.", Log.LevelsOfLogs.ERROR, "ClassicGameManager");
            return;
        }
        _menu.UpdateMoneyUI();
    }

    private void Timer()
    {
        timerUI.text = timer.ToString("0.0");
        scaleUI.transform.localScale = new Vector3(timer / timeToMove, 1, 1);
        if (firstCard) return;
        timer -= Time.deltaTime;
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
