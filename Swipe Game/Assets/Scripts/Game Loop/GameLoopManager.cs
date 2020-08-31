using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game_Loop
{
    public abstract class GameLoopManager : MonoBehaviour
    {
        #region Variables
        [Header("Scenes Elements")]
        [SerializeField] protected GameObject playScene = null;
        [SerializeField] protected GameObject loseScene = null;

        [Header("Parents")]
        [SerializeField] protected GameObject ParentForSwipeController = null;
        [Tooltip("Parent for all cards")]
        [SerializeField] protected GameObject cardsPool = null;

        [Header("Cards")]
        [SerializeField] protected GameObject[] cardPrefabs = null;
        [SerializeField] protected Transform pointForCard = null;

        [Header("Time")]
        [Tooltip("Start time to loop (move)")]
        [SerializeField] protected float timeToGameLoop = 0;
        protected float timer = 0;

        [Header("Managers")]
        [SerializeField] protected SwipeManager _swipeManager = null;
        [SerializeField] private MenuManager menuManager = null;
        protected CardManager _cardManager = null;

        // POOL
        protected List<List<GameObject>> cards = new List<List<GameObject>>();
        protected int currentList = 0;
        protected int idOfCardInList = -1;

        // flags
        protected bool firstCard = true;
        protected bool loseState = false;

        // Events for UI update
        protected delegate void UIUpdateLoseHandler();
        protected delegate void UIUpdateTimerHandler(object sender, TimerUpdateUIEventArgs e);
        protected event UIUpdateLoseHandler updateUIAfterLoseEvent = null;
        protected event UIUpdateTimerHandler updateTimerUIEvent = null;
        #endregion

        /// <summary>
        /// Restart game
        /// </summary>
        public abstract void RestartGame();

        /// <summary>
        /// Continue game after lose
        /// </summary>
        public abstract void ContinueLoop();

        /// <summary>
        /// Implements start-settings
        /// </summary>
        protected abstract void Prepare();

        /// <summary>
        /// Creates all cards pool and makes general pool
        /// </summary>
        protected abstract void CreatedAllCards();

        /// <summary>
        /// Implements logic of turn
        /// </summary>
        /// <param name="isFirst">if its first turn - true</param>
        protected abstract void NextTurn(bool isFirst = false);

        /// <summary>
        /// Set event that will update timer UI in game
        /// </summary>
        /// <param name="handler">event</param>
        protected void SetTimerUIUpdateEvent(UIUpdateTimerHandler handler)
        {
            updateTimerUIEvent += handler;
        }

        /// <summary>
        /// Set event that will update UI after game
        /// </summary>
        /// <param name="handler">event</param>
        protected void SetLoseUIUpdateEvent(UIUpdateLoseHandler handler)
        {
            updateUIAfterLoseEvent += handler;
        }

        /// <summary>
        /// Check player move that it is correct or not
        /// </summary>
        /// <param name="move"></param>
        public void CheckPlayerMove(bool stateCheck)
        {
            if (stateCheck)
            {
                firstCard = false;
                ScoreManager.UpdateScore();
                NextTurn();
            }
            else
            {
                Lose();
            }
        }

        protected void UpdateTimerUI(TimerUpdateUIEventArgs args)
        {
            updateTimerUIEvent?.Invoke(this, args);
        }

        /// <summary>
        /// Creates cards list
        /// </summary>
        /// <param name="pref"></param>
        protected List<GameObject> CreateList(GameObject pref)
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(Instantiate(pref));
            return list;
        }

        /// <summary>
        /// Creates card and adds it to list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pref"></param>
        protected void AddToList(List<GameObject> list, GameObject pref)
        {
            list.Add(Instantiate(pref));
        }

        /// <summary>
        /// Implements logic of timer
        /// </summary>
        protected void Timer()
        {
            if (firstCard) return;
            UpdateTimerUI(new TimerUpdateUIEventArgs(timer, timeToGameLoop));
            timer -= Time.deltaTime;
        }

        /// <summary>
        /// Calls after lose game
        /// </summary>
        protected void Lose()
        {
            Log.WriteLog("Game ended.", Log.LevelsOfLogs.INFO, "ClassicGameManager");
            loseState = true;
            _swipeManager.gameObject.SetActive(false);
            StopAllCoroutines();
            DeactiveAllCards();
            // UI
            updateUIAfterLoseEvent?.Invoke();

            playScene.SetActive(false);
            loseScene.SetActive(true);
        }

        /// <summary>
        ///  Deactive all cards in pool
        /// </summary>
        protected void DeactiveAllCards()
        {
            foreach (var items in cards)
                foreach (var item in items)
                    item.SetActive(false);
        }

        /// <summary>
        /// Turn off card after players move
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        protected IEnumerator TurnOffCard(GameObject card)
        {
            yield return new WaitForSeconds(0.5f);
            card.SetActive(false);
        }
    }
}