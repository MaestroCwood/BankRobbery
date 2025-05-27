using UnityEngine;
using UnityEngine.UI;

public class SetingsSwipe : MonoBehaviour
{
    [SerializeField] Slider sliderSens;
    [SerializeField] MouseSwipeCamera swipeCamera;
    

    private void Awake()
    {
        
       sliderSens.value = swipeCamera.sensitivity;
     
    }

    private void OnEnable()
    {
        sliderSens.onValueChanged.AddListener(SetSensitivity);
       // swipeCamera.swipeInputUI.gameObject.SetActive(false);
        
    }

    private void OnDisable()
    {
        sliderSens.onValueChanged.RemoveListener(SetSensitivity);
        PlayerPrefs.SetFloat("sensitivity", swipeCamera.sensitivity);
       // swipeCamera.swipeInputUI.gameObject.SetActive(true);
    }

    void SetSensitivity(float value)
    {
        swipeCamera.sensitivity = Mathf.Clamp(value, sliderSens.minValue, sliderSens.maxValue);
    }
}
