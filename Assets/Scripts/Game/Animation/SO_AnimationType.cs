using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SO_AnimationType", menuName = "Scriptable Object/Animation/Animation Type")]
public class SO_AnimationType : ScriptableObject
{
    public AnimationClip animationClip;
    public AnimationName animationName;
    public CharacterPartAnimator characterPart;
    public PartVariantColour partVariantColour;
    public PartVariantType partVariantType;
}