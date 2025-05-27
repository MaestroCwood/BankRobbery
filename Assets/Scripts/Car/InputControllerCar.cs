using UnityEngine;

public class InputControllerCar : MonoBehaviour
{
    public string inputSreerAxis = "Horizontal";
    public string inputThorletAxis = "Vertical";

    public float ThrottelInput { get; private set; }
    public float SteerInput { get; private set; }



    private void Update()
    {
        SteerInput = Input.GetAxis(inputSreerAxis);
        ThrottelInput = Input.GetAxis(inputThorletAxis);

    }
}
