using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateAnimationState(float moveInput, bool isGrounded, bool isRunning)
    {
        anim.SetBool("isJumping", !isGrounded);
        anim.SetBool("isRunning", isRunning && moveInput != 0);
    }
}
