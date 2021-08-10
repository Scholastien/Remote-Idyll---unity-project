using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;


//TODO: Refractor this shit
// Best approach would be to wrap all the parameters into an "input" object where we need to create a new class
[System.Serializable]
[IncludeMyAttributes]
[HideLabel]
[InlineProperty]
public class HubCommand : Attribute
{
    public float inputX, inputY;
    public bool isWalking, isRunning;
    public bool isIdle, isCarrying;
    public ToolEffect toolEffect;
    public bool isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown;
    public bool isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown;
    public bool isPickingRight, isPickingLeft, isPickingUp, isPickingDown;
    public bool isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown;
    public bool idleUp, idleDown, idleLeft, idleRight;

    public HubCommand(float inputX = 0f, float inputY = 0f, bool isWalking = false, bool isRunning = false, bool isIdle = false, bool isCarrying = false,
    ToolEffect toolEffect = ToolEffect.none,
    bool isUsingToolRight = false, bool isUsingToolLeft = false, bool isUsingToolUp = false, bool isUsingToolDown = false,
    bool isLiftingToolRight = false, bool isLiftingToolLeft = false, bool isLiftingToolUp = false, bool isLiftingToolDown = false,
    bool isPickingRight = false, bool isPickingLeft = false, bool isPickingUp = false, bool isPickingDown = false,
    bool isSwingingToolRight = false, bool isSwingingToolLeft = false, bool isSwingingToolUp = false, bool isSwingingToolDown = false,
    bool idleUp = false, bool idleDown = false, bool idleLeft = false, bool idleRight = false)
    {
        this.inputX = inputX;
        this.inputY = inputY;
        this.isWalking = isWalking;
        this.isRunning = isRunning;
        this.isIdle = isIdle;
        this.isCarrying = isCarrying;
        this.toolEffect = toolEffect;
        this.isUsingToolRight = isUsingToolRight;
        this.isUsingToolLeft = isUsingToolLeft;
        this.isUsingToolUp = isUsingToolUp;
        this.isUsingToolDown = isUsingToolDown;
        this.isLiftingToolRight = isLiftingToolRight;
        this.isLiftingToolLeft = isLiftingToolLeft;
        this.isLiftingToolUp = isLiftingToolUp;
        this.isLiftingToolDown = isLiftingToolDown;
        this.isPickingRight = isPickingRight;
        this.isPickingLeft = isPickingLeft;
        this.isPickingUp = isPickingUp;
        this.isPickingDown = isPickingDown;
        this.isSwingingToolRight = isSwingingToolRight;
        this.isSwingingToolLeft = isSwingingToolLeft;
        this.isSwingingToolUp = isSwingingToolUp;
        this.isSwingingToolDown = isSwingingToolDown;
        this.idleUp = idleUp;
        this.idleDown = idleDown;
        this.idleLeft = idleLeft;
        this.idleRight = idleRight;
    }
}
public delegate void MovementDelegate(HubCommand hubCommand);

public static class EventHandler
{
    #region Inventory Event
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdateEvent;
    public static void CallInventoryUpdateEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        InventoryUpdateEvent?.Invoke(inventoryLocation, inventoryList);
    }
    #endregion

    #region Movement Event
    public static event MovementDelegate MovementEvent;
    // Movement Event Call for publishers
    public static void CallMovementEvent(HubCommand hubCommand)
    {
        MovementEvent?.Invoke(hubCommand);
    }
    #endregion

    #region  Carry Item Event
    public static event Action<string> PlayerCarryItem;
    public static event Action PlayerClearCarriedItem;
    public static void CallPlayerToCarryItem(ItemDetails itemDetails)
    {
        if (itemDetails.canBeCarried)
            PlayerCarryItem?.Invoke(itemDetails.itemCode);
        else
            PlayerClearCarriedItem?.Invoke();
    }
    public static void CallPlayerToClearCarriedItem()
    {
        PlayerClearCarriedItem?.Invoke();
    }

    #endregion

    #region Time Event

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;
    public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameMinuteEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;
    public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameHourEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;
    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameDayEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;
    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameSeasonEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;
    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameYearEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    #endregion

    #region Scene Change Event


    //before the scene unload fade out event
    public static event Action BeforeSceneUnloadFadeOutEvent;
    public static void CallBeforeSceneUnloadFadeOutEvent()
    {
        BeforeSceneUnloadFadeOutEvent?.Invoke();
    }

    //before scene unload event
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    //after scene load event
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneUnloadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }

    //after the scene unload fade out event
    public static event Action AfterSceneLoadFadeInEvent;
    public static void CallAfterSceneUnloadFadeInEvent()
    {
        AfterSceneLoadFadeInEvent?.Invoke();
    }

    #endregion

}