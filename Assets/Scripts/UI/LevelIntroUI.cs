using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Project.Scripts.UI
{
    public class LevelIntroUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI mainTextMesh;
        [SerializeField] private GameObject continuePromptObject;

        [Header("Configuration")]
        [SerializeField] private float waitTime = 3f;
        [SerializeField, TextArea(3, 5)] private string introText = "Level Start";

        private bool _canDismiss = false;
        private System.IDisposable _inputSubscription;

        private void Start()
        {
            if (container == null || mainTextMesh == null || continuePromptObject == null)
            {
                Debug.LogWarning("[LevelIntroUI] Missing references. Please assign them in the inspector.");
                return;
            }

            mainTextMesh.text = introText;
            container.SetActive(true);
            continuePromptObject.SetActive(false);

            StartCoroutine(ShowPromptAfterDelay());
        }

        private IEnumerator ShowPromptAfterDelay()
        {
            yield return new WaitForSeconds(waitTime);
            
            continuePromptObject.SetActive(true);
            _canDismiss = true;

            // Subscribe to any button press
            _inputSubscription = InputSystem.onAnyButtonPress.Call(_ => DismissUI());
        }

        private void DismissUI()
        {
            if (!_canDismiss) return;

            container.SetActive(false);
            
            if (_inputSubscription != null)
            {
                _inputSubscription.Dispose();
                _inputSubscription = null;
            }
        }

        private void OnDestroy()
        {
            if (_inputSubscription != null)
            {
                _inputSubscription.Dispose();
            }
        }
    }
}
