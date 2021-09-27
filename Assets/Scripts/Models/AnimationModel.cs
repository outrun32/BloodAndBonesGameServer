using System;
using System.Collections.Generic;
using UnityEngine;
/*
 * < клиенту от сервера, > серверу от клиета
 * 
- InputMagnitude - магнитуда от джойстика. 
- X,Z - скорости по осям
- WalkStartAngle - угол поворота джойстика. Нужно для стартовой анимации
- WalkStopAngle - аналогично
- IsStopRU - анимации остановки при ходьбе
- ISStopLU - анимации остановке при беге
> HorAimAngle - поворачивает персонажа на определенный угол (лучше не меньше 45 градусов Можно до 180)
< IsFallnig - сваливание в полете. После отпускания срабатываем анимация приземления
< IsDodge - анимации уклонения (стрейф)
- Doodge Angle - угол для стрейфа
< LookAngle - угол, относительно персонажа, куда будет смотреть игрок (можно сделать для преследования игрока
< Shield - на сколько подняты оружия (и меч и щит)ю -1 опущены полностью (расслаблен игрок). 0 - среднее положение. 1 максимально подняты (максимально сосредоточение игрока)
< IsHit - получение дамага. Тригер
< HitInd
< IsJumpint - убогая анимация прыжга. Нужно ее или переделывать или доработать.
< IsDead - по идее должна быть смерть но пока не настроено
< IsBlock - отбитие атаки мечем.
< BlockFloat - 0 - легкое отбитие, 1, 2 - отбитие тяжелой атаки. 3 - отбитие тяжелой атаки щитом.
< IsAttack - тригер для атаки
< AttackINd - 0 - стандартная 8 комбо атака, 1 - второстеменая 6 комбо атака, 2 - удар ногой. 3 короткая сильная комбо атака, 4 короткая сильная второстепенная комбоатака

 */
public struct AnimationModel
{
    public float InputMagnitude;
    public float X;
    public float Z;
    public float WalkStartAngle;
    public float WalkStopAngle;
    public bool  IsStopRU;
    public bool  ISStopLU;
    public int   AttackInd;
    public float   HitInd;
    public bool  IsBlock;
    public bool  IsHit;
    public bool  IsJumping;
    public bool  IsBlockImpact;
    public bool  IsAttack;
    public bool  IsSuperAttack;
    public bool  IsDeath;
    public bool  IsFalling;
    public bool  IsDodge;
    public float HorAimAngle;
    public float Shield;
    public float LookAngle;
    public float BlockFloat;

    public AnimationModel(float inputMagnitude,float x, float z, float walkStartAngle, 
        float walkStopAngle, bool isStopRU, bool isStopLU,
        int attackInd, float hitInd, bool isAttack, bool isSuperAttack, bool isBlock, 
        bool isBlockImpact, bool isDeath, bool isHit,bool isJumping, bool isFalling, 
        bool isDodge, float horAimAngle, float shield, float lookAngle, float blockfloat)
    {
        AttackInd = attackInd;
        HitInd = hitInd;
        IsAttack = isAttack;
        IsSuperAttack = isSuperAttack;
        IsBlock = isBlock;
        IsBlockImpact = isBlockImpact;
        IsDeath = isDeath;
        IsHit = isHit;
        IsJumping = isJumping;
        IsFalling = isFalling;
        IsDodge = isDodge;
        HorAimAngle = horAimAngle;
        Shield = shield;
        LookAngle = lookAngle;
        BlockFloat = blockfloat;
        InputMagnitude = inputMagnitude;
        X = x;
        Z = z;
        WalkStartAngle = walkStartAngle;
        WalkStopAngle = walkStopAngle;
        IsStopRU = isStopRU;
        ISStopLU = isStopLU;
    }
}