using UnityEngine;
using Assets.Scripts.Game_Loop;

public class CardManager : MonoBehaviour
{
    [SerializeField] private bool isNot = false;
    [SerializeField] private AllState.State[] stateNeed = null;
    private int stateLength = 0;
    private int currentStateNeed = 0;
    private bool isMove = false;
    private Vector3 vecToMoveTo = Vector3.zero;

    /// <summary>
    /// Prepare
    /// </summary>
    private void Start()
    {
        stateLength = stateNeed.Length;
        SetVectorToMoveAftrerSwipe();
    }

    /// <summary>
    /// Set direction to move card after players move
    /// </summary>
    private void SetVectorToMoveAftrerSwipe()
    {
        vecToMoveTo = new Vector3(6.11f, 2.45f, 9.3f);
    }

    /// <summary>
    /// Do not able to do move
    /// </summary>
    private void OnEnable()
    {
        isMove = false;
    }

    /// <returns>if need skip turn - true, else - false</returns>
    public bool NeedSkip()
    {
        return (stateNeed[0] == AllState.State.Skip && !isNot) ? true : false;
    }

    /// <summary>
    /// Main core of card manager
    /// Get card state and check that playrs move  was correct
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="gameManager"></param>
    public void CheckCorrectState(AllState.State newState, GameLoopManager gameManager)
    {
        bool stateOfCheck = false;
        if (newState == stateNeed[currentStateNeed])
        {
            if (isNot) // wrong
            {
                stateOfCheck = false;
                SendResult(stateOfCheck, gameManager);
            }
            else // correct!
            {
                stateOfCheck = true;
                currentStateNeed++;
                if (currentStateNeed >= stateLength)
                    SendResult(stateOfCheck, gameManager);
            }
        }
        else
        {
            if (isNot && newState != AllState.State.Touch) // correct
            {
                stateOfCheck = true;
                currentStateNeed++;
                if (currentStateNeed >= stateLength)
                    SendResult(stateOfCheck, gameManager);
            }
            else // wrong
            {
                stateOfCheck = false;
                SendResult(stateOfCheck, gameManager);
            }
        }

        MoveCard();
    }

    /// <summary>
    /// Card able to move
    /// </summary>
    public void MoveCard()
    {
        isMove = true;
    }

    /// <summary>
    /// Send result to Game manager
    /// </summary>
    /// <param name="result"></param>
    /// <param name="gameManager"></param>
    private void SendResult(bool result, GameLoopManager gameManager)
    {
        currentStateNeed = 0;
        gameManager?.CheckPlayerMove(result);
    }

    private void Update()
    {
        if (isMove)
            transform.position = Vector3.MoveTowards(transform.position, vecToMoveTo, Time.deltaTime * 20);
    }
}
