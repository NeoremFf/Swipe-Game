using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager : MonoBehaviour
{
    [SerializeField] private bool isNot = false;
    [SerializeField] private AllState.State[] stateNeed = null;
    private int stateLength = 0;
    private int currentStateNeed = 0;
    private bool isMove = false;
    private Vector3 vecToMoveTo = Vector3.zero;

    private void Start()
    {
        stateLength = stateNeed.Length;
        SetVectorToMoveAftrerSwipe();
    }

    private void SetVectorToMoveAftrerSwipe()
    {
        vecToMoveTo = new Vector3(6.11f, 2.45f, 9.3f);
    }

    private void OnEnable()
    {
        isMove = false;
    }

    public bool NeedSkip()
    {
        return (stateNeed[0] == AllState.State.Skip && !isNot) ? true : false;
    }

    public void CheckCorrectState(AllState.State newState, ClassicGameManager _classic, ChallengeGameManager _challenge)
    {
        bool stateOfCheck = false;
        if (newState == stateNeed[currentStateNeed])
        {
            if (isNot) // wrong
            {
                stateOfCheck = false;
                SendResult(stateOfCheck, _classic, _challenge);
            }
            else // correct!
            {
                stateOfCheck = true;
                currentStateNeed++;
                if (currentStateNeed >= stateLength)
                    SendResult(stateOfCheck, _classic, _challenge);
            }
        }
        else
        {
            if (isNot && newState != AllState.State.Touch) // correct
            {
                stateOfCheck = true;
                currentStateNeed++;
                if (currentStateNeed >= stateLength)
                    SendResult(stateOfCheck, _classic, _challenge);
            }
            else // wrong
            {
                stateOfCheck = false;
                SendResult(stateOfCheck, _classic, _challenge);
            }
        }

        MoveCard();
    }

    public void MoveCard()
    {
        isMove = true;
    }

    private void SendResult(bool result, ClassicGameManager _classic, ChallengeGameManager _challenge)
    {
        currentStateNeed = 0;
        _classic?.CheckPlayerMove(result);
        _challenge?.CheckPlayerMove(result);
    }

    private void Update()
    {
        if (isMove)
            transform.position = Vector3.MoveTowards(transform.position, vecToMoveTo, Time.deltaTime * 20);
    }
}
