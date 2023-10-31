using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 one of enemy classes
 */
public class Bird : MonoBehaviour
{
    /*
     mobName using like a link/id in scriptable object for searching sounds, scores and sprites
     */
    public string _mobName = "Bird";

    /*
     links to other simple classes  
     */
    /*to play, stop sounds, and ect.*/
    public AudioControl _audioControl;
    //
    public DeathControl _deathControl;
    public MovingInCircle _moveControl;

    public Animator _animator;

    public Rigidbody _rigidbody;

    public GameObject playerObj;
    public float distAttackToPlayer;


    public enum state { 
        Nothing
        , Flying
        , Attack
        , Fall
    }

    public state _state;

    private void Awake()
    {
        playerObj = GameObject.Find("Player");
        InitAnim();
        ChangeState(state.Flying);
        _deathControl.OnDeath += Death;

    }
    void Update() {

        if (Vector3.Distance(transform.position, playerObj.transform.position) <= distAttackToPlayer && _state != state.Fall) {
            ChangeState(state.Attack);
        }   
        else if(Vector3.Distance(transform.position, playerObj.transform.position) > distAttackToPlayer && _state != state.Fall) {
            ChangeState(state.Flying);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _state != state.Fall)
            GameManager.Instance.GameOver();
    }


    void ChangeState(state newState) {
        if (newState == _state)
            return;
        switch (newState) {
            case state.Flying:
                _state = newState;
                AnimFlying();
                AnimStopAttack();
                _audioControl.PlayAudioClip(_mobName, "Flying");
                break;
            case state.Attack:
                _state = newState;
                AnimAttack();
                _audioControl.PlayAudioOneTime(_mobName, "Attack");
                _audioControl.PlayAudioClip(_mobName, "Flying");
                break;
            case state.Fall:
                _state = newState;
                AnimStopFlying();
                AnimFall();
                _moveControl.StartFall();
                _audioControl.PlayAudioOneTime(_mobName, "Fall");
                _audioControl.PlayAudioClip(_mobName, "Falling");
                break;
            default:
                return;
        }
        return;
    }


    public void Death(float deathTimer) {
        ChangeState(state.Fall);
        _moveControl.ChangeMovingSpeed(0f);
        _audioControl.PlayAudioClip(_mobName, "Death");
        GameManager.Instance.AddScore(_mobName);
        StartCoroutine(DeathTimer(deathTimer));
    }

    IEnumerator DeathTimer(float deathTimer)
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }

    public void InitAnim() {
        _animator = GetComponent<Animator>();
    }

    public void AnimFlying() {
        _animator.SetBool("b_Flying", true);
    }
    public void AnimStopFlying()
    {
        _animator.SetBool("b_Flying", false);
    }

    public void AnimAttack() {
        _animator.SetBool("b_Attack", true);
    }

    public void AnimStopAttack() {
        _animator.SetBool("b_Attack", false);
    }

    public void AnimFall() {
        _animator.SetBool("b_Fall", true);
    }
}
