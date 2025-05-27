using UnityEngine;
using YG;
public class SedaunCar : MonoBehaviour
{
    [SerializeField] InputHeroControll heroControl;
    [SerializeField] GameObject player;
    [SerializeField] Car car;
    [SerializeField] MouseSwipeCamera swipeCamera;
    [SerializeField] FixedFollower fixedFollower;
    [SerializeField] Transform exitTargetPos;
    [SerializeField] GameObject uiCanvasIsMobile;
    [SerializeField] GameObject uiCar;
    [SerializeField] GameObject exitCarBtn;
    [SerializeField] GameObject triggerZonaCar;




    bool isMobile;


    private void Awake()
    {
        isMobile = YG2.envir.isMobile;
    }
    public void SedaunInsideCar()
    {   
        
        heroControl.inputPlayer.Disable();
        
        triggerZonaCar.SetActive(false);
        if (isMobile)
        {
            uiCanvasIsMobile.SetActive(false);
        }
       
        swipeCamera.enabled = false;
        player.SetActive(false);
        car.inputPlayer.Enable();
        uiCar.SetActive(isMobile);
        fixedFollower.enabled = true;
        car.enabled = true;
        exitCarBtn.SetActive(true);
    }

    public void ExitCar ()
    {   
        heroControl.transform.position = exitTargetPos.transform.position;
        heroControl.inputPlayer.Enable();
        heroControl.ResetInput();
        triggerZonaCar.SetActive(true);
        if(isMobile)
        {
            uiCanvasIsMobile.SetActive(true);
        }
        
        swipeCamera.enabled = true;
        player.SetActive(true);
        car.inputPlayer.Disable();
        uiCar.SetActive(isMobile);
        fixedFollower.enabled = false;
        car.enabled = false;
        exitCarBtn.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SedaunInsideCar();
        } 
    }
}
