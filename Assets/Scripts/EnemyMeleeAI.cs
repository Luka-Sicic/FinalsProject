using UnityEngine;
using Pathfinding;

namespace Project.Scripts
{
    [RequireComponent(typeof(AIPath))]
    [RequireComponent(typeof(AIDestinationSetter))]
    public class EnemyMeleeAI : MonoBehaviour, INoiseListener
    {
        [Header("Detection & Movement")]
        public float chaseRange = 10f;
        public float attackRange = 1.5f;
        public float fovAngle = 360f;
        public float detectionTime = 0.5f;
        public LayerMask obstacleLayer;
        public float rotationOffset = 0f;

        [Header("Attacking")]
        public float attackRate = 1.5f;
        public int damage = 1;
        public float attackRadius = 1f;
        public Transform attackPoint;
        public LayerMask targetLayers;
        public string attackTrigger = "Attack";

        [Header("Visuals")]
        public Sprite staticSprite;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip swingSound, hitSound;

        [Header("Patrol")]
        public Transform[] patrolPoints;
        public float patrolWaitTime = 1f;

        private Transform _player;
        private AIDestinationSetter _setter;
        private AIPath _aiPath;
        private float _attackTimer;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private float _moveResumeTimer;
        private int _currentPatrolIndex;
        private float _patrolWaitTimer;
        private Vector3 _lastSeenPosition;
        private bool _isSearching;
        private bool _isWaitingAtLastSeen;
        private float _searchTimer;
        private float _detectionTimer;
        private bool _isPlayerDetected;
        private PlayerController _playerController;

        public void OnHearNoise(Vector2 sourcePosition)
        {
            if (_setter != null && _setter.target == null)
            {
                _lastSeenPosition = sourcePosition;
                _isSearching = true;
                _isWaitingAtLastSeen = false;
                if (_aiPath != null)
                {
                    _aiPath.destination = sourcePosition;
                    _aiPath.canMove = true;
                }
            }
        }

        public void Stun(float duration)
        {
            _moveResumeTimer = duration;
            if (_aiPath != null) _aiPath.canMove = false;
        }

        void Start()
        {
            if (AstarPath.active == null || AstarPath.active.data.graphs.Length == 0)
            {
                Debug.LogWarning($"[AI] AstarPath graph missing. Disabling {gameObject.name} to prevent CPU spikes.");
                this.enabled = false;
                return;
            }
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (_player != null) _playerController = _player.GetComponent<PlayerController>();
            _setter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_spriteRenderer != null && staticSprite != null)
            {
                _spriteRenderer.sprite = staticSprite;
            }

            if (_aiPath != null)
            {
                _aiPath.enableRotation = false;
            }

            if (attackPoint == null)
            {
                attackPoint = transform;
            }
        }

        void Update()
        {

            if (_moveResumeTimer > 0) _moveResumeTimer -= Time.deltaTime;
            if (_attackTimer > 0) _attackTimer -= Time.deltaTime;

            float dist = _player != null ? Vector2.Distance(transform.position, _player.position) : float.MaxValue;
            bool canSee = _player != null && HasLineOfSight() && dist <= chaseRange;

            float effectiveDetectionTime = (_playerController != null && _playerController.IsStealth) ? detectionTime * 3f : detectionTime;
            if (canSee)
            {
                if (!_isPlayerDetected)
                {
                    _detectionTimer += Time.deltaTime;
                    if (_detectionTimer >= effectiveDetectionTime)
                    {
                        _isPlayerDetected = true;
                    }
                }
            }
            else
            {
                _detectionTimer = 0f;
                _isPlayerDetected = false;
            }

            if (_isPlayerDetected)
            {
                _lastSeenPosition = _player.position;
                _isSearching = false;
                _isWaitingAtLastSeen = false;
                if (_setter != null) _setter.target = _player;
                _patrolWaitTimer = 0f;

                if (dist <= attackRange)
                {

                    if (_aiPath != null) _aiPath.canMove = false;

                    if (_attackTimer <= 0)
                    {
                        Attack();
                        _attackTimer = attackRate;
                    }
                }
                else
                {

                    if (_moveResumeTimer <= 0 && _aiPath != null)
                    {
                        _aiPath.canMove = true;
                    }
                }
            }
            else
            {
                if (_setter != null && _setter.target != null)
                {

                    _setter.target = null;
                    _aiPath.destination = _lastSeenPosition;
                    _isSearching = true;
                }

                if (_isSearching)
                {
                    if (_moveResumeTimer <= 0 && _aiPath != null)
                    {
                        _aiPath.canMove = true;
                    }

                    if (_aiPath.reachedDestination && !_aiPath.pathPending)
                    {
                        _isSearching = false;
                        _isWaitingAtLastSeen = true;
                        _searchTimer = 3f;
                    }
                }
                else if (_isWaitingAtLastSeen)
                {
                    if (_aiPath != null) _aiPath.canMove = false;

                    _searchTimer -= Time.deltaTime;
                    if (_searchTimer <= 0)
                    {
                        _isWaitingAtLastSeen = false;
                    }
                }
                else
                {
                    if (_moveResumeTimer <= 0 && _aiPath != null)
                    {
                        _aiPath.canMove = true;
                    }
                    UpdatePatrol();
                }
            }

            UpdateVisuals();
            UpdateRotation();
        }

        private void UpdatePatrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0) return;

            if (_currentPatrolIndex < 0 || _currentPatrolIndex >= patrolPoints.Length) _currentPatrolIndex = 0;
            if (patrolPoints[_currentPatrolIndex] == null) return;

            if (_aiPath != null)
            {
                _aiPath.destination = patrolPoints[_currentPatrolIndex].position;

                if (_aiPath.reachedDestination && !_aiPath.pathPending)
                {
                    _patrolWaitTimer += Time.deltaTime;
                    if (_patrolWaitTimer >= patrolWaitTime)
                    {
                        _patrolWaitTimer = 0f;
                        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
                    }
                }
            }
        }

        bool HasLineOfSight()
        {
            Vector2 dirToPlayer = (_player.position - transform.position).normalized;
            float dist = Vector2.Distance(transform.position, _player.position);

            if (fovAngle < 360f)
            {
                float currentFacingAngle = (transform.eulerAngles.z - rotationOffset) * Mathf.Deg2Rad;
                Vector2 forward = new Vector2(Mathf.Cos(currentFacingAngle), Mathf.Sin(currentFacingAngle));
                if (Vector2.Angle(forward, dirToPlayer) > fovAngle * 0.5f) return false;
            }

            int mask = obstacleLayer | (1 << _player.gameObject.layer);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, dist, mask);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player")) return true;
                if (hit.collider.TryGetComponent<Door>(out var door) && door.isLocked) return false;
            }

            return false;
        }

        void Attack()
        {
            if (_animator != null)
            {
                _animator.SetTrigger(attackTrigger);
            }

            if (audioSource != null && swingSound != null)
            {
                audioSource.PlayOneShot(swingSound);
            }

            if (_aiPath != null)
            {
                _aiPath.canMove = false;
                _moveResumeTimer = 0.5f;
            }

            Vector3 point = attackPoint != null ? attackPoint.position : transform.position;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(point, attackRadius, targetLayers);

            bool hitAny = false;
            foreach (var hit in hitColliders)
            {
                if (hit.TryGetComponent<Health>(out var health))
                {
                    health.TakeDamage(damage);
                    hitAny = true;
                }
            }

            if (hitAny && audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }

        void UpdateVisuals()
        {
            if (_aiPath == null || _animator == null) return;
            _animator.SetBool("IsWalking", _aiPath.velocity.sqrMagnitude > 0.01f);
        }

        void UpdateRotation()
        {
            if (_aiPath == null) return;

            Vector2 direction = Vector2.zero;
            if (_setter.target != null)
            {
                direction = (_player.position - transform.position).normalized;
            }
            else if (_aiPath.velocity.sqrMagnitude > 0.1f)
            {
                direction = _aiPath.velocity.normalized;
            }

            if (direction != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float currentAngle = transform.eulerAngles.z;
                float nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle + rotationOffset, _aiPath.rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, nextAngle);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 point = attackPoint != null ? attackPoint.position : transform.position;
            Gizmos.DrawWireSphere(point, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}