using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class movimento_giocatore : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField]
    private Transform _rotorTransform;

    private float _roll;
    private float _pitch;
    private float _Yaw;

    private float _throttle;
    [SerializeField]
    private float _throttleAmt;
    [SerializeField]
    private float _maxThrottle;
    [SerializeField]
    private float responsiveness;
    [SerializeField]
    private GameObject Water;
    [SerializeField]
    private float waterDifferance;
    [SerializeField]
    private float Boyancy;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        HandleInputs();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {

        _rigidbody.AddForce(transform.up * _maxThrottle * _throttle, ForceMode.Impulse);
        _rotorTransform.Rotate(0, _throttle, 0);

        if (transform.position.y < Water.transform.position.y+ waterDifferance)
        {
            _rigidbody.drag = 5;
            _rigidbody.angularDrag = 1;
            _rigidbody.AddForce(new Vector3(0,1,0) * Boyancy * Mathf.Abs(transform.position.y - Water.transform.position.y -waterDifferance), ForceMode.Impulse);


            Vector3 capsizeCorrectionRad = (Mathf.Deg2Rad * transform.localEulerAngles);

            transform.Rotate(-Mathf.Sin(capsizeCorrectionRad.x), -Mathf.Sin(capsizeCorrectionRad.y), -Mathf.Sin(capsizeCorrectionRad.z));
        }
        else
        {
            _rigidbody.drag = 1;
            _rigidbody.angularDrag = 1;
            _rigidbody.AddTorque(transform.forward * _roll * responsiveness);
            _rigidbody.AddTorque(transform.right * _pitch * responsiveness);
            _rigidbody.AddTorque(transform.up * _Yaw * responsiveness);
        }

    }
    void HandleInputs()
    {

        _roll = Input.GetAxis("Roll");
        _pitch = Input.GetAxis("Pitch");
        _Yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space))
            _throttle += Time.deltaTime * _throttleAmt;
        if (Input.GetKey(KeyCode.LeftShift))
            _throttle -= Time.deltaTime * _throttleAmt;

        _throttle = Mathf.Clamp(_throttle,0f,100f);
    }

}
