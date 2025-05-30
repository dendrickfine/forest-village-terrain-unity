using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // Untuk restart scene

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float killDistance = 2f;

    private NavMeshAgent agent;
    private bool isPlayerDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (isPlayerDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);
        }

        if (distance <= killDistance)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        isPlayerDead = true;
        Debug.Log("Player caught!");

        // Bisa diganti dengan animasi kematian atau game over UI
        // Untuk contoh, kita restart scene
        Invoke("RestartScene", 2f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Tambahan: jika ingin trigger dengan collider
    private void OnTriggerEnter(Collider other)
    {
        if (!isPlayerDead && other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }
}
