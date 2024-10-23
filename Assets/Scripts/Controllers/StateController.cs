using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CombatSystem;

public class StateController : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    public Animator Animator => _animator;

    protected BaseState currentState;

    protected virtual void Start()
    {
        ChangeState(StateName.Idle);
    }

    public void ChangeState(StateName stateName)
    {
        BaseState newState = null;

        switch (stateName)
        {
            case StateName.Idle:
                newState = new IdleState(this);
                break;
            case StateName.Attack:
                newState = new AttackState(this);
                break;
            case StateName.Defense:
                newState = new DefenseState(this);
                break;
            case StateName.Charge:
                newState = new ChargeState(this);
                break;
            //case StateName.Win:
            //    newState = new WinState(this);
            //    break;
            //case StateName.Lose:
            //    newState = new LoseState(this);
            //    break;
            default:
                break;
        }

        if (currentState != null)
            currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void ExecuteAction(StateController otherController)
    {
        if (currentState != null) currentState.ExecuteAction(otherController);
    }

    public void BackToIdle()
    {
        ChangeState(StateName.Idle);
    }

    public BaseState GetCurrentState()
    {
        return currentState;
    }

    public void CallOnAnimationEnd(string animationName, System.Action onAnimationComplete)
    {
        StartCoroutine(WaitForAnimationToEnd(animationName, onAnimationComplete));
    }

    private IEnumerator WaitForAnimationToEnd(string animationName, System.Action onAnimationComplete)
    {
        // Wait for the animation to start
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        // Wait for the animation to finish
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // Call the action once the animation is complete
        onAnimationComplete?.Invoke();
    }
}
