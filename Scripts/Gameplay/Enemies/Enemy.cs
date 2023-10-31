using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum state
    {
        Normal
    , Warning
    , Returning
    , Fall
    , Rolling
    }


    public string _mobName = "Enemy";

    protected float InitRotY = 0;
    protected float raduis;
    protected float crntAng;

    protected Rigidbody crntRb;

    [SerializeField]
    protected float moveAnglSpeed = 0.1f;

    [SerializeField]
    protected Vector3 dirRayBlock;
    
    [SerializeField]
    protected Vector3 dirRayBigBlock;

    [SerializeField]
    protected float rayDist = 5.0f;

    [SerializeField]
    protected float rayPosFowardCor = 0.5f;

    [SerializeField]
    protected float fallMinVal = -1.0f;

    [SerializeField]
    protected state crntState;


    public GameObject blockPrefab;
    public GameObject blockSmallPrefab;
    public float blockHeigh;

    [SerializeField]
    protected float blockAngDif;

    [SerializeField]
    protected float blockAngDifSmallBlock;

    [SerializeField]
    protected float blockRotateDifY;

    [SerializeField]
    protected float blockRotateDifX;

    /*
    [SerializeField]
    protected AudioPlayerConfigurator audioConfigurator;
    */


    [SerializeField]
    protected AudioControl _audioControl;
//    protected AudioSource _audio;

    [SerializeField]
    protected DeathControl deathControl;

    [SerializeField]
    protected float resumeAudioDelay = 0.1f;

    void Awake()
    {
        crntRb = GetComponent<Rigidbody>();

        crntRb.freezeRotation = true;

        raduis = Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.z * transform.position.z);
        crntAng = Mathf.Atan2(transform.position.x, transform.position.z);
        InitRotY = transform.rotation.eulerAngles.y - crntAng * Mathf.Rad2Deg;

        deathControl.OnDeath += Death;

        ChangeState(state.Normal);
        InitAnim();
    }

    void Update()
    {

        AnimControl();
        if (crntRb.velocity.y < fallMinVal && !deathControl.IsDestroyed()) {
            ChangeState(state.Fall);
        }
        if (crntState == state.Fall && crntRb.velocity.y > fallMinVal && !deathControl.IsDestroyed())
        {
            ChangeState(state.Rolling);
        }
        Move();
        if (crntState == state.Normal || crntState == state.Returning)
        {
            TryFindBlock();
        }
    }

    void Move() {
        float crtMoveAnglSpeed = moveAnglSpeed * Time.deltaTime;

        crntAng = Mathf.Atan2(transform.position.x, transform.position.z);
        float newAnlg = crntAng + crtMoveAnglSpeed;


        transform.position = new Vector3(raduis * Mathf.Sin(newAnlg), transform.position.y, raduis * Mathf.Cos(newAnlg));
        transform.rotation = Quaternion.Euler(0, InitRotY + newAnlg * Mathf.Rad2Deg, 0);
    }

    void TryFindBlock() {
        RaycastHit hit;
        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        Vector3 dirPos = transform.position + rayPosFowardCor * transform.forward;



        if (Physics.Raycast(dirPos, transform.TransformDirection(dirRayBlock), out hit, rayDist, layerMask)) {
            Debug.DrawRay(dirPos, transform.TransformDirection(dirRayBlock) * hit.distance, Color.yellow);
            /*
            if (
                !hit.collider.CompareTag("Block") 
                && !hit.collider.CompareTag("UnbreakableBlock") 
                && !hit.collider.CompareTag("SpeedBlock")
                ) {
                if (crntState == state.Normal)
                {
                    SpaceFound();
                }
                if (crntState == state.Returning)
                {
                    PutTheBlock();
                }
            }
            */
        }
        else
        {
            Debug.DrawRay(dirPos, transform.TransformDirection(dirRayBlock) * 1000, Color.white);
            if (crntState == state.Normal)
            {
                SpaceFound();
            }
            if (crntState == state.Returning) {
                RaycastHit hitBigBlock;
                Debug.DrawRay(dirPos, transform.TransformDirection(dirRayBigBlock) * rayDist, Color.red);
                if (!Physics.Raycast(dirPos, transform.TransformDirection(dirRayBigBlock), out hitBigBlock, rayDist, layerMask))
                {
                    PutTheBlock();
                }
                else {
                    PutTheSmallBlock();
                }
                
            }
        }
    }

    void SpaceFound() {
        ChangeState(state.Warning);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Respawn") && crntState == state.Warning) {
            ChangeState(state.Returning);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Respawn") && crntState == state.Warning)
        {
            ChangeState(state.Returning);
        }
    }

    void ChangeState(state newState) {
        switch (newState) {
            case state.Warning:
                crntState = state.Warning;
                moveAnglSpeed *= -2;
                InitRotY = InitRotY + 180;
                _audioControl.PlayAudioOneTime(_mobName, "Warning");
                _audioControl.PlayAudioClip(_mobName, "Moving");
                AnimTurn();
                break;
            case state.Returning:
                crntState = state.Returning;
                moveAnglSpeed /= -2;
                InitRotY = InitRotY - 180;
                _audioControl.PlayAudioClip(_mobName, "MovingWithBlock");
                break;
            case state.Fall:
                crntState = state.Fall;
                _audioControl.PlayAudioClip(_mobName, "Falling");
                StartFall();
                break;
            case state.Normal:
                crntState = state.Normal;
                _audioControl.PlayAudioClip(_mobName, "Moving");
                //                moveAnglSpeed =2;
                break;
            case state.Rolling:
                crntState = state.Rolling;
                //                moveAnglSpeed *= 1.5f;
                _audioControl.PlayAudioOneTime(_mobName, "Fall");
                _audioControl.PlayAudioClip(_mobName, "Rolling");
                AnimRolling();
                break;
            default:
                return;
        }
        return;
    }

    void PutTheBlock() {

        Vector3 blockPos = new Vector3(raduis * Mathf.Sin(crntAng + blockAngDif), blockHeigh, raduis * Mathf.Cos(crntAng + blockAngDif));
        Quaternion blockRot = Quaternion.Euler(blockPrefab.transform.rotation.eulerAngles + transform.rotation.eulerAngles + Vector3.up * blockAngDif * Mathf.Rad2Deg + Vector3.up * blockRotateDifY * Mathf.Rad2Deg + Vector3.right * blockRotateDifX);
        Instantiate(blockPrefab, blockPos, blockRot);
        _audioControl.PlayAudioOneTime(_mobName, "InstallBlock");
        ChangeState(state.Normal);
    }

    void PutTheSmallBlock()
    {

        Vector3 blockPos = new Vector3(raduis * Mathf.Sin(crntAng + blockAngDifSmallBlock), blockHeigh, raduis * Mathf.Cos(crntAng + blockAngDifSmallBlock));
        Quaternion blockRot = Quaternion.Euler(blockSmallPrefab.transform.rotation.eulerAngles + transform.rotation.eulerAngles + Vector3.up * blockAngDifSmallBlock * Mathf.Rad2Deg + Vector3.up * blockRotateDifY * Mathf.Rad2Deg + Vector3.right * blockRotateDifX);
        Instantiate(blockSmallPrefab, blockPos, blockRot);
        _audioControl.PlayAudioOneTime(_mobName, "InstallBlock");
        ChangeState(state.Normal);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.IsGameplay() && !deathControl.IsDestroyed())
        {
            GameManager.Instance.GameOver();
        }
        return;
    }

    private void StartFall() {
        deathControl.Death();
        AnimFall();
    }

    public virtual void InitAnim() {
    }

    public virtual void AnimControl() {
    }

    public virtual void AnimTurn() {
    }

    public virtual void AnimFall() {
    }

    public virtual void AnimRolling() {
    }

    /*
    public void PlayAudioOneTime(string carrier, string action)
    {
        _audio.Stop();
        AudioClip audioClip = audioConfigurator.GetAudioClip(carrier, action);
        _audio.PlayOneShot(audioClip);
        StartCoroutine(ResumePlayAudio(audioClip.length));
    }

    public void PlayAudioClip(string carrier, string action)
    {
        AudioClip newAudioClip = audioConfigurator.GetAudioClip(carrier, action);
        if (_audio.clip != newAudioClip || !_audio.isPlaying)
        {
            _audio.clip = newAudioClip;
            _audio.Play();
        }
    }

    public void StopPlayAudio()
    {
        _audio.Stop();
    }

    IEnumerator ResumePlayAudio(float waitLength)
    {
        yield return new WaitForSeconds(waitLength+ resumeAudioDelay);
        if (!_audio.isPlaying)
            _audio.Play();
    }
    */

    public virtual void Death(float deathTimer) {
        GameManager.Instance.AddScore(_mobName);
        ChangeState(state.Rolling);
        _audioControl.PlayAudioClip(_mobName, "Destroy");
        StartCoroutine(DeathTimer(deathTimer));
    }

    IEnumerator DeathTimer(float deathTimer)
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }

}
