using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
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

    private Rigidbody _rigidBody;

    private CapsuleCollider _capCol;

    private bool _isShooting;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _capCol = GetComponent<CapsuleCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;

        _isJumping |= Input.GetKeyDown(KeyCode.J);

        /**
        this.transform.Translate(Vector3.forward * _vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * _hInput * Time.deltaTime);
        //*/

        _isShooting |= Input.GetKeyDown(KeyCode.Space);

    }

    private void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * _hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rigidBody.MovePosition(this.transform.position + 
            this.transform.forward * _vInput * Time.fixedDeltaTime);
        _rigidBody.MoveRotation(_rigidBody.rotation * angleRot);


        if (_isJumping && IsGrounded())
        {
            _rigidBody.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);

        }

        _isJumping = false;

        if (_isShooting)
        {
            GameObject newBullet = Instantiate(Bullet, this.transform.position +
                new Vector3(0, 0, 1), this.transform.rotation);

            Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();
            BulletRB.velocity = this.transform.forward * BulletSpeed;

        }

        _isShooting = false;
        
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_capCol.bounds.center.x, 
            _capCol.bounds.min.y, _capCol.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_capCol.bounds.center, capsuleBottom, 
            DistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);

        return grounded;

    }

}
