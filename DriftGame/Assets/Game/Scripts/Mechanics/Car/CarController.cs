using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class CarController : MonoBehaviour, ICarController
{
    [Header("CAR SETUP")]
    [Space(10)]
    public CarConfig carConfig;
    public Rigidbody rb;
    public PhotonView photonView;
    public CarWheelSync carWheelSync;

    [Header("WHEELS")]
    [Space(10)]
    public GameObject fLMesh;
    public WheelCollider fLCollider;
    [Space(10)]
    public GameObject fRMesh;
    public WheelCollider fRCollider;
    [Space(10)]
    public GameObject rLMesh;
    public WheelCollider rLCollider;
    [Space(10)]
    public GameObject rRMesh;
    public WheelCollider rRCollider;

    [Header("EFFECTS")]
    [Space(10)]
    public bool useEffects = false;
    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;

    [Space(10)]
    public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;

    [HideInInspector] public float speed { get; private set; }
    [HideInInspector] public float driftAmount { get; private set; }
    [HideInInspector] public bool isDrifting { get; private set; }

    private float driftStartThreshold = 2.5f; 
    private float driftForce = 0.05f;
    private float baseStiffness = 1f;
    
    private float flipCheckDelay = 2f;
    private float flipTimer = 0f;

    private float steerRecoverSpeed = 6f; 
    private float steerReduceSpeed = 3f; 
    private float currentMaxSteerAngle;

    private float steerInput;
    private float throttleInput;
    private float brakeInput;
    private float handbrakeInput;

    private void Start()
    {
        currentMaxSteerAngle = carConfig.maxSteerAngle;
    }
    
    public void Init(CarWheelSet MeshWheels)
    {
        fLMesh = MeshWheels.fLMesh;
        fRMesh = MeshWheels.fRMesh;
        rLMesh = MeshWheels.rLMesh;
        rRMesh = MeshWheels.rRMesh;

        carWheelSync.Init(MeshWheels);
    }


    private void Update()
    {
        if (!photonView.IsMine && photonView.Owner != null) return;

        GetInput();
        UpdateWheelMeshes();
        FlipTimer(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //if (!photonView.IsMine) return;
        
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        float localX = localVelocity.x;
        float localZ = localVelocity.z;

        driftAmount = Mathf.Abs(localX);
        speed = rb.velocity.magnitude;

        isDrifting = Mathf.Abs(localX) > driftStartThreshold && speed > 8f;

        //driftSync.SetDriftState(isDrifting, driftAmount);

        ApplySteering(isDrifting);
        ApplyMotor(localZ);
        ApplyBrakes();
        ApplyDrift(localX, speed, isDrifting);
    }
    
    private void GetInput()
    {
        steerInput = Input.GetAxis("Horizontal");
        throttleInput = Input.GetAxis("Vertical");
        brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
        handbrakeInput = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;
    }

    private void ApplySteering(bool isDrifting)
    {
        if (isDrifting)
        {
            currentMaxSteerAngle -= steerReduceSpeed * Time.deltaTime;
            currentMaxSteerAngle = Mathf.Clamp(currentMaxSteerAngle, 0f, carConfig.maxSteerAngle);
        }
        else
        {
            currentMaxSteerAngle += steerRecoverSpeed * Time.deltaTime;
            currentMaxSteerAngle = Mathf.Clamp(currentMaxSteerAngle, 0f, carConfig.maxSteerAngle);
        }

        float steer = steerInput * currentMaxSteerAngle;

        if (steerInput == 0)
            steer = 0;

        fLCollider.steerAngle = steer;
        fRCollider.steerAngle = steer;
    }

    private void ApplyMotor(float localZ)
    {
        float torque = throttleInput * carConfig.motorPower;

        if (localZ > carConfig.maxSpeed && throttleInput > 0)
            torque = 0;

        if (localZ < -carConfig.maxReverseSpeed && throttleInput < 0)
            torque = 0;

        rLCollider.motorTorque = torque;
        rRCollider.motorTorque = torque;

        if (throttleInput == 0)
            rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity * 0.98f, Time.fixedDeltaTime * carConfig.decelerationMultiplier);
    }
    
    private void ApplyBrakes()
    {
        float brakeTorque = brakeInput * carConfig.brakePower;

        fLCollider.brakeTorque = brakeTorque;
        fRCollider.brakeTorque = brakeTorque;

        if (handbrakeInput > 0)
        {
            rLCollider.brakeTorque = carConfig.brakePower * 2f;
            rRCollider.brakeTorque = carConfig.brakePower * 2f;
        }
        else
        {
            rLCollider.brakeTorque = 0;
            rRCollider.brakeTorque = 0;
        }
    }

    private void ApplyDrift(float localX, float speed, bool isDrifting)
    {
        if (isDrifting)
        {
            rb.AddForce(transform.right * localX * driftForce, ForceMode.Acceleration);

            float driftGrip = 1f - (carConfig.driftMultiplier * 0.1f);
            driftGrip = Mathf.Clamp(driftGrip, 0.985f, 1f);

            WheelFrictionCurve fric = rLCollider.sidewaysFriction;
            fric.stiffness = baseStiffness * driftGrip;
            rLCollider.sidewaysFriction = fric;
            rRCollider.sidewaysFriction = fric;
        }
        else
        {
            WheelFrictionCurve fric = rLCollider.sidewaysFriction;
            fric.stiffness = baseStiffness;
            rLCollider.sidewaysFriction = fric;
            rRCollider.sidewaysFriction = fric;
        }

        HandleEffects(localX, speed, isDrifting);
    }

    private void HandleEffects(float localX, float speed, bool isDrifting)
    {
        if (!useEffects) return;

        bool heavySlide = Mathf.Abs(localX) > 3f && speed > 10f;

        RLWTireSkid.emitting = heavySlide;
        RRWTireSkid.emitting = heavySlide;

        var e1 = RLWParticleSystem.emission;
        var e2 = RRWParticleSystem.emission;
        e1.enabled = heavySlide;
        e2.enabled = heavySlide;
    }

    private void UpdateWheelMeshes()
    {
        UpdateWheel(fLCollider, fLMesh);
        UpdateWheel(fRCollider, fRMesh);
        UpdateWheel(rLCollider, rLMesh);
        UpdateWheel(rRCollider, rRMesh);
    }

    private void UpdateWheel(WheelCollider collider, GameObject mesh)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        mesh.transform.SetPositionAndRotation(pos, rot);
    }

    private void FlipTimer(float deltaTime)
    {
        flipTimer += Time.deltaTime;

        if (flipTimer >= flipCheckDelay)
        {
            ResetCarIfFlipped();
            flipTimer = 0f;
        }
    }

    public void ResetCarIfFlipped()
    {
        if (rb == null) return;

        bool isFlipped = Vector3.Dot(transform.up, Vector3.up) < 0.4f;
        if (!isFlipped) return;

        Vector3 resetPos = transform.position + Vector3.up * 2f;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = resetPos;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    

}
