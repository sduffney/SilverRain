using UnityEngine;

public class BloodSplatterTest : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem bloodSplatterPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TestBloodSplatter();
        }
    }

    private void TestBloodSplatter() 
    {
        Vector3 bloodSplatterSpawn = transform.position;
        bloodSplatterSpawn.y += 2;
        var bloodSplatter = Instantiate(bloodSplatterPrefab, bloodSplatterSpawn, Quaternion.identity);

        bloodSplatter.Play();
        Destroy(bloodSplatter, bloodSplatter.main.duration);
    }
}
