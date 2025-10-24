using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Animator _anim;
    private Vector3 _scale;
    private bool isMove = false;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _scale = gameObject.transform.localScale;
    }

    public void ChangeDirection(float amount)
    {
        if (amount > 0)
        {
            transform.localScale = new Vector3(_scale.x,_scale.y,_scale.y);
        }
        else if (amount < 0)
        {
            transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.y);
        }
    }

    public void WalkAnim() {
        if (isMove) return; 
        _anim.SetBool("IsWalk", true);
        isMove = true;
    }
    public void IdleAnim() {
        _anim.SetBool("IsWalk", false);
        isMove = false;
    }
}
