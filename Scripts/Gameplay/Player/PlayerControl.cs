using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControl : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    public float cameraMoveSpeed = 5.0f;
    public float fallSpeed = 10.0f;
    public float playerJumpForce = 50.0f;

    public Rigidbody playerRb;
    public PlayerInput crnt_PlayerInput;


    private SnowClimber gameActions;

    public GameObject fclPoint;

    public AudioPlayerConfigurator audioConfigurator;
    public AudioSource _audio;

    public float resumeAudioDelay = 0.1f;

    public Quaternion initRotation;
    public Vector3 initPosition;

    public bool isOnGround = true;

    public enum faceTo {
        Left
        , Right
        , Camera
    }
    public faceTo crntFaceTo;

    public enum state {
        Ground
        , OnCloud
        , OnSpeedBlock
        , Jump
        , Fall
        , Attack
    }

    public state crntState;

    [SerializeField]
    private HeadCollision crntHeadCol;

    [SerializeField]
    private FootCollision crntFootCol;

    public GameObject crntAttackObj;
    public float crntAttackTime = 1.0f;

    public PlayerSpaceChecker leftSpaceChecker;
    public PlayerSpaceChecker rightSpaceChecker;



    public GameObject playerModel;
    public Animator _animator;


    public GameObject crntMovingThing;
    public float constMovingThingsToRad = 5.0f;

    public float fallMinVal = -1.0f;

    public AudioControl _audioControl;

    public string actionMapPlayerControls = "GameplayControl";
    public string actionMapMenuControls = "UIControl";


    public void Start()
    {
        fclPoint = GameObject.Find("FocalPoint");

        crntFaceTo = faceTo.Left;
        crntState = state.Ground;
        initPosition = transform.localPosition;
        playerRb.freezeRotation = true;

        crntFootCol.OnGround += FallOnGround;
        crntFootCol.OnCloud += FallOnMovingObject;
        crntFootCol.OnSpeedBlock += FallOnSpeedBlock;

        gameActions = InputManager.inputActions;
        gameActions.GameplayControl.Enable();
    }

    public void Update()
    {
        
        if (gameActions.GameplayControl.Pause.triggered)
        {
            GameManager.Instance.ChangePaused();
        }
        if (gameActions.GameplayControl.Skip.triggered)
        {
            GameManager.Instance.CompleteLevel();
        }

        if (GameManager.IsGameplay())
        {
            transform.localPosition = new Vector3(initPosition.x, transform.localPosition.y, initPosition.z);
            Vector2 move = new Vector2(0.0f, 0.0f);
            if (
                CanItMove())
            {
                move = gameActions.GameplayControl.Move.ReadValue<Vector2>();
            }
            else if (crntState == state.Jump || crntState == state.Fall)
            {
                switch (crntFaceTo)
                {
                    case faceTo.Left:
                        move = new Vector2(-fallSpeed, 0.0f);
                        break;
                    case faceTo.Right:
                        move = new Vector2(fallSpeed, 0.0f);
                        break;
                }

            }
            Move(move);


            if (gameActions.GameplayControl.Jump.triggered && (CanItJump()))
            {
                Jump();
            }

            if (playerRb.velocity.y < fallMinVal && crntState == state.Jump)
            {
                StartFall();
            }
            if (crntState == state.Jump && crntHeadCol.jumped == false)
            {
                StartFall();
            }

            if (gameActions.GameplayControl.Attack.triggered && CanItMove())
            {
                crntState = state.Attack;
                _audioControl.PlayAudioOneTime("Player", "Attack");
                crntAttackObj.GetComponent<Attack>().OnAttack();
                StartCoroutine(Attack());
            }
        }

    }

    public void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
        {
            AnimStopMoving();
        }
        float scaledMoveSpeed = moveSpeed * Time.deltaTime;
        float scaledCameraMoveSpeed = cameraMoveSpeed * Time.deltaTime;
        float scaledThirdThingsMoveSpeed = 0; 
        if (crntState == state.OnCloud) {
            scaledThirdThingsMoveSpeed = crntMovingThing.GetComponentInParent<MovingInCircle>().getMoveAngSpeed() * Time.deltaTime;
        }

        if (crntState == state.OnSpeedBlock) {
            scaledThirdThingsMoveSpeed = crntMovingThing.GetComponentInParent<SpeedBlock>().GetSpeedBost() * Time.deltaTime;
        }

        if (direction.sqrMagnitude > 0.01)
        {
            if (direction.x < 0 && faceTo.Right == crntFaceTo)
            {
                Turn(faceTo.Left);
                AnimTurn();
            }
            else if (direction.x > 0 && faceTo.Left == crntFaceTo)
            {
                Turn(faceTo.Right);
                AnimTurn();
            }
            if (direction.x < 0 && leftSpaceChecker.spaceEmpty == true)
            {
                fclPoint.transform.Rotate(Vector3.down, direction.x * scaledCameraMoveSpeed + scaledThirdThingsMoveSpeed * Mathf.Rad2Deg* constMovingThingsToRad);
                AnimMoving();

            }
            if (direction.x > 0 && rightSpaceChecker.spaceEmpty == true)
            {
                fclPoint.transform.Rotate(Vector3.down, direction.x * scaledCameraMoveSpeed + scaledThirdThingsMoveSpeed * Mathf.Rad2Deg* constMovingThingsToRad);
                AnimMoving();
            }

            switch (crntState) {
                case state.Ground:
                case state.OnSpeedBlock:
                    _audioControl.PlayAudioClip("Player", "Move");
                    break;
                case state.OnCloud:
                    _audioControl.PlayAudioClip("Player", "MoveCloud");
                    break;
                default:
                    _audioControl.PlayAudioClip("Player", "Empty");
                    break;
            }
        }
        else {
            if (scaledThirdThingsMoveSpeed < 0 && leftSpaceChecker.spaceEmpty == true)
            {
                fclPoint.transform.Rotate(Vector3.down, scaledThirdThingsMoveSpeed * Mathf.Rad2Deg* constMovingThingsToRad);
            }
            if (scaledThirdThingsMoveSpeed > 0 && rightSpaceChecker.spaceEmpty == true)
            {
                fclPoint.transform.Rotate(Vector3.down, scaledThirdThingsMoveSpeed * Mathf.Rad2Deg* constMovingThingsToRad);
            }

            switch (crntState)
            {
                case state.Ground:
                    StopPlayAudio();
                    break;
                case state.OnCloud:
                    StopPlayAudio();
                    break;
                case state.OnSpeedBlock:
                    StopPlayAudio();
                    break;


            }
        }

    }

    /*
     REMOVE
     
    public void ShowBindings() {
        Debug.Log(crnt_PlayerInput.actions["Attack"].bindings[0]);
        InputManager.GetCurrentBinding("Attack", 0);

    }
    */


    private void Falling() {
        
        isOnGround = true;
        crntHeadCol.jumped = false;
        AnimStopFalling();
        if (transform.position.y >= GameManager.Instance.GetCurrentFloor() * GameManager.Instance.GetFloorHeigh())
            GameManager.Instance.NextFloor();
    }

    private void FallOnGround() {
        if (GameManager.IsGameplay())
        {
            Falling();
            crntState = state.Ground;
        }
    }

    private void FallOnMovingObject()
    {
        if (GameManager.IsGameplay())
        {
            Falling();
            crntState = state.OnCloud;
            crntMovingThing = crntFootCol.crntMovingThing;
        }
    }
    private void FallOnSpeedBlock() {
        if (GameManager.IsGameplay())
        {
            Falling();
            crntState = state.OnSpeedBlock;
            crntMovingThing = crntFootCol.crntMovingThing;
        }
    }

    public void Jump() {
        crntHeadCol.jumped = true;
        _audioControl.PlayAudioOneTime("Player", "Jump");
        playerRb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        isOnGround = false;
        crntState = state.Jump;    
        AnimJump();
        
        // td add anim

    }

    public void StartFall() {
        AnimStopFlying();

        crntState = state.Fall;
        crntHeadCol.jumped = false;
        playerRb.ResetInertiaTensor();
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
    }

    IEnumerator Attack() {
        // td addAnimation
        AnimAttack();
        yield return new WaitForSeconds(crntAttackTime);
        crntState = state.Ground;
        
        crntAttackObj.GetComponent<Attack>().EndAttack();
    }

        


    public void Turn(faceTo newFaceTo) {
        switch (newFaceTo) {
            case faceTo.Right:
                crntFaceTo = newFaceTo;
                crntAttackObj.GetComponent<Attack>().ChangeSide();
                playerModel.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                break;
            case faceTo.Left:
                crntFaceTo = newFaceTo;
                crntAttackObj.GetComponent<Attack>().ChangeSide();
                playerModel.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                break;
            case faceTo.Camera:
                crntFaceTo = newFaceTo;
//                playerModel.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                break;
        }
    }

    public void AnimMoving() {
        _animator.SetBool("b_Moving", true);
        
    }

    public void AnimTurn() {
        _animator.SetTrigger("t_Turn");
    }

    public void AnimJump() {
        _animator.SetBool("b_Flying", true);
        _animator.SetTrigger("t_Jump");

    }

    public void AnimAttack() {
        _animator.SetTrigger("t_Attack");
    }

    public void AnimStopMoving()
    {
        _animator.SetBool("b_Moving", false);
    }

    public void AnimStopFlying() {
        _animator.SetBool("b_Flying", false);
        _animator.SetBool("b_Fall", true);
    }

    public void AnimStopFalling() {
        _animator.SetBool("b_Fall", false);
    }


    //actionMa

    void RemoveAllBindingOverrides()
    {
        InputActionRebindingExtensions.RemoveAllBindingOverrides(crnt_PlayerInput.currentActionMap);
    }

    public void EnableGameplayControls()
    {
        crnt_PlayerInput.SwitchCurrentActionMap(actionMapPlayerControls);
    }

    public void EnablePauseMenuControls()
    {
        crnt_PlayerInput.SwitchCurrentActionMap(actionMapMenuControls);
    }


    /*
    public void PlayAudioOneTime(string carrier, string action) {
        _audio.Stop();
        AudioClip audioClip = audioConfigurator.GetAudioClip(carrier, action);
        _audio.PlayOneShot(audioClip);
        StartCoroutine(ResumePlayAudio(audioClip.length));
    }

    public void PlayAudioClip(string carrier, string action) {
        AudioClip newAudioClip = audioConfigurator.GetAudioClip(carrier, action);
        if (_audio.clip != newAudioClip || !_audio.isPlaying)
        {
            _audio.clip = newAudioClip;
            _audio.Play();
        }
    }

    IEnumerator ResumePlayAudio(float waitLength) {
        yield return new WaitForSeconds(waitLength+ resumeAudioDelay);
        if (!_audio.isPlaying)
            _audio.Play();
    }
    */


    public bool CanItMove() {

        switch (crntState) {
            case state.Ground:
            case state.OnCloud:
            case state.OnSpeedBlock:
                return true;
            default:
                return false;
        }
    }

    public bool CanItJump()
    {

        switch (crntState)
        {
            case state.Ground:
            case state.OnCloud:
            case state.OnSpeedBlock:
                return true;
            default:
                return false;
        }
    }
    public void StopPlayAudio() {
        _audio.Stop();
    }

    public void OnPause()
    {
        
    }

}
