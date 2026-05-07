using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Points")]
    public Transform orderPoint;
    public Transform seatPoint;

    [Header("Settings")]
    public float orderDistance = 1.5f;
    public float sitDistance = 1f;

    private bool isOrdering = false;
    private bool isSitting = false;

    void Start()
    {
        agent.SetDestination(orderPoint.position);
    }

    void Update()
    {
        UpdateAnimation();

        CheckOrderPoint();
        CheckSeatPoint();
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void CheckOrderPoint()
    {
        if (!isOrdering && Vector3.Distance(transform.position, orderPoint.position) < orderDistance)
        {
            StartCoroutine(OrderProcess());
        }
    }

    IEnumerator OrderProcess()
    {
        isOrdering = true;

        agent.isStopped = true;

        Debug.Log("NPC ordering...");

        
        yield return new WaitForSeconds(3f);


        agent.isStopped = false;
        agent.SetDestination(seatPoint.position);
    }

    void CheckSeatPoint()
    {
        if (isOrdering && !isSitting && Vector3.Distance(transform.position, seatPoint.position) < sitDistance)
        {
            SitDown();
        }
    }

    void SitDown()
    {
        isSitting = true;

        agent.isStopped = true;

        transform.position = seatPoint.position;
        transform.rotation = seatPoint.rotation;

        Debug.Log("NPC sitting");

        animator.SetTrigger("Sit");
    }
}