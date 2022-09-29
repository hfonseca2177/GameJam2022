using UnityEngine;

public class AnimationName : MonoBehaviour
{
    [SerializeField] private string deathAnimation;
    [SerializeField] private string idleAnimation;
    [SerializeField] private string fallAnimation;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayDeath()
    {
        anim.Play(deathAnimation);
    }
    public void PlayIdle()
    {
        anim.Play(idleAnimation);
    }
    public void PlayFall()
    {
        anim.Play(fallAnimation);
    }

}
