using UnityEngine;
public struct InputModel
{
    public Vector2 JoystickAxis;
    public float CameraRotate;
    public bool IsJumping;
    public bool IsAttacking;
    public bool IsBlocking;
    public bool IsSuperAtacking;
    public bool IsStrafing;
    public bool IsSat;
    public bool IsAim;

    public InputModel(Vector2 joystickAxis, float rotation, bool isJumping, bool isAttacking, bool isBlocking, bool isSuperAttacking, bool isStrafing, bool isSat, bool isAim)
    {
        JoystickAxis = joystickAxis;
        CameraRotate = rotation;
        IsJumping = isJumping;
        IsAttacking = isAttacking;
        IsBlocking = isBlocking;
        IsSuperAtacking = isSuperAttacking;
        IsStrafing = isStrafing;
        IsSat = isSat;
        IsAim = isAim;
    }
}
