using UnityEngine;
using YG;
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public InputControllerCar inputControllerCar { get; private set; }

    [SerializeField] GameObject mobileCanvas;

    bool isMobile;
    private void Awake()
    {
        instance = this;
        inputControllerCar = GetComponent<InputControllerCar>();
        isMobile = YG2.envir.isMobile;
    }

    private void Start()
    {
        mobileCanvas.SetActive(isMobile);
    }

}
