using UnityEngine;
using System.Collections.Generic;

public class MovementAnimationParameterControl : MonoBehaviour
{
    private Animator animator;

    #region Monobehaviour

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.MovementEvent += SetAnimationParameters;
    }


    private void OnDisable()
    {
        EventHandler.MovementEvent -= SetAnimationParameters;
    }

    #endregion

    // TODO: Use Reflexion or Introspection here to avoid long if statements
    private void SetAnimationParameters(HubCommand hubCommand)
    {
        animator.SetFloat(Settings.xInput, hubCommand.inputX);
        animator.SetFloat(Settings.yInput, hubCommand.inputY);
        animator.SetBool(Settings.isWalking, hubCommand.isWalking);
        animator.SetBool(Settings.isRunning, hubCommand.isRunning);

        animator.SetInteger(Settings.toolEffect, (int)hubCommand.toolEffect);

        // Using Tool
        if (hubCommand.isUsingToolRight)
            animator.SetTrigger(Settings.isUsingToolRight);
        if (hubCommand.isUsingToolLeft)
            animator.SetTrigger(Settings.isUsingToolLeft);
        if (hubCommand.isUsingToolUp)
            animator.SetTrigger(Settings.isUsingToolUp);
        if (hubCommand.isUsingToolDown)
            animator.SetTrigger(Settings.isUsingToolDown);

        // Lifting Tool
        if (hubCommand.isLiftingToolRight)
            animator.SetTrigger(Settings.isLiftingToolRight);
        if (hubCommand.isLiftingToolLeft)
            animator.SetTrigger(Settings.isLiftingToolLeft);
        if (hubCommand.isLiftingToolUp)
            animator.SetTrigger(Settings.isLiftingToolUp);
        if (hubCommand.isLiftingToolDown)
            animator.SetTrigger(Settings.isLiftingToolDown);

        // Picking
        if (hubCommand.isPickingRight)
            animator.SetTrigger(Settings.isPickingRight);
        if (hubCommand.isPickingLeft)
            animator.SetTrigger(Settings.isPickingLeft);
        if (hubCommand.isPickingUp)
            animator.SetTrigger(Settings.isPickingUp);
        if (hubCommand.isPickingDown)
            animator.SetTrigger(Settings.isPickingDown);

        // Swigging Tool
        if (hubCommand.isSwingingToolRight)
            animator.SetTrigger(Settings.isSwingingToolRight);
        if (hubCommand.isSwingingToolLeft)
            animator.SetTrigger(Settings.isSwingingToolLeft);
        if (hubCommand.isSwingingToolUp)
            animator.SetTrigger(Settings.isSwingingToolUp);
        if (hubCommand.isSwingingToolDown)
            animator.SetTrigger(Settings.isSwingingToolDown);

        // Idle
        if (hubCommand.idleUp)
            animator.SetTrigger(Settings.idleUp);
        if (hubCommand.idleDown)
            animator.SetTrigger(Settings.idleDown);
        if (hubCommand.idleLeft)
            animator.SetTrigger(Settings.idleLeft);
        if (hubCommand.idleRight)
            animator.SetTrigger(Settings.idleRight);

    }

    private void AnimationEventPlayFootstepSound()
    {

    }
}
