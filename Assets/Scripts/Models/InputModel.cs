using UnityEngine;
public struct InputModel
{
    public Vector2 JoystickAxis;
    public Quaternion Rotation;
    public bool IsJumping;
    public bool IsAttacking;
    public bool IsBlocking;
    public bool IsSuperAtacking;
    public bool IsStrafing;
    public bool IsSat;

    public InputModel(Vector2 joystickAxis, Quaternion rotation, bool isJumping, bool isAttacking, bool isBlocking, bool isSuperAttacking, bool isStrafing, bool isSat)
    {
        JoystickAxis = joystickAxis;
        Rotation = rotation;
        IsJumping = isJumping;
        IsAttacking = isAttacking;
        IsBlocking = isBlocking;
        IsSuperAtacking = isSuperAttacking;
        IsStrafing = isStrafing;
        IsSat = isSat;
    }
}
