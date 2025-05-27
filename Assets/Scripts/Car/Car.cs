using SimpleInputNamespace;
using UnityEngine;
using YG;
public class Car : MonoBehaviour
{
    public InputSystem_Actions inputPlayer;
    public MobileInputCar inputCar;
    public SteeringWheel wheelSteerUI;
    public Transform centerOfMass;
    public float motorTorque = 1500f;
    public float maxSteer = 20f;
    public bool isCarInsidePLayer;

    public float Steer { get; set; }
    public float Throttle { get; set; }

    bool isMobile;
    Rigidbody rb;
    Wheel[] wheels;
    private void Awake()
    {
        isMobile = YG2.envir.isMobile;
        inputPlayer = new InputSystem_Actions();
        inputPlayer.Enable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        wheels = GetComponentsInChildren<Wheel>();
    }

    private void Update()
    {   
        Steer = isMobile ? wheelSteerUI.Value : inputPlayer.Player.Move.ReadValue<Vector2>().x;
        Throttle = isMobile ? inputCar.inputValue : inputPlayer.Player.Move.ReadValue<Vector2>().y;
        //Steer = GameManager.instance.inputControllerCar.SteerInput;
       // Throttle = GameManager.instance.inputControllerCar.ThrottelInput;
        foreach (var wheel in wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.Torque = Throttle * motorTorque;
        }
    }
}
