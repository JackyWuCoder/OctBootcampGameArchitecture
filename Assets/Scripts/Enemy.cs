using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private Transform targetPointsParent;

    [Range(0f, 0.5f)]
    [SerializeField] private float accuracy = 0.1f;


    [Range(0f, 5f)]
    [SerializeField] private float waitTime = 1f;

    private int currentTarget = 0;
    private NavMeshAgent agent;

    public void GetTargetPositions()
    {
        targetPoints = new Transform[targetPointsParent.childCount];
        for (int i = 0; i < targetPointsParent.childCount; i++)
        {
            targetPoints[i] = targetPointsParent.GetChild(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetTargetPositions();
        agent.destination = targetPoints[currentTarget].position;
    }

    // Update is called once per frame
    void Update()
    {
        // Checking distance between enemy and target position
        if (agent.remainingDistance < accuracy)
        {
            currentTarget++;
            // Ensuring the current target does not exceed the number of target points
            if (currentTarget >= targetPoints.Length)
            {
                currentTarget = 0;
            }
            StartCoroutine(PatrolAtPosition(waitTime));
        }
    }

    private IEnumerator PatrolAtPosition(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // Set new enemy position
        agent.destination = targetPoints[currentTarget].position;
    }
}
