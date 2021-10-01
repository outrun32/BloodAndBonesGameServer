using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHitController : MonoBehaviour
{
    private IAnimationContoller _animationContoller;
    public OnTriggeredEnterController Center;
    public OnTriggeredEnterController Left;
    public OnTriggeredEnterController Right;
    [SerializeField]private List<Collider> _hitsColliders;
    public string Tag;
    public void Init(IAnimationContoller animationContoller)
    {
        _animationContoller = animationContoller;
    }
    private void OnEnable()
    {
        Center.OnTriggerEnterEvent += OnTriggeredEnterInvoke;
        Left.OnTriggerEnterEvent += OnTriggeredEnterInvoke;
        Right.OnTriggerEnterEvent += OnTriggeredEnterInvoke;
    }

    private void OnDisable()
    {
        Center.OnTriggerEnterEvent -= OnTriggeredEnterInvoke;
        Left.OnTriggerEnterEvent -= OnTriggeredEnterInvoke;
        Right.OnTriggerEnterEvent -= OnTriggeredEnterInvoke;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggeredEnterInvoke(string name, Collider collider)
    {
        Debug.Log($"CheckTag {collider.tag} Name = {collider.name}");
        if(collider.CompareTag(Tag) && !_hitsColliders.Contains(collider))
        {
            Debug.Log("Set Hit Ind");
            switch (name)
            {
                case "Center":
                    _animationContoller.SendMassage(AnimationMessages.HitInd, 0);
                    break;
                case "Left":
                    _animationContoller.SendMassage(AnimationMessages.HitInd, -1);
                    break;
                case "Right":
                    _animationContoller.SendMassage(AnimationMessages.HitInd, 1);
                    break;
            }
        }
    }
}
