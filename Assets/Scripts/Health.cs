using UnityEngine;

namespace Project.Scripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 1;
        [SerializeField] private Sprite deathSprite;
        [SerializeField] private GameObject dropPrefab;
        [SerializeField] private Sprite[] bloodSprites;
        [SerializeField] private bool canBleed = true;
        [SerializeField] private float bloodOffsetRange = 0.2f;
        
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => maxHealth;

        private int _currentHealth;
private bool _isDead;

        public static System.Action OnPlayerDeath;

        private void Start()
{
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (_isDead) return;

            if (amount > 0 && canBleed)
            {
                SpawnBlood();
            }

            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void SpawnBlood()
        {
            if (bloodSprites == null || bloodSprites.Length == 0) return;

            GameObject blood = new GameObject("BloodSplatter");
            
            Vector3 offset = new Vector3(
                Random.Range(-bloodOffsetRange, bloodOffsetRange),
                Random.Range(-bloodOffsetRange, bloodOffsetRange),
                0f
            );
            blood.transform.position = transform.position + offset;
            blood.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            SpriteRenderer sr = blood.AddComponent<SpriteRenderer>();
            sr.sprite = bloodSprites[Random.Range(0, bloodSprites.Length)];
            sr.sortingLayerName = "Ground";
            sr.sortingOrder = 1;
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;

            if (CompareTag("Player"))
            {
                OnPlayerDeath?.Invoke();
            }

            if (dropPrefab != null)
{
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            }

            
            GameObject corpse = new GameObject(name + "_Corpse");
            corpse.transform.position = transform.position;
            corpse.transform.rotation = transform.rotation;
            corpse.transform.localScale = transform.localScale;

            
            SpriteRenderer sr = corpse.AddComponent<SpriteRenderer>();
            sr.sprite = deathSprite;
            
            
            SpriteRenderer originalSR = GetComponent<SpriteRenderer>();
            if (originalSR != null)
            {
                sr.sortingLayerID = originalSR.sortingLayerID;
                sr.sortingOrder = originalSR.sortingOrder;
                sr.color = originalSR.color;
            }

            Destroy(gameObject);
        }
    }
}