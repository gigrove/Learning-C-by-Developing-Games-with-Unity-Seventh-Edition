using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;
    public float JumpVelocity = 5f;
    public float DistanceToGround = 0.1f;
    public LayerMask GroundLayer;
    public GameObject Bullet;
    public float BulletSpeed = 100f;

    private float _vInput;
    private float _hInput;
    private bool _isJumping;
    private CapsuleCollider _col;
    private bool _isShooting;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;

        _isJumping |= Input.GetKeyDown(KeyCode.Space);

        _isShooting |= Input.GetMouseButtonDown(0);

        /*
        this.transform.Translate(Vector3.forward * _vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * _hInput *Time.deltaTime);
        */



    }

    void FixedUpdate()
    {
        if (_isJumping && IsGrounded())
        {
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
        }
        _isJumping = false;

        Vector3 rotation = Vector3.up * _hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + this.transform.forward * _vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);

        if (_isShooting)
        {
            GameObject newBullet = Instantiate(Bullet, this.transform.position +
                new Vector3(0f, 0f, 1f), this.transform.rotation);
            Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();
            BulletRB.velocity = this.transform.forward * BulletSpeed;
        }
        _isShooting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "KillPlane")
        {
            this.transform.SetPositionAndRotation(Vector3.up * 5, Quaternion.identity);
            Debug.Log("Don't fall off!");
        }
    }


    private bool IsGrounded()
    {
        Vector3 Capsulebottom = new(_col.bounds.center.x, _col.bounds.min.y,
            _col.bounds.center.x);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, Capsulebottom,
            DistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }
}