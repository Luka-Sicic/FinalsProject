using UnityEngine;
using Pathfinding;

namespace MyGame.Audio
{
    public class MusicManager : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioClip _combatClip;
        [SerializeField] private AudioClip[] _stealthClips;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _maxPauseDuration = 60f;
        [SerializeField] private float _combatExitDelay = 10f;

        private Transform _playerTransform;
        private int _currentStealthIndex = 0;
        private bool _inCombat = false;
        private bool _returningToStealth = false;
        
        private float _combatCheckInterval = 0.5f;
        private float _combatCheckTimer = 0f;
        private float _combatExitTimer = 0f;
        private bool _cachedAnyFollowing = false;

        private float _pauseTimer = 0f;
        private bool _isPausing = false;

        private void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
                if (_audioSource == null)
                {
                    _audioSource = gameObject.AddComponent<AudioSource>();
                }
            }
            
            
            _audioSource.spatialBlend = 0f;
            _audioSource.playOnAwake = false;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("MusicManager: Player tag 'Player' not found in scene.");
            }
        }

        private void Start()
        {
            if (_stealthClips != null && _stealthClips.Length > 0)
            {
                
                
                
                
                PlayNextStealth();
            }
            else
            {
                Debug.LogWarning("MusicManager: No stealth clips assigned.");
            }
        }

        private void Update()
        {
            UpdateCombatDetection();

            if (_inCombat)
            {
                
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
                return;
            }

            
            if (!_audioSource.isPlaying)
            {
                if (_returningToStealth)
                {
                    _returningToStealth = false;
                    StartPause();
                }
                else if (!_isPausing)
                {
                    
                    StartPause();
                }
            }

            if (_isPausing)
            {
                _pauseTimer -= Time.deltaTime;
                if (_pauseTimer <= 0)
                {
                    _isPausing = false;
                    PlayNextStealth();
                }
            }
        }

        private void UpdateCombatDetection()
        {
            _combatCheckTimer -= Time.deltaTime;
            if (_combatCheckTimer <= 0f)
            {
                _combatCheckTimer = _combatCheckInterval;
                _cachedAnyFollowing = CheckIfFollowing();
            }

            if (_cachedAnyFollowing)
            {
                _combatExitTimer = _combatExitDelay;
                if (!_inCombat)
                {
                    StartCombat();
                }
            }
            else if (_inCombat)
            {
                _combatExitTimer -= Time.deltaTime;
                if (_combatExitTimer <= 0f)
                {
                    EndCombat();
                }
            }
        }

        private bool CheckIfFollowing()
        {
            if (_playerTransform == null) return false;

            
            AIDestinationSetter[] setters = Object.FindObjectsByType<AIDestinationSetter>(FindObjectsInactive.Exclude);
            foreach (var setter in setters)
            {
                if (setter != null && setter.target == _playerTransform)
                {
                    return true;
                }
            }
            return false;
        }

        private void StartCombat()
        {
            _inCombat = true;
            _returningToStealth = false;
            _isPausing = false; 
            
            
            
            if (_audioSource.clip == _combatClip && _audioSource.isPlaying)
            {
                _audioSource.loop = true;
            }
            else
            {
                _audioSource.clip = _combatClip;
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }

        private void EndCombat()
        {
            _inCombat = false;
            _returningToStealth = true;
            
            
            _audioSource.loop = false;
        }

        private void StartPause()
        {
            _isPausing = true;
            _pauseTimer = Random.Range(0f, _maxPauseDuration);
        }

        private void PlayNextStealth()
        {
            if (_stealthClips == null || _stealthClips.Length == 0) return;

            _audioSource.clip = _stealthClips[_currentStealthIndex];
            _audioSource.loop = false;
            _audioSource.Play();

            _currentStealthIndex = (_currentStealthIndex + 1) % _stealthClips.Length;
        }

        
        public void SetClips(AudioClip combat, AudioClip[] stealth)
        {
            _combatClip = combat;
            _stealthClips = stealth;
        }
    }
}
