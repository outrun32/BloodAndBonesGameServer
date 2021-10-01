using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationContoller
{
    void Init(Animator animator, AnimatorSettings animatorSettings);
    void Update(InputModel inputModel);
    AnimationModel GetAnimationModel();
    void SendMassage<T>(AnimationMessages message, T value);
}

public enum AnimationMessages
{
    Damage,
    Death,
    EndRotation,
    HitInd,
    EndHit
}
