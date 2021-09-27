using UnityEngine;

public class MovementByAnimationController : IMovemennt
{
    private Transform _transform;
    // Start is called before the first frame update
    public void SetCanMove(bool value)
    {
        //throw new System.NotImplementedException();
    }

    public void Start()
    {
       
    }

    public MovementByAnimationController(Transform transform)
    {
        _transform = transform;
    }
    public void FixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public void SetInput(InputModel inputModel)
    {
        //_transform.rotation = inputModel.Rotation;
        //throw new System.NotImplementedException();
    }

}
