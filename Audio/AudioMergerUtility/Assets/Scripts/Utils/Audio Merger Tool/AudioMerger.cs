using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Audio_Merger_Tool
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioMerger : MonoBehaviour
    {
    
        [SerializeField] private AudioClip soundToMerge;
        [SerializeField] private Button mergingButton;
        [SerializeField] private float fadeTimeSeconds = 1;
        [SerializeField] private bool isUsingButton = false;

        private AudioSource _audioSource;
        private AudioSource _audioSourceToMerge;
        private TextMeshProUGUI _buttonLabel;

        private readonly WaitForSeconds _waitingTime = new WaitForSeconds(.1f);
        private bool _fading = false;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if(!isUsingButton) return;
            mergingButton.onClick.AddListener(MergeAudio);
            _buttonLabel = mergingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void MergeAudio()
        {
            if(_fading) return;
        
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
            _fading = true;

            if (isUsingButton)
                mergingButton.interactable = false;
        
            var timeElapsed = 0f;

            while (_audioSourceToMerge.volume > 0) 
            {
                _audioSourceToMerge.volume = Mathf.Lerp(1, 0, timeElapsed / fadeTimeSeconds);
                timeElapsed += Time.deltaTime;
                yield return _waitingTime;
            }
        
            Destroy(_audioSourceToMerge.GameObject());
            _fading = false;
        
            if (!isUsingButton) yield break;
        
            _buttonLabel.text = "Merge";
            mergingButton.interactable = true;
        
            yield break;
        }

        private IEnumerator FadeIn() 
        {
            _fading = true;
        
            if (isUsingButton)
                mergingButton.interactable = false;
        
            var timeElapsed = 0f;

            while (_audioSourceToMerge.volume < 1) 
            {
                _audioSourceToMerge.volume = Mathf.Lerp(0, 1, timeElapsed / fadeTimeSeconds);
                timeElapsed += Time.deltaTime;
                yield return _waitingTime;
            }
        
            _fading = false;
        
            if(!isUsingButton) yield break;
        
            _buttonLabel.text = "Stop Merge";
            mergingButton.interactable = true;
        
            yield break;
        }
    }
}
