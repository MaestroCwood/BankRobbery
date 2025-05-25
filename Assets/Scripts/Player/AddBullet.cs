using UnityEngine;

public class AddBullet : MonoBehaviour
{
    public int ammoAmount = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ���� ��������� ������ � ������
            GunShooter gun = other.GetComponentInChildren<GunShooter>();
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                gun.UpdateUIWeapon();
                Destroy(gameObject); // ������� ������ ����� �������
            }
        }
    }
}
