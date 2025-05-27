using UnityEngine;

public class MobileInputCar : MonoBehaviour
{
    public float inputValue;



    public void UpValue(float value)
    {
        inputValue = value;
    }
    public void DownValue(float value)
    {
        inputValue = value;
    }
    public void ZeroValue()
    {
        inputValue = 0;
    }

   
}
