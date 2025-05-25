using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceSfx;
    [SerializeField] AudioClip[] audioClipSfx;




    public void PlaySounFx(int name)
    {
        audioSourceSfx.PlayOneShot(audioClipSfx[name]);
    }
}
