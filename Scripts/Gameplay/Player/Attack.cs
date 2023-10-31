using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private bool crntStateAttack = false;

    public GameObject hammer;
    public Animator _animator;
    // Start is called before the first frame update

    public void Awake() {
        _animator = hammer.GetComponent<Animator>();
    }
    void OnTriggerStay(Collider other)
    {
        if (crntStateAttack && other.CompareTag("Enemy"))
        {
            DeathControl dthCntrl = other.GetComponent<DeathControl>();
            if (dthCntrl != null)
            {
                dthCntrl.Death();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void OnAttack() {
        hammer.SetActive(true);
        AnimAttack();
        crntStateAttack = true;
    }

    public void EndAttack()
    {
        hammer.SetActive(false);
        crntStateAttack = false;
    }

    public void ChangeSide() {
        transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        hammer.transform.localPosition = new Vector3(-hammer.transform.localPosition.x, hammer.transform.localPosition.y, hammer.transform.localPosition.z);
        hammer.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
    }

    public void AnimAttack() {
        _animator.SetTrigger("t_Attack");
    }

}
