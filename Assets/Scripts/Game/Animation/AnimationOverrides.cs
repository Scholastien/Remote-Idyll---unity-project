using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AnimationOverrides : MonoBehaviour
{
    [SerializeField] private GameObject character = null;
    [SerializeField] private SO_AnimationType[] soAnimationTypeArray = null;

    [ShowInInspector] private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation;
    [ShowInInspector] private Dictionary<string, SO_AnimationType> animationTypeDictionaryByCompositeAttributeKey;

    private void Start()
    {
        animationTypeDictionaryByAnimation = InitializeAnimationTypeDictionaryKeyedByAnimationClip(soAnimationTypeArray);
        animationTypeDictionaryByCompositeAttributeKey = InitializeAnimationTypeDictionaryKeyedByString(soAnimationTypeArray);
    }
    // Initialize animation type dictionary keyed by animation clip
    public Dictionary<AnimationClip, SO_AnimationType> InitializeAnimationTypeDictionaryKeyedByAnimationClip(SO_AnimationType[] sO_AnimationTypesArray)
    {
        Dictionary<AnimationClip, SO_AnimationType> result = new Dictionary<AnimationClip, SO_AnimationType>();

        foreach (SO_AnimationType item in sO_AnimationTypesArray)
        {
            result.Add(item.animationClip, item);
        }

        return result;
    }
    // Initialize animation type dictionary keyed by string
    public Dictionary<string, SO_AnimationType> InitializeAnimationTypeDictionaryKeyedByString(SO_AnimationType[] sO_AnimationTypesArray)
    {
        Dictionary<string, SO_AnimationType> result = new Dictionary<string, SO_AnimationType>();

        foreach (SO_AnimationType item in sO_AnimationTypesArray)
        {
            string key = item.characterPart.ToString() + item.partVariantColour.ToString() + item.partVariantType.ToString() + item.animationName.ToString();
            result.Add(key, item);
        }

        return result;
    }

    public void ApplyCharacterCustomizationParameters(List<CharacterAttribute> characterAttributesList)
    {
        // loop through all character attributes and set the animation override controller for each
        foreach (CharacterAttribute characterAttribute in characterAttributesList)
        {
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> animationKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            string animatorSOAssetName = characterAttribute.characterPart.ToString();

            // Find animators in scene that match scriptable object animator type
            Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();

            currentAnimator = Array.Find(animatorsArray, element => element.name == animatorSOAssetName);

            // Get base current animations for animator
            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
            List<AnimationClip> animationList = new List<AnimationClip>(aoc.animationClips);

            foreach (AnimationClip animationClip in animationList)
            {
                //find animation in dictionary
                bool foundAnimation = animationTypeDictionaryByAnimation.TryGetValue(animationClip, out SO_AnimationType sO_AnimationType);

                if (foundAnimation)
                {
                    string key = characterAttribute.characterPart.ToString() + characterAttribute.partVariantColour.ToString() + characterAttribute.partVariantType.ToString() + sO_AnimationType.animationName.ToString();

                    bool foundSwapAnimation = animationTypeDictionaryByCompositeAttributeKey.TryGetValue(key, out SO_AnimationType swapSO_AnimationType);

                    // Swap the current AnimationClip with the one in the override one
                    // => instead of playing the current animation, the dictionary is pointing the override clip as a Value with the current animation as a Key
                    if (foundSwapAnimation)
                    {
                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip;

                        animationKeyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, swapAnimationClip));
                    }
                }
            }

            // Apply animation updates to animation override controller and then update animator with the new controller
            aoc.ApplyOverrides(animationKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }
    }
}