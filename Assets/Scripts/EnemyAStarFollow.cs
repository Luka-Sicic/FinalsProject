using UnityEngine;
using Pathfinding;

namespace Project.Scripts
{
    [RequireComponent(typeof(AIPath))]
    [RequireComponent(typeof(AIDestinationSetter))]
    public class EnemyAStarFollow : MonoBehaviour, INoiseListener
{
        [Header("Detection")]
        public float chaseRange = 5f;
        public float fovAngle = 180f;
        public LayerMask obstacleLayer;

        [Header("Visuals")]
        [Tooltip("Offset added to rotation. If the sprite points its 'side' at the player, adjust this (usually 90 or -90).")]
        public float rotationOffset = -90f;

        [Header("Patrol")]
        public Transform[] patrolPoints;
        public float patrolWaitTime = 1f;

        private Transform _player;
        private AIDestinationSetter _setter;
        private AIPath _aiPath;
        private SpriteRenderer _sprite;
        private Animator _animator;
        private float _stunTimer;
        private int _currentPatrolIndex;
        private float _patrolWaitTimer;

        private Vector3 _lastSeenPosition;
        private bool _isSearching;
        private bool _isWaitingAtLastSeen;
        private float _searchTimer;

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

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            _setter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
            _sprite = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            if (_aiPath != null)
            {

                _aiPath.enableRotation = false;
            }

        }

        void Update()
        {
            if (_stunTimer > 0)
            {
                _stunTimer -= Time.deltaTime;
                if (_aiPath != null) _aiPath.canMove = false;
                UpdateVisuals();
                return;
            }

            if (_aiPath != null) _aiPath.canMove = !_isWaitingAtLastSeen;

            float dist = _player != null ? Vector2.Distance(transform.position, _player.position) : float.MaxValue;
            bool canSee = _player != null && dist <= chaseRange;

            if (canSee)
            {
                Vector2 dirToPlayer = ((Vector2)_player.position - (Vector2)transform.position).normalized;

                float currentFacingAngle = (transform.eulerAngles.z - rotationOffset) * Mathf.Deg2Rad;
                Vector2 forward = new Vector2(Mathf.Cos(currentFacingAngle), Mathf.Sin(currentFacingAngle));

                if (Vector2.Angle(forward, dirToPlayer) > fovAngle * 0.5f)
                {
                    canSee = false;
                }
                else if (!HasLineOfSight(dirToPlayer, dist))
                {
                    canSee = false;
                }
}

            if (canSee)
            {
                _lastSeenPosition = _player.position;
                _isSearching = false;
                _isWaitingAtLastSeen = false;
                if (_setter != null) _setter.target = _player;
                _patrolWaitTimer = 0f;
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
                    if (_aiPath.reachedDestination && !_aiPath.pathPending)
                    {
                        _isSearching = false;
                        _isWaitingAtLastSeen = true;
                        _searchTimer = 3f;
                    }
                }
                else if (_isWaitingAtLastSeen)
                {
                    _searchTimer -= Time.deltaTime;
                    if (_searchTimer <= 0)
                    {
                        _isWaitingAtLastSeen = false;
                    }
                }
                else
                {
                    UpdatePatrol();
                }
            }

            UpdateVisuals();
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

        void UpdateVisuals()
        {
            if (_aiPath == null) return;

            if (_animator != null)
            {
                _animator.SetBool("IsWalking", _aiPath.velocity.sqrMagnitude > 0.01f);
            }

            if (_aiPath.velocity.sqrMagnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(_aiPath.velocity.y, _aiPath.velocity.x) * Mathf.Rad2Deg;
                float currentAngle = transform.eulerAngles.z;
                float nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle + rotationOffset, _aiPath.rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, nextAngle);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            float currentFacingAngle = (transform.eulerAngles.z - rotationOffset) * Mathf.Deg2Rad;
            Vector3 forward = new Vector3(Mathf.Cos(currentFacingAngle), Mathf.Sin(currentFacingAngle), 0);
            Vector3 leftRay = Quaternion.Euler(0, 0, fovAngle * 0.5f) * forward;
            Vector3 rightRay = Quaternion.Euler(0, 0, -fovAngle * 0.5f) * forward;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, leftRay * chaseRange);
            Gizmos.DrawRay(transform.position, rightRay * chaseRange);
        }

        public void Stun(float duration)
        {
            _stunTimer = duration;
            if (_aiPath != null) _aiPath.canMove = false;
        }

        private bool HasLineOfSight(Vector2 dirToPlayer, float dist)
        {

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dirToPlayer, dist);

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == gameObject) continue;
                if (hit.collider.CompareTag("Player")) return true;

                if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0) return false;

                if (hit.collider.TryGetComponent<Door>(out var door) && door.isLocked) return false;
            }

            return false;
        }
    }
}