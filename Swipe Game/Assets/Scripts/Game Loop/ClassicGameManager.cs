using UnityEngine;
using Assets.Scripts.Game_Loop;

public class ClassicGameManager : GameLoopManager
{
    [Header("Time")]
    [Tooltip("How many time will be removed at once")]
    [SerializeField] private float timeRemoveValue = 0;
    [Tooltip("How many moves needs to remove time")]
    [SerializeField] private int timeRemoveCountMoves = 0;
    [Tooltip("Min value of time (not removes below)")]
    [SerializeField] private float timeMinRemove = 0;
    private float timeToMove = 0;

    private void Start()
    {
        Prepare();
    }

    protected override void Prepare()
    {
        Log.WriteLog("Classic mode.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
        Log.WriteLog("Game started.", Log.LevelsOfLogs.INFO, "ClassicGameManager");

        GameLoopUIUpdate uiUpdate = FindObjectOfType<GameLoopUIUpdate>();
        MenuManager menu = FindObjectOfType<MenuManager>();
        SetTimerUIUpdateEvent(uiUpdate.UpdateTimerUI);
        //SetLoseUIUpdateEvent(menu.UpdateMoneyUI);

        timeToMove = timeToGameLoop;
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        _swipeManager.transform.SetParent(ParentForSwipeController.transform);

        CreatedAllCards();
        timer = timeToMove;
        NextTurn(true);
    }

    public override void RestartGame()
    {
        Log.WriteLog("Restart Game.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
        ScoreManager.SetScoreToZero();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timeToMove = timeToGameLoop;
        timer = timeToMove;
        firstCard = true;
        UpdateTimerUI(new TimerUpdateUIEventArgs(timer, timeToGameLoop));
        NextTurn(true);
    }

    public override void ContinueLoop()
    {
        Log.WriteLog("Continue Game.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        ScoreManager.SetScoreToContinue();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timer = timeToMove;
        firstCard = true;
        UpdateTimerUI(new TimerUpdateUIEventArgs(timer, timeToGameLoop));
        NextTurn(true);
    }

    protected override void CreatedAllCards()
    {
        cards.Add(CreateList(cardPrefabs[0]));
        cards.Add(CreateList(cardPrefabs[1]));
        cards.Add(CreateList(cardPrefabs[2]));
        cards.Add(CreateList(cardPrefabs[3]));
        cards.Add(CreateList(cardPrefabs[4]));
        cards.Add(CreateList(cardPrefabs[5]));
        cards.Add(CreateList(cardPrefabs[6]));
        cards.Add(CreateList(cardPrefabs[7]));
        cards.Add(CreateList(cardPrefabs[8]));
        cards.Add(CreateList(cardPrefabs[9]));
        cards.Add(CreateList(cardPrefabs[10]));
        DeactiveAllCards();
    }

    private void Update()
    {
        Timer();
        if (timer <= 0) // time is uot
        {
            if (_cardManager.NeedSkip()) // player have to do nothing
            {
                ScoreManager.UpdateScore();
                _cardManager.MoveCard();
                NextTurn();
            }
            else // player have to do move
            {
                if (!loseState)
                {
                    Log.WriteLog("Time out.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
                    Lose();
                }
            }
        }
    }

    protected override void NextTurn(bool isFirst = false)
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
            AddToList(cards[currentList], cards[currentList][idOfCardInList]);
            idOfCardInList++;
        }

        _cardManager = cards[currentList][idOfCardInList].GetComponent<CardManager>();
        if (!_cardManager)
            Log.WriteLog("Card manager not set.", Log.LevelsOfLogs.ERROR, "ClassicGameManager");
        _swipeManager.SetCurrentCardManager(_cardManager);

        cards[currentList][idOfCardInList].transform.position = pointForCard.position;
        cards[currentList][idOfCardInList].SetActive(true);
    }
}