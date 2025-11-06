using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyHealth health;
    EnemyController controller;
    [SerializeField] private int scoreValue;
    [SerializeField] private int xpValue;
    [SerializeField] public float damage;
    private Renderer[] renderers;

//    private void OnEnable()
//    {
//        //Subscribe to reveal all event
//    }

//    private void OnDisable()
//    {
//        //Unsubscribe to reveal all event
//    }

    private void Update()
    {
        //Trigger reveal when reveal all event is called
    }
    public void Reveal()
    {
        Debug.Log("Enemy is Revealing");
        foreach (var r in renderers) 
        { 
            r.enabled = true;
        }
    }

    public void Hide()
    {
        foreach (var r in renderers)
        {
            r.enabled = false;
        }
    }

    private void Start()
    {
        health = GetComponent<EnemyHealth>();
        controller = GetComponent<EnemyController>();
        renderers = GetComponentsInChildren<Renderer>();

        Hide();
    }
}
