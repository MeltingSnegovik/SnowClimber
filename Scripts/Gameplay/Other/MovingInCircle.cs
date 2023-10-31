using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingInCircle : MonoBehaviour
{
    public Rigidbody crntRb;

    public Vector3 initPosition;

    public float raduis;

    public float crntAng;

    public float moveAnglSpeed;
    public float crtMoveAnglSpeed;

    public Vector3 addToPos;
    public Vector3 addToRot;

    public AnimationCurve movementYCurve;

    public bool isFall;

    private float _time;
    [SerializeField]
    private float updMovePosY;
    private float initMovePosY;

    void Awake() {
        _time = 0f;
        crntRb = GetComponent<Rigidbody>();
//        crntRb.freezeRotation = true;
        initPosition = transform.position;
        raduis = Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.z * transform.position.z);
        crtMoveAnglSpeed = moveAnglSpeed;
    }

    void Update() {
        Move();
    }
        
    void Move()
    {
        _time += Time.deltaTime;
        updMovePosY = movementYCurve.Evaluate(_time);
        float crtMoveAnglSpeed = moveAnglSpeed * Time.deltaTime;

        crntAng = Mathf.Atan2(transform.position.x, transform.position.z);
        float newAnlg = crntAng + crtMoveAnglSpeed;

        Vector3 newPos = new Vector3(0,0,0);
        Vector3 newRot = new Vector3(0, 0, 0);

        if (!isFall)
        {
           newPos = new Vector3(raduis * Mathf.Sin(newAnlg), initPosition.y + updMovePosY, raduis * Mathf.Cos(newAnlg)) + addToPos;
           newRot = new Vector3(0, newAnlg * Mathf.Rad2Deg, 0) + addToRot;
        }
        else {
            newPos = new Vector3(raduis * Mathf.Sin(newAnlg), transform.position.y, raduis * Mathf.Cos(newAnlg)) + addToPos;
            newRot = new Vector3(0, newAnlg * Mathf.Rad2Deg, 0) + addToRot;
        }

        transform.position = newPos;
        transform.rotation = Quaternion.Euler(newRot);
    }


    public float getMoveAngSpeed() {
        return moveAnglSpeed;
    }

    public void ChangeMovingSpeed(float newMoveAnglSpeed) {
        moveAnglSpeed = newMoveAnglSpeed;
    }

    public void StartFall() {
        isFall = true;
        crntRb.useGravity = true;
    }
}
