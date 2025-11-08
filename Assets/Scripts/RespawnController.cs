using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public WheelCollider frontRight;
    public WheelCollider frontLeft;
    public WheelCollider backRight;
    public WheelCollider backLeft;

    public int ticksBeforeRespawn = 100; // nombre de FixedUpdate avant respawn
    public float respawnHeight = 2f;     // hauteur du TP
    public float uprightResetSpeed = 5f; // vitesse de correction rotation

    private int notGroundedTicks = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        int groundedWheels = 0;

        if (frontLeft.isGrounded) groundedWheels++;
        if (frontRight.isGrounded) groundedWheels++;
        if (backLeft.isGrounded) groundedWheels++;
        if (backRight.isGrounded) groundedWheels++;

        // Si moins de 2 roues touchent le sol → compte
        if (groundedWheels < 2)
        {
            notGroundedTicks++;
            if (notGroundedTicks >= ticksBeforeRespawn)
            {
                RespawnCar();
                notGroundedTicks = 0;
            }
        }
        else
        {
            // si la voiture revient normale, reset le compteur
            notGroundedTicks = 0;
        }
    }

    void RespawnCar()
    {
        // freeze physique un petit moment pour éviter l’éjection
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // remonter la voiture
        Vector3 pos = transform.position;
        pos.y += respawnHeight;
        transform.position = pos;

        // reset rotation droite
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
