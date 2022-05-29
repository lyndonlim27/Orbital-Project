using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _ac1, _ac2, _ac3, _ac4, _ac5;
    [SerializeField] private AudioClip[] audios;

    public void AudioClip1()
    {
        _audioSource.clip = _ac1;
        _audioSource.Play();

    }
    public void AudioClip2()
    {
        _audioSource.clip = _ac2;
        _audioSource.Play();

    }

    public void AudioClip3()
    {
        _audioSource.clip = _ac3;
        _audioSource.Play();

    }
    public void AudioClip4()
    {
        _audioSource.clip = _ac4;
        _audioSource.Play();

    }
    public void AudioClip5()
    {
        _audioSource.clip = _ac5;
        _audioSource.Play();

    }

}
