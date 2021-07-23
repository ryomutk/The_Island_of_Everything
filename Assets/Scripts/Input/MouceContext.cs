using UnityEngine;

public class MouceContext
{
    public InputState state{get;set;}
    public Vector2 position{get;set;}
    public KeyCode key{get;set;}

}

public enum InputState
{
    none,
    start,
    pressed,
    cancel
}