using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class ShipMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _sailControlsAction;
    [SerializeField] private ShipData _shipData;

    [Header("Ship variables")]
    [SerializeField] private float _sailStatusSpeed = 10f; //raising/lowering sails speed. couldn't think of a better name
    [SerializeField] private float _sailRotationSpeed = 10f;
    [SerializeField] private float _mass = 2f;
    [SerializeField] private float _angularDrag = 3f;

    [Header("Env variables")]
    [SerializeField] private Vector3 _windDirection = new Vector3(1f, 0f, 0f);
    [SerializeField] private float _windSpeed = 16f;
    [SerializeField] private float _waterDrag = 0.2f;
    [SerializeField] private float _windTurningStrength = 15f;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem _windParticles;

    private float _sailStatus = 0f;
    private float _sailRotation = 0f;
    private Vector3 _currentVelocity = Vector3.zero;
    private float _currentAngularVelocity = 0f;

    private List<Transform> _sails = new List<Transform>();
    private List<Transform> _masts = new List<Transform>();

    private void Awake()
    {
        var snails = GetComponentsInChildren<Transform>();
        //get all children for rotation and scaling animation
        foreach (var child in snails)
        {
            if (child.name.Contains("Sail"))
            {
                _sails.Add(child);
            }
            if (child.name.Contains("Mast"))
            {
                _masts.Add(child);
            }
        }
    }

    private void OnEnable()
    {

        _sailControlsAction.action.Enable();
    }

    private void OnDisable()
    {
        _sailControlsAction.action.Disable();
    }

    private void Update()
    {
        HandleInputs();
        UpdateSailsVisuals();
        ApplyPhysics();
        UpdateWindVisuals();
        _shipData.SailStatus = _sailStatus;
        _shipData.SailRotation = _sailRotation;
        _shipData.Velocity = Mathf.Abs(_currentVelocity.x) + Mathf.Abs(_currentVelocity.z);
    }

    private void HandleInputs()
    {
        var input = _sailControlsAction.action.ReadValue<Vector2>();

        _sailStatus += input.y * _sailStatusSpeed * Time.deltaTime;
        _sailStatus = Mathf.Clamp(_sailStatus, 0f, 100f);
        _sailRotation += input.x * _sailRotationSpeed * Time.deltaTime;
        _sailRotation = Mathf.Clamp(_sailRotation, -45f, 45f);
    }

    private void ApplyPhysics()
    { 
        Vector3 windDir = _windDirection.normalized;

        Quaternion sailRotation = Quaternion.Euler(0f, _sailRotation, 0f);
        Vector3 sailNormal = sailRotation * transform.forward;
        Vector3 sailRight = sailRotation * transform.right;

        float windDot = Vector3.Dot(windDir, sailNormal);
        float sailOpenRatio = _sailStatus / 100f;
        float caughtWindMagnitude = _windSpeed * Mathf.Abs(windDot) * sailOpenRatio;

        Vector3 sailForce = sailNormal * (caughtWindMagnitude * Mathf.Sign(windDot));
        float forwardForceMagnitude = Vector3.Dot(sailForce, transform.forward);
        Vector3 forwardForce = transform.forward * forwardForceMagnitude;

        Vector3 linearAcceleration = forwardForce / _mass;
        _currentVelocity += linearAcceleration * Time.deltaTime;
        _currentVelocity -= _currentVelocity * _waterDrag * Time.deltaTime;

        float windForwardDot = Vector3.Dot(sailNormal, windDir);
        float windRightDot = Vector3.Dot(sailRight, windDir);
        float rawTorque = 4f * windForwardDot * windRightDot * ((windForwardDot * windForwardDot) - (windRightDot * windRightDot));
        float torqueForce = rawTorque * _windTurningStrength * sailOpenRatio;
        float angularAcceleration = torqueForce / _mass;

        _currentAngularVelocity += angularAcceleration * Time.deltaTime;
        _currentAngularVelocity -= _currentAngularVelocity * _angularDrag * Time.deltaTime;
        
        transform.Rotate(0f, _currentAngularVelocity * Time.deltaTime, 0f);
        transform.position += _currentVelocity * Time.deltaTime;
    }

    private void UpdateSailsVisuals()
    {
        var targetRotation = Quaternion.Euler(0f, _sailRotation, 0f);
        var scaleY = Mathf.Max(0.01f, _sailStatus / 100f);
        var targetScale = new Vector3(1f, scaleY, 1f);

        foreach (var sail in _sails)
        {
            sail.localRotation = targetRotation;
            sail.localScale = targetScale;
        }
        foreach (var mast in _masts)
        {
            mast.localRotation = targetRotation;
        }
    }

    private void UpdateWindVisuals()
    {
        if (_windDirection != Vector3.zero)
        {
            _windParticles.transform.rotation = Quaternion.LookRotation(_windDirection);
            var particleMain = _windParticles.main;
            particleMain.startSpeed = _windSpeed * 1.5f;
        }
    }
}
