using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Thorny,
};

public class PlatformEffect : MonoBehaviour
{
    public PlatformType Type;
    public ParticleSystem Particles;
    public AudioClip[] Sounds;

    private bool active_ = true;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!active_)
        {
            if (!Particles.isPlaying && !_audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (collision.collider.CompareTag("Player"))
        {
            Apply(collision.gameObject.GetComponent<CharacterLogic>());
        }
    }

    private void Apply(CharacterLogic character)
    {
        switch (Type)
        {
            case PlatformType.Thorny:
            {
                if (character.IsInvlunerable)
                    return;

                active_ = false;
                character.TakeDamage(1);
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                Particles.Play();

                SoundUtil.PlayRandomSound(Sounds, _audioSource);

                break;
            }
            default:
                break;
        }
    }
}
