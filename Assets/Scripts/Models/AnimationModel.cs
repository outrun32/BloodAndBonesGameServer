using UnityEngine;

public struct AnimationModel
{
    public float Speed;
    public float SpeedX;
    public float SpeedY;
    public int AttackInd;
    public int BlockInd;
    public bool IsAttack;
    public bool IsSupedAttack;

    public AnimationModel(float speed, float speedX, float speedY, int attackInd, int blockInd, bool isAttack, bool isSupedAttack)
    {
        Speed = speed;
        SpeedX = speedX;
        SpeedY = speedY;
        AttackInd = attackInd;
        BlockInd = blockInd;
        IsAttack = isAttack;
        IsSupedAttack = isSupedAttack;
    }
}
