using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovemennt
{
    void SetCanMove(bool value);
    void Start();
    void FixedUpdate();
    void SetInput(InputModel inputModel);
}