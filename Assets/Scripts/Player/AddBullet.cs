using UnityEngine;

public class AddBullet : MonoBehaviour
{
    public int ammoAmount = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ищем компонент оружия у игрока
            GunShooter gun = other.GetComponentInChildren<GunShooter>();
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                gun.UpdateUIWeapon();
                Destroy(gameObject); // Удаляем патрон после подбора
            }
        }
    }
}
