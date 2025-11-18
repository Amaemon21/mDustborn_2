using UnityEngine;

public interface IControllable
{
    public void Move(Vector2 direction);
    public void Look(Vector2 direction);
    public void Jump();
}