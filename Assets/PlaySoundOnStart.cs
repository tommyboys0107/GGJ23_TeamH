using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnSet : MonoBehaviour
{
    [SerializeField] private AudioClip _Clip;
    void Start()
    {
        SoundManager.Instance.PlaySound(_Clip);
    }
}
