using UnityEngine;

public class GoblinUIDeathSync : MonoBehaviour
{
    [SerializeField]
    private Animator goblinAAnimator;
    [SerializeField]
    private GameObject goblinB;

    private Renderer[] goblinBRenderers;

    private string idleStateName = "Idle";
    //private string deathStateName = "Death";

    private void Awake()
    {
            goblinAAnimator = GetComponent<Animator>();

            goblinBRenderers = goblinB.GetComponentsInChildren<Renderer>(true);
    }

    private void Update()
    {
        AnimatorStateInfo currentState = goblinAAnimator.GetCurrentAnimatorStateInfo(0);

        bool showEnemyB = !currentState.IsName(idleStateName);
        ToggleRenderers(goblinBRenderers, showEnemyB);
    }

    private void ToggleRenderers(Renderer[] renderers, bool visible)
    {
        foreach (Renderer r in renderers) 
        {
            r.enabled = visible;
        }

    }

}
