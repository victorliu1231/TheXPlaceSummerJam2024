using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapshot
{
    public Vector2 pos;

    public Vector2 mousePos;

	public RigidbodyState rbState;
    public Rigidbody2D rb;

    public enum SnapAction { Fire }

    public List<SnapAction> actions = new List<SnapAction>();

    public float timeSinceFirstSnapshot;

    public Snapshot(Vector2 _pos, Vector2 _mousePos, Rigidbody2D _rb, List<SnapAction> _actions, float _time)
    {
        pos = _pos;
        mousePos = _mousePos;
        rb = _rb;
        rbState = new RigidbodyState(_rb);
        foreach (SnapAction action in _actions)
        {
            actions.Add(action);
        }
        timeSinceFirstSnapshot = _time;

    }

    public Snapshot CopySelf()
    {
        return new Snapshot(pos, mousePos, rb, actions, timeSinceFirstSnapshot);
    }
}