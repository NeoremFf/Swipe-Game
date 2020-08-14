using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerDownHandler
{
    private ClassicGameManager _classic = null;
    private ChallengeGameManager _challenge = null;

    /// Current State that player did
    private AllState.State currentState;
    /// Card Manager to references CheckStates()
    private CardManager _cardManager;

    private void Start()
    {
        GameModeManager _mode = FindObjectOfType<GameModeManager>();
        switch (_mode.GetGameMode())
        {
            case GameModes.Modes.NoneGame:
                break;
            case GameModes.Modes.Classic:
                _classic = FindObjectOfType<ClassicGameManager>();
                if (!_classic)
                    Log.WriteLog("ClassicGameManager not set.", Log.LevelsOfLogs.ERROR, "SwipeManager");
                break;
            case GameModes.Modes.Challenge:
                _challenge = FindObjectOfType<ChallengeGameManager>();
                if (!_challenge)
                    Log.WriteLog("ClassicGameManager not set.", Log.LevelsOfLogs.ERROR, "SwipeManager");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Set CardManager to make check correct state on card
    /// </summary>
    /// <param name="newCardManager"></param>
    public void SetCurrentCardManager(CardManager newCardManager)
    {
        _cardManager = newCardManager;
        if (!_cardManager)
            Log.WriteLog("Card Manager not set.", Log.LevelsOfLogs.ERROR, "SwipeManager");
    }

    /// <summary>
    /// Get Current State to check that it is correct
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnCurrentState()
    {
        yield return new WaitForSeconds(0.1f);

        /// return move of palyer to check
        if (_cardManager)
        {
            if (_classic)
            {
                _cardManager.CheckCorrectState(currentState, _classic, null);
            }
            else
            {
                _cardManager.CheckCorrectState(currentState, null, _challenge);
            }
        }
    }

    /// <summary>
    /// Check Swipe
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        /// swipe was on Horizontal
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
        {
            ///swipe was on Right
            if (eventData.delta.x > 0)
            {
                currentState = AllState.State.Right;
            }
            /// swipe was on Left
            else
            {
                currentState = AllState.State.Left;
            }
        }
        /// swipe was on Vertical
        else
        {
            ///swipe was on Top
            if (eventData.delta.y > 0)
            {
                currentState = AllState.State.Top;
            }
            /// swipe was on Down
            else
            {
                currentState = AllState.State.Down;
            }
        }

       // ClickVFX.EndVFX();
    }

    /// <summary>
    /// Check Touch
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        ClickVFX.SetVFXStatus(true);
        currentState = AllState.State.Touch;

        StartCoroutine(ReturnCurrentState());
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
