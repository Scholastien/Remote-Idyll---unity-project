using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
    public float inputX, inputY;
    public bool isWalking, isRunning, isIdle, isCarrying;
    public ToolEffect toolEffect;
    public bool isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown;
    public bool isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown;
    public bool isPickingRight, isPickingLeft, isPickingUp, isPickingDown;
    public bool isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown;
    public bool idleUp, idleDown, idleLeft, idleRight;

    private void Update()
    {
        HubCommand command = new HubCommand(inputX, inputY,
      isWalking, isRunning, isIdle, isCarrying,
      toolEffect,
      isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
      isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
      isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
      isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
      idleUp, idleDown, idleLeft, idleRight);

      EventHandler.CallMovementEvent(command);
    }
}
