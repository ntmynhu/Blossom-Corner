using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlWalking : MonoBehaviour
{
    [SerializeField] private MoneyButton moneyButton;
    [SerializeField] private Animator animator;

    private Transform counterPos;
    private Transform exitPos;
    private bool isExiting = false;
    private bool isWalking = false;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        counterPos = GameObject.Find("Counter").transform;
        agent.SetDestination(counterPos.position);
        
        isWalking = true;

        moneyButton.OnCollectMoney += MoneyButton_OnCollectMoney;
    }

    private void Update()
    {
        ReachedCounterCheck();

        if (isExiting && HasReachedDestination())
        {
            FlowerShopManager.Instance.DecreaseCustomerNumber();
            Destroy(gameObject);
        }

        //animator.SetBool("isWalking", agent.velocity.magnitude > 0);
        animator.SetBool("isWalking", isWalking);
    }

    private void MoneyButton_OnCollectMoney(object sender, System.EventArgs e)
    {
        isExiting = true;
        animator.SetBool("isExiting", isExiting);
        exitPos = GameObject.Find("ExitPos").transform;
        agent.SetDestination(exitPos.position);
    }

    private bool HasReachedDestination()
    {
        // Check if the agent is close enough to the destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ReachedCounterCheck()
    {
        // Check if the agent is close enough to the destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    isWalking = false;
                }
            }
        }
    }

    public bool IsMoving()
    {
        return isWalking || isExiting;
    }
}
