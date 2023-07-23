using System.Collections;
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
    [SerializeField] private float fadeTimeSeconds = 1;

    private AudioSource _audioSource;
    private AudioSource _audioSourceToMerge;
    private TextMeshProUGUI _buttonLabel;

    private WaitForSeconds _waitingTime = new WaitForSeconds(.1f);
    private bool fading = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        mergingButton.onClick.AddListener(MergeAudio);
        _buttonLabel = mergingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void MergeAudio()
    {
        if(fading) return;
        
        if (_audioSourceToMerge == null)
        {
            _audioSourceToMerge = new GameObject().AddComponent<AudioSource>();
            _audioSourceToMerge.clip = soundToMerge;
            _audioSourceToMerge.loop = true;
            
            _audioSourceToMerge.Play();
            _audioSourceToMerge.volume = 0;
            _audioSourceToMerge.time = _audioSource.time;

            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        fading = true;
        mergingButton.interactable = false;
        var timeElapsed = 0f;

        while (_audioSourceToMerge.volume > 0) 
        {
            _audioSourceToMerge.volume = Mathf.Lerp(1, 0, timeElapsed / fadeTimeSeconds);
            timeElapsed += Time.deltaTime;
            yield return _waitingTime;
        }
        
        Destroy(_audioSourceToMerge.GameObject());
        _buttonLabel.text = "Merge";
        fading = false;
        mergingButton.interactable = true;
        yield break;
    }

    private IEnumerator FadeIn() 
    {
        fading = true;
        mergingButton.interactable = false;
        var timeElapsed = 0f;

        while (_audioSourceToMerge.volume < 1) 
        {
            _audioSourceToMerge.volume = Mathf.Lerp(0, 1, timeElapsed / fadeTimeSeconds);
            timeElapsed += Time.deltaTime;
            yield return _waitingTime;
        }
        
        _buttonLabel.text = "Stop Merge";
        mergingButton.interactable = true;
        fading = false;
        yield break;
    }
}
