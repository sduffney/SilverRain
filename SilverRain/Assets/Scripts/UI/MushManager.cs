using System.Collections;
using UnityEngine;

public class MushManager : MonoBehaviour
{
    public GameObject miniMushPrefab;
    [SerializeField] int miniMushCount = 5;
    [SerializeField] float spawnRadius = 2f;
    [SerializeField] float jumpsForce = 5f;
 

    [Header("Explosion Settings")]
    [SerializeField] float miniMushScale = 0.3f;
    [SerializeField] float explodeDelay = 1f;
    bool hasTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hasTriggered) return;
            hasTriggered = true;
            StartCoroutine(ExplodeAfterDelay());
        }

    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explodeDelay);
        Destroy(gameObject);
        SpawnMiniMushrooms(transform);
        
    }
    // Spawns mini mushrooms at random positions around the spawn point

    private void SpawnMiniMushrooms(Transform spawnPoint)
    {
        for (int i = 0; i < miniMushCount; i++)
        {
            Vector3 randomPos = spawnPoint.position + Random.insideUnitSphere * spawnRadius;

            GameObject miniMush = Instantiate(miniMushPrefab, randomPos, Quaternion.identity);

            
            miniMush.transform.localScale = miniMushPrefab.transform.localScale * miniMushScale;

            Rigidbody rb = miniMush.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 jumpDirection = (miniMush.transform.position - spawnPoint.position).normalized + Vector3.up;
                rb.AddForce(jumpDirection * jumpsForce, ForceMode.Impulse);
            }
        }
    }

}
