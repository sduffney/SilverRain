using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyHealth health;
    EnemyController controller;
    [SerializeField] private int scoreValue;
    [SerializeField] private float xpValue;
    [SerializeField] public float damage;
    //[SerializeField] private float goldValue;
    private Renderer[] renderers;

    private void OnEnable()
    {
        //Subscribe to reveal all event
        EnemyEvents.OnGlobalReveal += RevealTimed;
    }

    private void OnDisable()
    {
        //Unsubscribe to reveal all event
        EnemyEvents.OnGlobalReveal -= RevealTimed;
    }

    private void Update()
    {
        //Trigger reveal when reveal all event is called
        health.DamageTest();
    }
    public void Reveal()
    {
        //Debug.Log("Enemy is Revealing");
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

    public void RevealTimed(float seconds) 
    {
        //Debug.Log("Timed Reveal Start");
        StopAllCoroutines();
        StartCoroutine(RevealCorutine(seconds));
    }

    private void Start()
    {
        health = GetComponent<EnemyHealth>();
        controller = GetComponent<EnemyController>();
        renderers = GetComponentsInChildren<Renderer>();

        Hide();
    }

    private IEnumerator RevealCorutine(float duration) 
    {
        Reveal();
        yield return new WaitForSeconds(duration);
        Hide();
    }

    public float RewardXP() 
    {
        return xpValue;
    }
    public int RewardScore()
    {
        return scoreValue;
    }
}
