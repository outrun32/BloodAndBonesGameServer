using UnityEngine;

public struct AnimationModel
{
    public float Speed;
    public float SpeedX;
    public float SpeedY;
    public int AttackInd;
    public int HitInd;
    public bool IsBlock;
    public bool IsBlockImpact;
    public bool IsAttack;
    public bool IsSuperAttack;
    public bool IsDeath;

    public AnimationModel(float speed, float speedX, float speedY, int attackInd, int hitInd, bool isAttack, bool isSuperAttack, bool isBlock, bool isBlockImpact, bool isDeath)
    {
        Speed = speed;
        SpeedX = speedX;
        SpeedY = speedY;
        AttackInd = attackInd;
        HitInd = hitInd;
        IsAttack = isAttack;
        IsSuperAttack = isSuperAttack;
        IsBlock = isBlock;
        IsBlockImpact = isBlockImpact;
        IsDeath = isDeath;
    }
}