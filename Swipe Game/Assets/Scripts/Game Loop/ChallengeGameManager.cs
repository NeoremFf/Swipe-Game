using UnityEngine;
using Assets.Scripts.Game_Loop;

public class ChallengeGameManager : GameLoopManager
{
    private void Start()
    {
        Prepare();
    }

    protected override void Prepare()
    {
        Log.WriteLog("Challenge mode.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        Log.WriteLog("Game started.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");

        GameLoopUIUpdate uiUpdate = FindObjectOfType<GameLoopUIUpdate>();
        SetTimerUIUpdateEvent(uiUpdate.UpdateTimerUI);

        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        _swipeManager.transform.SetParent(ParentForSwipeController.transform);

        CreatedAllCards();
        timer = timeToGameLoop;
        NextTurn(true);
    }

    public override void RestartGame()
    {
        Log.WriteLog("Restart Game.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        base.RestartGame();
        ScoreManager.SetScoreToZero();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timer = timeToGameLoop;
        UpdateTimerUI(new TimerUpdateUIEventArgs(timer, timeToGameLoop));
        NextTurn(true);
    }

    public override void ContinueLoop()
    {
        Log.WriteLog("Continue Game.", Log.LevelsOfLogs.INFO, "ChallengeGameManager");
        ScoreManager.SetScoreToContinue();
        loseState = false;
        _swipeManager.gameObject.SetActive(true);
        timer += 5;
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
        DeactiveAllCards();
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

    protected override void NextTurn(bool isFirst = false)
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
            AddToList(cards[currentList], cards[currentList][idOfCardInList]);
            idOfCardInList++;
        }

        CardManager _cardManager = cards[currentList][idOfCardInList].GetComponent<CardManager>();
        if (!_cardManager)
            Log.WriteLog("Card manager not set.", Log.LevelsOfLogs.ERROR, "ChallengeGameManager");
        _swipeManager.SetCurrentCardManager(_cardManager);

        cards[currentList][idOfCardInList].transform.position = pointForCard.position;
        cards[currentList][idOfCardInList].SetActive(true);
    }
}
