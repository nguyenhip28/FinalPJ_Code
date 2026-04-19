using UnityEngine;

public class NPCOrder
{
    public bool meat;
    public bool lettuce;
    public bool tomato;

    public NPCOrder()
    {
        meat = Random.value > 0.5f;
        lettuce = Random.value > 0.5f;
        tomato = Random.value > 0.5f;
    }
}