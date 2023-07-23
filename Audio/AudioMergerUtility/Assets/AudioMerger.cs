using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioMerger : MonoBehaviour
{ 
    [SerializeField] private AudioClip soundToMerge;
    [SerializeField] private Button mergingButton;

    private AudioSource _audioSource;
    private AudioSource _audioSourceToMerge;
    private TextMeshProUGUI _buttonLabel;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        mergingButton.onClick.AddListener(MergeAudio);
        _buttonLabel = mergingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void MergeAudio()
    {
        if (_audioSourceToMerge == null)
        {
            _audioSourceToMerge = new GameObject().AddComponent<AudioSource>();
            _audioSourceToMerge.clip = soundToMerge;
            _audioSourceToMerge.loop = true;
            
            _audioSourceToMerge.Play();
            _audioSourceToMerge.time = _audioSource.time;

            _buttonLabel.text = "Stop Merge";
        }
        else
        {
            Destroy(_audioSourceToMerge.GameObject());
            _buttonLabel.text = "Merge";
        }
    }
}
