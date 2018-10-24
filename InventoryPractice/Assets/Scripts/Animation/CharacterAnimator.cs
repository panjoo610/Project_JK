using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterAnimator : MonoBehaviour {

    public AnimationClip replaceableAttackAnim;
    public AnimationClip[] defaultAttackAnimSet;
    protected AnimationClip[] currentAttackAnimSet;
    

    const float locomationAnimationSmoothTime = 0.1f;
    protected NavMeshAgent agent;

    protected Animator animator;

    protected CharacterCombat combat;
    protected AnimatorOverrideController overrideController;


	// Use this for initialization
	protected virtual void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        combat = GetComponent<CharacterCombat>();
        if(overrideController == null)
        {
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        }
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        currentAttackAnimSet = defaultAttackAnimSet;
        combat.OnAttack += OnAttack;
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;

        animator.SetFloat("SpeedPercent", speedPercent, locomationAnimationSmoothTime, Time.deltaTime);

        animator.SetBool("InCombat", combat.InCombat);

        animator.SetBool("Die", combat.Die);
    }

     protected virtual void OnAttack()
     {
        animator.SetTrigger("Attack");
        int attackIndex = Random.Range(0, currentAttackAnimSet.Length);
        overrideController[replaceableAttackAnim.name] = currentAttackAnimSet[attackIndex];
     }
    protected virtual IEnumerator FadeOut(GameObject gObject)
    {
        Color color = new Vector4(1, 1, 1, 0);
        //transform.renderer.material.color = color;
        int gRendererCount = gObject.GetComponentsInChildren<Renderer>().Length;
        int chackCount = 0;
        Renderer[] renderers = new Renderer[gRendererCount];
        renderers = gObject.GetComponentsInChildren<Renderer>();

        bool isDone = false;
        while (isDone == false)
        {
            Debug.Log("FadeOut in");
            for (int i = 0; i < renderers.Length; i++)
            {
                //renderers[i].material.
                renderers[i].material.color = Color.Lerp(renderers[i].material.color, color, Time.deltaTime); 
                if(renderers[i].material.color == color)
                {
                    chackCount += 1;
                    if(chackCount >= gRendererCount)
                    {
                        Debug.Log("FadeOut Done");
                        isDone = true;
                    }
                }
            }
            yield return null;
        }
        yield return null;
        
    }
}
