using UnityEngine;

namespace Project.Scripts
{
    
    
    
    public class SecurityCamera : MonoBehaviour
    {
        [Header("Oscillation")]
        [Tooltip("The range of oscillation in degrees around the base rotation.")]
        [SerializeField] private float oscillationRange = 45f;
        [Tooltip("How fast the camera oscillates.")]
        [SerializeField] private float oscillationSpeed = 1f;
        [Tooltip("Offset applied to the rotation. Use -90 if the sprite faces right but points up at 0 rotation.")]
        [SerializeField] private float rotationOffset = 0f;

        [Header("Detection Settings")]
        [Tooltip("Field of view of the camera in degrees.")]
        [SerializeField] private float fov = 60f;
        [Tooltip("How far the camera can see.")]
        [SerializeField] private float viewDistance = 10f;
        [Tooltip("How long the player must be seen to trigger an alert.")]
        [SerializeField] private float detectionTime = 2f;
        [Tooltip("Layers that block the camera's vision.")]
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private string playerTag = "Player";

        [Header("Alert Settings")]
        [Tooltip("The radius of the noise made when alerted.")]
        [SerializeField] private float alertRadius = 15f;

        [Header("Visual Colors")]
        [SerializeField] private Color normalColor = new Color(0, 1, 0, 0.4f);
        [SerializeField] private Color detectingColor = new Color(1, 0.5f, 0, 0.6f);
        [SerializeField] private Color alertColor = new Color(1, 0, 0, 0.8f);

        private float _baseRotation;
        private float _detectionTimer;
        private Transform _playerTransform;
        private VisionConeVisual _visionCone;
        private bool _isAlerted;

        private void Start()
        {
            _baseRotation = transform.eulerAngles.z;
            _visionCone = GetComponentInChildren<VisionConeVisual>();
            
            GameObject player = GameObject.FindWithTag(playerTag);
            if (player != null)
            {
                _playerTransform = player.transform;
            }

            
            if (_visionCone != null)
            {
                _visionCone.SetFov(fov);
                _visionCone.SetViewDistance(viewDistance);
                _visionCone.SetColor(normalColor);
            }
        }

        private void Update()
        {
            UpdateOscillation();
            CheckForPlayer();
            UpdateVisuals();
        }

        private void UpdateOscillation()
        {
            
            float angle = _baseRotation + Mathf.Sin(Time.time * oscillationSpeed) * oscillationRange;
            transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        }

        private void CheckForPlayer()
        {
            if (_playerTransform == null) return;

            if (IsPlayerInView())
            {
                _detectionTimer += Time.deltaTime;
                if (_detectionTimer >= detectionTime && !_isAlerted)
                {
                    TriggerAlert();
                }
            }
            else
            {
                
                
                _detectionTimer = Mathf.Max(0, _detectionTimer - Time.deltaTime);
                if (_detectionTimer <= 0)
                {
                    _isAlerted = false;
                }
            }
        }

        private bool IsPlayerInView()
        {
            Vector3 directionToPlayer = _playerTransform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            
            if (distanceToPlayer > viewDistance) return false;

            
            float angleToPlayer = Vector3.Angle(transform.right, directionToPlayer);
            if (angleToPlayer > fov / 2f) return false;

            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, obstacleLayer);
            if (hit.collider != null && !hit.collider.CompareTag(playerTag))
            {
                return false;
            }

            return true;
        }

        private void TriggerAlert()
        {
            _isAlerted = true;
            
            
            
            Vector2 alertLocation = _playerTransform != null ? (Vector2)_playerTransform.position : (Vector2)transform.position;
            NoiseManager.MakeNoise(alertLocation, alertRadius);
            
            Debug.Log($"Security Camera Alerted at {alertLocation}!");
        }

        private void UpdateVisuals()
        {
            if (_visionCone == null) return;

            
            _visionCone.SetFov(fov);
            _visionCone.SetViewDistance(viewDistance);

            
            if (_isAlerted)
            {
                _visionCone.SetColor(alertColor);
            }
            else if (_detectionTimer > 0)
            {
                
                float t = _detectionTimer / detectionTime;
                _visionCone.SetColor(Color.Lerp(detectingColor, alertColor, t));
            }
            else
            {
                _visionCone.SetColor(normalColor);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 forward = transform.right;
            Vector3 leftLimit = Quaternion.AngleAxis(-fov / 2f, Vector3.forward) * forward;
            Vector3 rightLimit = Quaternion.AngleAxis(fov / 2f, Vector3.forward) * forward;

            Gizmos.DrawLine(transform.position, transform.position + leftLimit * viewDistance);
            Gizmos.DrawLine(transform.position, transform.position + rightLimit * viewDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, alertRadius);
        }
    }
}
