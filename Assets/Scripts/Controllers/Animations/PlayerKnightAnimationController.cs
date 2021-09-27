using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnightAnimationController : IAnimationContoller
{
    private Transform _transform;
    private AnimatorSettings _animatorSettings;
    private Animator _animator;
    private float _speedUp = 1.2f;
    private float _speedDown = 2;
    private float _speedShield = 1.5f;
    private float _shield = 0;
    private float _shieldUp = 0;
    private bool isRotate;
    private Vector3 _rotation;
    private Vector2 _axis = Vector2.zero;
    private Vector2 _inputAxis = Vector2.zero;
    private bool _isAim = false;
    private Dictionary<string, TriggerVar> _triggers = new Dictionary<string, TriggerVar>();

    public PlayerKnightAnimationController(Transform transform)
    {
        _transform = transform;
    }
    public void Init(Animator animator,AnimatorSettings animatorSettings)
    {
        _animatorSettings = animatorSettings;
        _animator = animator;
        Debug.Log($"Init{animator} {animatorSettings}");
    }
    public void SetDirectionMove(Vector2 axis)
    {

        _inputAxis = axis;
        if (!_isAim) axis = axis * 1.7f;
        _axis = Vector2.Lerp(_axis, axis, (axis.magnitude < _axis.magnitude? _speedDown: _speedUp) * Time.deltaTime);
        if (_isAim) _shieldUp = 1;
        else
        {_shieldUp = 0;
            if (_axis.magnitude > 1) _shieldUp = -1;
            else _shieldUp = 0;
        }

        _shield = Mathf.Lerp(_shield, _shieldUp, _speedShield * Time.deltaTime); 
            
        SetValue(_animatorSettings.Shield, _shield);
            
        SetValue(_animatorSettings.InputMagnitude, axis.magnitude);
        SetValue(_animatorSettings.X, _axis.x);
        SetValue(_animatorSettings.Z, _axis.y);
            
        if (_axis.magnitude < 0.8f) SetValue(_animatorSettings.IsStopRU, axis.magnitude < _axis.magnitude);
        else SetValue(_animatorSettings.IsStopLU, axis.magnitude < _axis.magnitude);
            
        SetValue(_animatorSettings.WalkStartAngle,  Mathf.Atan2 (_axis.x, _axis.y) * Mathf.Rad2Deg);
        if (_axis.magnitude > 0.2f)
        {
            SetValue(_animatorSettings.WalkStopAngle, Mathf.Atan2(_axis.x, _axis.y) * Mathf.Rad2Deg);

        }
    }
    private void SetValue(AnimationParameter parameter, object value)
    {
        SetValue(parameter.AnimationDataType, parameter.Name, value);
    }
    private void SetValue(AnimationDataType animationDataType,string name,object value)
    {
        switch (animationDataType)
        {
            case AnimationDataType.integer:
                _animator.SetInteger(name,(int)value);
                break;
            case AnimationDataType.boolean:
                _animator.SetBool(name,(bool)value);
                break;
            case AnimationDataType.floatVar:
                _animator.SetFloat(name,(float)value);
                break;
            case AnimationDataType.trigger:
                if (!_triggers.ContainsKey(name)) _triggers.Add(name,new TriggerVar());
                if ((bool) value)
                {
                    _triggers[name].Value = true;
                    _animator.SetTrigger(name);
                }
                break;
        }
    }
    private object GetValue(AnimationParameter parameter)
    {
        return GetValue(parameter.AnimationDataType, parameter.Name);
    }
    private object GetValue(AnimationDataType animationDataType,string name)
    {
        switch (animationDataType)
        {
            case AnimationDataType.integer:
                return _animator.GetInteger(name);
            case AnimationDataType.boolean:
                return _animator.GetBool(name);
            case AnimationDataType.floatVar:
                return _animator.GetFloat(name);
            case AnimationDataType.trigger:
                if (!_triggers.ContainsKey(name))
                {
                    _triggers.Add(name, new TriggerVar());
                    _triggers[name].Value = false;
                }
                return _triggers[name].Value;
            default: return null;
        }
    }
    public void SetIsAim(bool value)
    {
        //TODO: ВСЕ ГОВНО. ВСЕ ПЕРЕДЕЛАТЬ
        _isAim = value;
    }
    public void Dodging(Vector2 axis)
    {
        if (axis.magnitude > 0.1f)
        {
            Debug.Log("Dodging!");
            SetValue(_animatorSettings.DodgeAngle, Mathf.Atan2(axis.x, axis.y) * Mathf.Rad2Deg);
            SetValue(_animatorSettings.IsDodge, true);
        }
        
    }
    public void Update(InputModel inputModel)
    {
        
        SetValue(_animatorSettings.IsAttack, inputModel.IsAttacking);
        Debug.Log($"inputModel.IsStrafing = {inputModel.IsStrafing}");
        if (inputModel.IsStrafing) Dodging(inputModel.JoystickAxis);
        else SetValue(_animatorSettings.IsDodge, false);
        SetIsAim(inputModel.IsAim);
        if (inputModel.JoystickAxis.magnitude > 0.1f)
        {
            if (Mathf.Abs(inputModel.CameraRotate) > 60)
            {
                if(!isRotate)SetValue(_animatorSettings.HorAimAngle, inputModel.CameraRotate);

                isRotate = true;
            }
            else
            {
                if (!isRotate)_transform.Rotate(0,inputModel.CameraRotate * Time.deltaTime * 10,0);
            }
        }
        
        SetDirectionMove((!isRotate)?inputModel.JoystickAxis: Vector2.zero);
        
    }

    public AnimationModel GetAnimationModel()
    { return new AnimationModel(
        (float)GetValue(_animatorSettings.InputMagnitude),
        (float)GetValue(_animatorSettings.X),
        (float)GetValue(_animatorSettings.Z),
        (float)GetValue(_animatorSettings.WalkStartAngle),
        (float)GetValue(_animatorSettings.WalkStopAngle),
        (bool)GetValue(_animatorSettings.IsStopRU),
        (bool)GetValue(_animatorSettings.IsStopLU),
        (int)GetValue(_animatorSettings.AttackInd),
        (float)GetValue(_animatorSettings.HitInd),
        (bool)GetValue(_animatorSettings.IsAttack),
        false,
        (bool)GetValue(_animatorSettings.IsBlock),
        false,
        (bool)GetValue(_animatorSettings.IsDead),
        (bool)GetValue(_animatorSettings.IsHit),
        (bool)GetValue(_animatorSettings.IsJumping),
        (bool)GetValue(_animatorSettings.IsFalling),
        (bool)GetValue(_animatorSettings.IsDodge),
        (float)GetValue(_animatorSettings.HorAimAngle),
        (float)GetValue(_animatorSettings.Shield),
        (float)GetValue(_animatorSettings.LookAngle),
        (float)GetValue(_animatorSettings.BlockFloat)
            );
    }

    public void SendMassage<T>(AnimationMessages message, T value)
    {
        switch (message)
        {
            case AnimationMessages.Damage:
                Damage(Convert.ToInt32(value));
                break;
            case AnimationMessages.Death:
                Death();
                break;
            case AnimationMessages.EndRotation:
                SetValue(_animatorSettings.HorAimAngle, 0f);
                isRotate = false;
                break;
        }
    }
    public void Damage(int value)
    {
        SetValue(_animatorSettings.HitInd, value);
    }
    public void Death()
    {
        SetValue(_animatorSettings.IsDead, true);
    }
}


public class TriggerVar
{
    private bool _value;

    public bool Value
    {
        get
        {
            bool res = _value;
            _value = false;
            return res;
        }
        set
        {
            _value = value;
        }
    }
}
