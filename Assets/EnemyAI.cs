using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float killDistance = 2f;
    public float punchAnimationDuration = 1.2f;
    public float patrolRadius = 10f;
    public float waitTimeAtPatrolPoint = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isPlayerDead = false;
    private bool hasStartedPunch = false;
    private bool isPatrolling = true;
    private Vector3 patrolTarget;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        SetNewPatrolPoint();
    }

    void Update()
    {
        if (isPlayerDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Deteksi player
        if (distanceToPlayer <= killDistance && !hasStartedPunch)
        {
            StartCoroutine(PunchAndKill());
            return;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            isPatrolling = false;
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            return;
        }

        // Jika tidak mendeteksi player, kembali patroli
        if (!isPatrolling)
        {
            isPatrolling = true;
            SetNewPatrolPoint();
        }

        PatrolLogic();
    }

    void PatrolLogic()
    {
        if (isWaiting) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitThenPatrol());
        }

        animator.SetBool("isRunning", true);
    }

    IEnumerator WaitThenPatrol()
    {
        isWaiting = true;
        animator.SetBool("isRunning", false);
        yield return new WaitForSeconds(waitTimeAtPatrolPoint);
        SetNewPatrolPoint();
        isWaiting = false;
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
        }
    }

    IEnumerator PunchAndKill()
    {
        isPlayerDead = true;
        hasStartedPunch = true;

        agent.ResetPath();
        animator.SetBool("isRunning", false);
        animator.SetTrigger("punch");

        Debug.Log("Enemy memulai animasi punch...");

        yield return new WaitForSeconds(punchAnimationDuration);

        Debug.Log("Player terbunuh.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlayerDead && !hasStartedPunch && other.CompareTag("Player"))
        {
            StartCoroutine(PunchAndKill());
        }
    }
}
