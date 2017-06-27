using UnityEngine;

public class AudioController 
{
    private float audioCooldown;
	public AudioController()
	{
        Player.Current.Jumped += OnPlayerJumped;
	}

    public void Update ()
	{
	    audioCooldown -= Time.deltaTime;
	}

    private void OnPlayerJumped(object sender, PlayerEventArgs args)
    {
        AudioClip audioClip = AudioManager.Get("Power_Jump");
        if (audioClip == null) return;
        PlayAudioClip(audioClip);
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        if (audioCooldown > 0) return;

        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        audioCooldown = 0.1f;
    }
}
