using UnityEngine;

public class PlayerInteractionModel
{
    public GameObject heldObject;
    public Knife heldKnife;

    public Transform currentHoldPoint;

    public bool IsHoldingObject() => heldObject != null;
    public bool HasKnife() => heldKnife != null;
    public bool IsHoldingAnything() => heldObject != null || heldKnife != null;

    public GameObject GetHeldGameObject()
    {
        if (heldObject != null) return heldObject;
        if (heldKnife != null) return heldKnife.gameObject;
        return null;
    }

    public void Clear()
    {
        heldObject = null;
        heldKnife = null;
        currentHoldPoint = null;
    }
}