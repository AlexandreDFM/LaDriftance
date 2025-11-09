using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarDriftController : MonoBehaviour
{
    [Header("Wheels (colliders + visuals)")]
    public WheelCollider frontRight;

    public WheelCollider frontLeft;
    public WheelCollider backRight;
    public WheelCollider backLeft;

    public Transform frontRightTransform;
    public Transform frontLeftTransform;
    public Transform backRightTransform;
    public Transform backLeftTransform;

    [Header("Engine / Steering")]
    // Increased defaults for a much faster car per user request
    // Increased defaults for a much faster car per user request
    public float maxMotorTorque = 200000f; // Arcade-boosted power (raised)

    public float maxBrakeTorque = 500f;
    public float maxSteerAngle = 40f;
    public float speedMultiplier = 5.0f;

    [Header("Arcade Drive (optional)")]
    [Tooltip(
        "If enabled, apply an additional Rigidbody force based on motor torque to guarantee acceleration (useful when WheelColliders slip or are misconfigured).")]
    public bool useArcadeDrive = true;

    [Tooltip(
        "Multiplier that converts motor torque into a linear force applied to the Rigidbody. Increase to accelerate faster.")]
    public float motorToForce = 0.0005f;

    [Header("Speed Tuning")] public float maxSpeed = 200f; // ~720 km/h — increased to allow higher top speeds

    public float rearSidewaysFriction = 0.7f; // lowers rear grip for natural drift
    public float rearForwardFriction = 0.9f;

    [Header("Input (optional - Input System)")]
    public InputActionReference moveAction;

    public InputActionReference brakeAction;
    public InputActionReference hornAction;

    [Header("Audio")] public AudioSource engineAudioSource;

    [Tooltip("Vitesse pour passer de longnion à nion")]
    public float speedThresholdNion = 30f;

    [Tooltip("Vitesse pour passer de nion à ptinion")]
    public float speedThresholdPtinion = 60f;

    [Header("Visuals")] public float wheelRotationSmooth = 30f;

    [Header("Stopping / Decay")]
    [Tooltip("If true the car will stop instantly when no input; if false the car will decay to a stop over time.")]
    public bool useInstantStop;

    [Tooltip("Higher = faster decay to zero. Typical values: 2-10.")]
    public float velocityDecayRate = 5f;

    [Tooltip("Angular velocity decay rate. Higher = faster rotation damping.")]
    public float angularDecayRate = 5f;

    private bool _warnedBrake;
    private bool _warnedMove;

    // Audio tracking
    private string currentEngineSound = "";

    // debug logging timer to avoid spamming every FixedUpdate
    private readonly float debugLogInterval = 0.5f;
    private float debugLogTimer;
    private WheelFrictionCurve frBackLeftSide, frBackRightSide, frBackLeftForward, frBackRightForward;
    private bool isHornPlaying;

    private Rigidbody rb;

    private void Awake()
    {
        HandleHornInput();
        rb = GetComponent<Rigidbody>();
        CacheRearFriction();
        ApplyRearFriction();

        // Setup audio source for engine sounds
        if (engineAudioSource == null)
        {
            engineAudioSource = gameObject.AddComponent<AudioSource>();
            engineAudioSource.loop = true;
            engineAudioSource.playOnAwake = false;
            engineAudioSource.spatialBlend = 1f; // 3D sound
        }
    }

    private void FixedUpdate()
    {
        // --- INPUT ---
        var move = Vector2.zero;
        float brakeVal = 0f;

        if (moveAction && moveAction.action != null)
        {
            move = moveAction.action.ReadValue<Vector2>();
        }
        else
        {
            if (!_warnedMove)
            {
                Debug.LogWarning("CarDriftController: moveAction not assigned (Input System). Using old Input.");
                _warnedMove = true;
            }

            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");
        }

        if (brakeAction && brakeAction.action != null)
        {
            brakeVal = brakeAction.action.ReadValue<float>();
        }
        else
        {
            if (!_warnedBrake)
            {
                Debug.LogWarning(
                    "CarDriftController: brakeAction not assigned (Input System). Using Space key for brake.");
                _warnedBrake = true;
            }

            brakeVal = Input.GetKey(KeyCode.Space) ? 1f : 0f;
        }

        float steer = Mathf.Clamp(move.x, -1f, 1f);
        float throttle = Mathf.Clamp(move.y, -1f, 1f);

        float currentSpeed = rb.linearVelocity.magnitude;

        // If the player hasn't pressed throttle and isn't braking, either reset speed or decay to zero
        if (Mathf.Abs(throttle) < 0.01f && brakeVal <= 0.01f)
        {
            if (useInstantStop)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                // Smoothly decay velocity and angular velocity (frame-rate independent)
                rb.linearVelocity =
                    Vector3.Lerp(rb.linearVelocity, Vector3.zero, velocityDecayRate * Time.fixedDeltaTime);
                rb.angularVelocity =
                    Vector3.Lerp(rb.angularVelocity, Vector3.zero, angularDecayRate * Time.fixedDeltaTime);
            }
        }

        // --- Motor ---
        float motor = maxMotorTorque * throttle * speedMultiplier;
        ApplyMotorTorque(motor);

        // Optional arcade-style force to ensure the vehicle accelerates even if WheelColliders spin.
        if (useArcadeDrive && rb)
            // Apply forward force proportional to motor torque. ForceMode.Force integrates with mass.
            rb.AddForce(transform.forward * (motor * motorToForce), ForceMode.Force);

        // --- Debug logging (periodic) ---
        debugLogTimer += Time.fixedDeltaTime;
        if (debugLogTimer >= debugLogInterval)
        {
            debugLogTimer = 0f;
            float leftRpm = backLeft != null ? backLeft.rpm : 0f;
            float rightRpm = backRight != null ? backRight.rpm : 0f;
        }

        // --- Brakes ---
        ApplyBrakes(maxBrakeTorque * brakeVal);

        // --- Steering ---
        float steerAngle = maxSteerAngle * steer;
        if (frontLeft) frontLeft.steerAngle = steerAngle;
        if (frontRight) frontRight.steerAngle = steerAngle;

        // --- Wheel visuals ---
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);

        // --- Engine Sound Management ---
        UpdateEngineSound(currentSpeed);

        // --- Speed cap ---
        if (currentSpeed > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        return;

        void UpdateEngineSound(float currentSpeed)
        {
            if (!AudioManager.Instance || engineAudioSource == null)
                return;

            string targetSound = "";

            // Déterminer quel son jouer en fonction de la vitesse
            if (currentSpeed < speedThresholdNion)
                targetSound = "longnion"; // Vitesse lente
            else if (currentSpeed < speedThresholdPtinion)
                targetSound = "nion"; // Vitesse moyenne
            else
                targetSound = "ptinion"; // Vitesse rapide

            // Ne changer le son que si c'est différent du son actuel
            if (currentEngineSound == targetSound) return;
            currentEngineSound = targetSound;
            AudioManager.Instance.PlaySoundFXFromHandler(targetSound, engineAudioSource);
        }
    }

    private void OnEnable()
    {
        if (moveAction != null && moveAction.action != null) moveAction.action.Enable();
        if (brakeAction != null && brakeAction.action != null) brakeAction.action.Enable();
        if (hornAction != null && hornAction.action != null) hornAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null && moveAction.action != null) moveAction.action.Disable();
        if (brakeAction != null && brakeAction.action != null) brakeAction.action.Disable();
        if (hornAction != null && hornAction.action != null) hornAction.action.Disable();
    }

    private void OnValidate()
    {
        maxSpeed = Mathf.Max(0f, maxSpeed);
        maxMotorTorque = Mathf.Max(1f, maxMotorTorque);
        rearSidewaysFriction = Mathf.Clamp01(rearSidewaysFriction);
        rearForwardFriction = Mathf.Clamp01(rearForwardFriction);
    }

    private void CacheRearFriction()
    {
        if (backLeft != null)
        {
            frBackLeftSide = backLeft.sidewaysFriction;
            frBackLeftForward = backLeft.forwardFriction;
        }

        if (backRight != null)
        {
            frBackRightSide = backRight.sidewaysFriction;
            frBackRightForward = backRight.forwardFriction;
        }
    }

    private void ApplyRearFriction()
    {
        if (backLeft != null)
        {
            var s = backLeft.sidewaysFriction;
            var f = backLeft.forwardFriction;
            s.stiffness = rearSidewaysFriction;
            f.stiffness = rearForwardFriction;
            backLeft.sidewaysFriction = s;
            backLeft.forwardFriction = f;
        }

        if (backRight != null)
        {
            var s = backRight.sidewaysFriction;
            var f = backRight.forwardFriction;
            s.stiffness = rearSidewaysFriction;
            f.stiffness = rearForwardFriction;
            backRight.sidewaysFriction = s;
            backRight.forwardFriction = f;
        }
    }

    private void ApplyMotorTorque(float torque)
    {
        if (backLeft) backLeft.motorTorque = torque;
        if (backRight) backRight.motorTorque = torque;
    }

    private void ApplyBrakes(float brake)
    {
        if (frontLeft) frontLeft.brakeTorque = brake;
        if (frontRight) frontRight.brakeTorque = brake;
        if (backLeft) backLeft.brakeTorque = brake;
        if (backRight) backRight.brakeTorque = brake;
    }

    private void UpdateWheel(WheelCollider col, Transform trans)
    {
        if (!col || !trans) return;

        col.GetWorldPose(out var pos, out var rot);
        trans.position = pos;
        trans.rotation = Quaternion.RotateTowards(trans.rotation, rot, wheelRotationSmooth * Time.fixedDeltaTime);
    }

    private void HandleHornInput()
    {
        if (AudioManager.Instance == null || engineAudioSource == null)
            return;

        bool hornPressed = false;

        // Vérifier si le klaxon est pressé
        if (hornAction != null && hornAction.action != null) hornPressed = hornAction.action.IsPressed();

        // Jouer ou arrêter le son du klaxon
        if (hornPressed && !isHornPlaying)
        {
            // Créer un AudioSource temporaire pour le klaxon pour ne pas interrompre le son du moteur
            var hornObject = new GameObject("HornSound");
            hornObject.transform.SetParent(transform);
            hornObject.transform.localPosition = Vector3.zero;
            var hornSource = hornObject.AddComponent<AudioSource>();
            hornSource.spatialBlend = 1f; // 3D sound

            AudioManager.Instance.PlaySoundFXFromHandler("uiuiui", hornSource);
            isHornPlaying = true;

            // Détruire l'objet après la durée du son (ajuster selon la longueur du son)
            Destroy(hornObject, 2f);
        }
        else if (!hornPressed && isHornPlaying)
        {
            isHornPlaying = false;
        }
    }
}