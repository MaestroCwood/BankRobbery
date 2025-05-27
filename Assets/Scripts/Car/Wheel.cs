using UnityEngine;

public class Wheel : MonoBehaviour
{
    public bool steer;
    public bool inverSteer;
    public bool power;

    public float SteerAngle { get; set; }
    public float Torque { get; set; }

    WheelCollider wheelCollider;
    Transform wheelTransform;

    private void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
        wheelTransform = GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();

        SetupFriction();
    }

    private void Update()
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void FixedUpdate()
    {
        if (steer)
        {
            wheelCollider.steerAngle = SteerAngle * (inverSteer ? -1 : 1);
        }

        if(power)
        {
            wheelCollider.motorTorque = Torque;
        }
    }

    private void SetupFriction()
    {
        // Forward friction (вдоль движения)
        WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
        forwardFriction.extremumSlip = 0.4f;
        forwardFriction.extremumValue = 1f;
        forwardFriction.asymptoteSlip = 0.8f;
        forwardFriction.asymptoteValue = 0.5f;
        forwardFriction.stiffness = 2.0f;
        wheelCollider.forwardFriction = forwardFriction;

        // Sideways friction (боковое скольжение)
        WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
        sidewaysFriction.extremumSlip = 0.2f;
        sidewaysFriction.extremumValue = 1f;
        sidewaysFriction.asymptoteSlip = 0.5f;
        sidewaysFriction.asymptoteValue = 0.75f;
        sidewaysFriction.stiffness = 2.5f;
        wheelCollider.sidewaysFriction = sidewaysFriction;
    }
}
