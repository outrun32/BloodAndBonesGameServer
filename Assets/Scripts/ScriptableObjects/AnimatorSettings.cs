using System;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimatorSettings", menuName = "ScriptableObjects/AnimatorSettings", order = 1)]
public class AnimatorSettings : ScriptableObject
{
    public AnimationParameter InputMagnitude;
    public AnimationParameter X;
    public AnimationParameter Z;
    public AnimationParameter WalkStartAngle;
    public AnimationParameter WalkStopAngle;
    public AnimationParameter IsStopRU;
    public AnimationParameter IsStopLU;
    public AnimationParameter HorAimAngle;
    public AnimationParameter IsFalling;
    public AnimationParameter IsDodge;
    public AnimationParameter DodgeAngle;
    public AnimationParameter LookAngle;
    public AnimationParameter Shield;
    public AnimationParameter IsHit;
    public AnimationParameter HitInd;
    public AnimationParameter IsJumping;
    public AnimationParameter IsDead;
    public AnimationParameter IsBlock;
    public AnimationParameter BlockFloat;
    public AnimationParameter IsAttack;
    public AnimationParameter AttackInd;
}
[Serializable]
public struct AnimationParameter
{
    public string Name;
    //public Packet.DataType DataType;
    public AnimationDataType AnimationDataType;

    public AnimationParameter(string name)
    {
        Name = name;
        //DataType = Packet.DataType.boolean;
        AnimationDataType = AnimationDataType.floatVar;
    }
}

public enum AnimationDataType
{
    integer,
    boolean,
    floatVar,
    trigger
}