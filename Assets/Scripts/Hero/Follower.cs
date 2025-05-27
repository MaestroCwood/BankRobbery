using UnityEngine;

public abstract class Follower : MonoBehaviour
{
    [SerializeField] Transform targetFollow = null;
    [SerializeField] float smooth, offsetZ, offsetY;

    Vector3 offset;

    private void Start()
    {
        OffsetCam();
    }


    protected void FollowMove(float delta)
    {
        // ������� �������� ������ � ������� � ������ ��������
        Vector3 desiredPosition = targetFollow.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, delta * smooth);

        // ������� ������� ������ � ����
        Quaternion targetRotation = Quaternion.LookRotation(targetFollow.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, delta * smooth);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            OffsetCam();
        }
    }

    public void OffsetCam()
    {
        // �������� ������ ����� � ������� ����� ������������ ����
        offset = -targetFollow.forward * offsetZ + Vector3.up * offsetY;

        // ������������� ������� ������
        transform.position = targetFollow.position + offset;

        // ������������ ������ �� ����
        transform.LookAt(targetFollow);
    }
}
