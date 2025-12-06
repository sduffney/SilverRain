using System.Collections;
using System.Collections.Generic;
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

    [Header("Jump Settings")]
    [SerializeField] float jumpDelay = 3f;
    [SerializeField] float arcUpFactor = 1.0f;
    [SerializeField] float jumpDuration = 1f;
    [SerializeField] Transform juompTo;

    [Header("VFX Settings")]
    [SerializeField] ParticleSystem explodeVFX;

    bool hasTriggered = false;
    Renderer[] mushRenderers;

    private void Awake()
    {
        mushRenderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (hasTriggered) return;
        hasTriggered = true;

        StartCoroutine(ExplodeAfterDelay(juompTo));
    }

    private IEnumerator ExplodeAfterDelay(Transform enemyTarget)
    {
        yield return new WaitForSeconds(explodeDelay);

        HideMushVisuals();
        PlayExplosionVfx();

        SpawnMiniMushrooms(transform, enemyTarget);

        Destroy(gameObject, 0.1f);
    }

    private void SpawnMiniMushrooms(Transform spawnPoint, Transform enemyTarget)
    {
        List<Transform> miniMushes = new List<Transform>();

        for (int i = 0; i < miniMushCount; i++)
        {
            Vector3 randomPos = spawnPoint.position + Random.insideUnitSphere * spawnRadius;

            GameObject miniMush = Instantiate(miniMushPrefab, randomPos, Quaternion.identity);

            miniMush.transform.localScale = miniMushPrefab.transform.localScale * miniMushScale;

            miniMushes.Add(miniMush.transform);
        }
        Debug.Log("Spawned " + miniMushes.Count + " mini mushrooms.");
        StartCoroutine(JumpTowardsEnemyAfterDelay(miniMushes, enemyTarget));
    }

    private IEnumerator JumpTowardsEnemyAfterDelay(List<Transform> miniMushes, Transform enemyTarget)
    {
        yield return new WaitForSeconds(jumpDelay);

        foreach (var t in miniMushes)
        {
            if (t == null) continue;
            if (enemyTarget == null) continue;
            Debug.Log("Mini mushroom jumping towards enemy at position: " + enemyTarget.position);
            StartCoroutine(JumpArc(t, enemyTarget.position));
        }
    }

    private IEnumerator JumpArc(Transform mini, Vector3 targetPos)
    {
        float elapsed = 0f;
        Vector3 startPos = mini.position;

        while (elapsed < jumpDuration)
        {
            float t = elapsed / jumpDuration;

            Vector3 flatPos = Vector3.Lerp(startPos, targetPos, t);
            float height = 4f * arcUpFactor * t * (1f - t);

            mini.position = new Vector3(flatPos.x, flatPos.y + height, flatPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mini.position = targetPos;
    }

    private void HideMushVisuals()
    {
        if (mushRenderers == null) return;

        foreach (var rend in mushRenderers)
        {
            if (rend != null)
                rend.enabled = false;
        }
    }

    private void PlayExplosionVfx()
    {
        if (explodeVFX == null) return;

        ParticleSystem ps = Instantiate(
            explodeVFX,
            transform.position,
            Quaternion.identity
        );
        ps.Play();

        var main = ps.main;
        Destroy(ps.gameObject, main.duration + main.startLifetime.constantMax + 0.2f);
    }
}
