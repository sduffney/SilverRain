using UnityEngine;

[ExecuteInEditMode]
public class ForestGenerator : MonoBehaviour
{
    [Header("References")]
    public GameObject treePrefab;
    public GameObject areaObject;

    [Header("Density and scale")]
    public float density = 1f;
    public float minScale = 0.8f;
    public float maxScale = 1.5f;

    [Header("Placement")]
    public bool snapToSurface = true;
    public int randomSeed = 0;
    public string containerName = "GeneratedForest";

    [HideInInspector]
    public int lastGeneratedCount = 0;

    // Public method available in context menu (Component -> ... -> Generate Forest)
    [ContextMenu("Generate Forest")]
    public void GenerateForest()
    {
        if (treePrefab == null)
        {
            Debug.LogWarning("ForestGenerator: treePrefab is not assigned.");
            return;
        }

        Bounds bounds = GetAreaBounds();
        if (bounds.size == Vector3.zero)
        {
            Debug.LogWarning("ForestGenerator: areaObject has no bounds. Assign an object with renderers, or position this GameObject to define area.");
            return;
        }

        // Determine count based on XZ area * density
        float areaXZ = Mathf.Abs(bounds.size.x * bounds.size.z);
        int count = Mathf.FloorToInt(areaXZ * density);
        count = Mathf.Max(0, count);

        if (count == 0)
        {
            Debug.Log("ForestGenerator: density or area too small, no trees generated.");
            lastGeneratedCount = 0;
            return;
        }

        // Create or find container
        string nameToUse = string.IsNullOrEmpty(containerName) ? "GeneratedForest" : containerName;
        Transform container = transform.Find(nameToUse);
        if (container == null)
        {
            GameObject containerGO = new GameObject(nameToUse);
            containerGO.hideFlags = HideFlags.None;
            containerGO.transform.SetParent(transform, false);
            container = containerGO.transform;
        }

        // Optionally seed RNG for repeatable placements
        System.Random prng = null;
        bool seeded = randomSeed != 0;
        if (seeded) prng = new System.Random(randomSeed);

        int created = 0;
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = new Vector3(
                RandomRange(bounds.min.x, bounds.max.x, prng),
                bounds.min.y,
                RandomRange(bounds.min.z, bounds.max.z, prng)
            );

            Vector3 spawnPos = randomPos;

            if (snapToSurface)
            {
                // cast from above bounds.max.y + margin downwards
                float castStartY = bounds.max.y + 5f;
                RaycastHit hit;
                Ray down = new Ray(new Vector3(randomPos.x, castStartY, randomPos.z), Vector3.down);
                if (Physics.Raycast(down, out hit, (castStartY - bounds.min.y) + 10f))
                {
                    spawnPos.y = hit.point.y;
                }
                else
                {
                    // if nothing hit, keep at bounds.min.y
                    spawnPos.y = bounds.min.y;
                }
            }

            GameObject instance;
            // Instantiate works for both prefabs and scene objects in the editor
            instance = Instantiate(treePrefab, spawnPos, Quaternion.identity, container);

            // Random uniform scale
            float scale = RandomRange(minScale, maxScale, prng);
            instance.transform.localScale = Vector3.one * scale;

            created++;
        }

        lastGeneratedCount = created;
        Debug.Log($"ForestGenerator: generated {created} trees under '{nameToUse}'.");
    }

    [ContextMenu("Clear Generated Trees")]
    public void ClearGeneratedTrees()
    {
        string nameToUse = string.IsNullOrEmpty(containerName) ? "GeneratedForest" : containerName;
        Transform container = transform.Find(nameToUse);
        if (container == null)
        {
            Debug.Log("ForestGenerator: no generated container found to clear.");
            return;
        }

        // DestroyImmediate works in editor
#if UNITY_EDITOR
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(container.GetChild(i).gameObject);
        }
        DestroyImmediate(container.gameObject);
#else
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
        Destroy(container.gameObject);
#endif

        lastGeneratedCount = 0;
        Debug.Log("ForestGenerator: cleared generated trees.");
    }

    public Bounds GetAreaBounds()
    {
        if (areaObject == null)
        {
            // fallback to a small bounds around this transform
            return new Bounds(transform.position, Vector3.zero);
        }

        Renderer[] rends = areaObject.GetComponentsInChildren<Renderer>();
        if (rends == null || rends.Length == 0)
        {
            // if no renderers, fallback to areaObject position
            return new Bounds(areaObject.transform.position, Vector3.zero);
        }

        Bounds b = rends[0].bounds;
        for (int i = 1; i < rends.Length; i++)
            b.Encapsulate(rends[i].bounds);

        return b;
    }

    // Helper random range that can use System.Random for deterministic results if provided
    float RandomRange(float a, float b, System.Random prng = null)
    {
        if (prng == null)
            return Random.Range(a, b);
        double v = prng.NextDouble(); // [0,1)
        return Mathf.Lerp(a, b, (float)v);
    }
}
