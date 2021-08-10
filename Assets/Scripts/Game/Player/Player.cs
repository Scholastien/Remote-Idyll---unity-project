using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

public class Player : SingletonMonobehaviour<Player>
{
    private AnimationOverrides _animationOverrides;

    // Movement Parameters
    public HubCommand hubCommand;
    private Camera _mainCamera;
    private Rigidbody2D _rigidBody2D;
    private Direction _playerDirection;

    private List<CharacterAttribute> _characterAttributeCustomizationList;
    [InfoBox("Should be populated in the prefab with the equipped item sprite renderer")]
    [ChildGameObjectsOnly(IncludeSelf = false)]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

    private CharacterAttribute _armsCharacterAttribute;
    private CharacterAttribute _toolCharacterAttribute;

    private float _movementSpeed;
    // TODO: Consider a State Machine instead of a boolean value that will be set by everything everywhere
    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }


    private void OnEnable()
    {
        EventHandler.PlayerCarryItem += ShowCarriedItem;
        EventHandler.PlayerClearCarriedItem += ClearCarriedItem;
    }

    private void OnDisable()
    {
        EventHandler.PlayerCarryItem -= ShowCarriedItem;
        EventHandler.PlayerClearCarriedItem -= ClearCarriedItem;
    }

    protected override void Awake()
    {
        base.Awake();

        hubCommand = new HubCommand();

        _rigidBody2D = GetComponent<Rigidbody2D>();

        _animationOverrides = GetComponentInChildren<AnimationOverrides>();

        // Initialize swapable character attributes
        _armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.arms, PartVariantColour.none, PartVariantType.none);

        // Initialize character attribute list
        _characterAttributeCustomizationList = new List<CharacterAttribute>();

        // Get Reference to the main camera
        // TODO: change this to not Find() the camera
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        #region Player Input

        if (!PlayerInputIsDisabled)
        {
            ResetAnimationTriggers();

            PlayerMovementInput();

            PlayerWalkInput();

            EventHandler.CallMovementEvent(hubCommand);
        }

        #endregion
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float newPosX = hubCommand.inputX * _movementSpeed * Time.deltaTime;
        float newPosY = hubCommand.inputY * _movementSpeed * Time.deltaTime;

        Vector2 move = new Vector2(newPosX, newPosY);
        _rigidBody2D.MovePosition(_rigidBody2D.position + move);
    }


    private void ResetAnimationTriggers()
    {
        hubCommand.toolEffect = ToolEffect.none;
        hubCommand.isUsingToolRight = false;
        hubCommand.isUsingToolLeft = false;
        hubCommand.isUsingToolUp = false;
        hubCommand.isUsingToolDown = false;
        hubCommand.isLiftingToolRight = false;
        hubCommand.isLiftingToolLeft = false;
        hubCommand.isLiftingToolUp = false;
        hubCommand.isLiftingToolDown = false;
        hubCommand.isPickingRight = false;
        hubCommand.isPickingLeft = false;
        hubCommand.isPickingUp = false;
        hubCommand.isPickingDown = false;
        hubCommand.isSwingingToolRight = false;
        hubCommand.isSwingingToolLeft = false;
        hubCommand.isSwingingToolUp = false;
        hubCommand.isSwingingToolDown = false;
    }

    // TODO: Use the new Input System from Package Manager
    private void PlayerMovementInput()
    {
        hubCommand.inputY = Input.GetAxisRaw("Vertical");
        hubCommand.inputX = Input.GetAxisRaw("Horizontal");

        if (hubCommand.inputY != 0 && hubCommand.inputX != 0)
        {
            hubCommand.inputX *= 0.71f;
            hubCommand.inputY *= 0.71f;
        }

        if (hubCommand.inputX != 0 || hubCommand.inputY != 0)
        {
            hubCommand.isRunning = true;
            hubCommand.isWalking = false;
            hubCommand.isIdle = false;
            _movementSpeed = Settings.runningSpeed;

            // Capture PlayerDirection for save game
            if (hubCommand.inputX < 0)
                _playerDirection = Direction.left;
            else if (hubCommand.inputX > 0)
                _playerDirection = Direction.right;
            else if (hubCommand.inputY < 0)
                _playerDirection = Direction.down;
            else
                _playerDirection = Direction.up;
        }
        else if (hubCommand.inputY == 0 && hubCommand.inputX == 0)
        {
            hubCommand.isRunning = false;
            hubCommand.isWalking = false;
            hubCommand.isIdle = true;
        }

    }

    private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            hubCommand.isRunning = false;
            hubCommand.isWalking = true;
            hubCommand.isIdle = false;
            _movementSpeed = Settings.walkingSpeed;
        }
        else
        {
            hubCommand.isRunning = true;
            hubCommand.isWalking = false;
            hubCommand.isIdle = false;
            _movementSpeed = Settings.runningSpeed;
        }
    }
    private void ResetMovement()
    {
        hubCommand.inputX = 0f;
        hubCommand.inputY = 0f;
        hubCommand.isRunning = false;
        hubCommand.isWalking = false;
        hubCommand.isIdle = true;
    }

    public void DisablePlayerInputAndResetMovement()
    {
        DisablePlayerInput();
        ResetMovement();

        EventHandler.CallMovementEvent(hubCommand);
    }


    public void EnablePlayerInput()
    {
        PlayerInputIsDisabled = false;
    }
    public void DisablePlayerInput()
    {
        PlayerInputIsDisabled = true;
    }

    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        // apply base character arms customization
        _armsCharacterAttribute.partVariantType = PartVariantType.none;
        _characterAttributeCustomizationList.Clear();
        _characterAttributeCustomizationList.Add(_armsCharacterAttribute);
        _animationOverrides.ApplyCharacterCustomizationParameters(_characterAttributeCustomizationList);

        hubCommand.isCarrying = false;
    }

    public void ShowCarriedItem(string itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            // apply 'carry' character arms customization
            _armsCharacterAttribute.partVariantType = PartVariantType.carry;
            _characterAttributeCustomizationList.Clear();
            _characterAttributeCustomizationList.Add(_armsCharacterAttribute);
            _animationOverrides.ApplyCharacterCustomizationParameters(_characterAttributeCustomizationList);

            hubCommand.isCarrying = true;
        }
    }

    public Vector3 GetPlayerViewportPosition()
    {
        return _mainCamera.WorldToViewportPoint(transform.position);
    }


}
