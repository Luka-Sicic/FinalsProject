using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindUI : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private int bindingIndex;
    [SerializeField] private TMP_Text actionLabel;
    [SerializeField] private TMP_Text bindingText;
    [SerializeField] private Button rebindButton;
    [SerializeField] private GameObject overlay;

    public void Initialize(InputActionReference actionRef, int index, TMP_Text label, TMP_Text text, Button button, GameObject overlayObj)
    {
        actionReference = actionRef;
        bindingIndex = index;
        actionLabel = label;
        bindingText = text;
        rebindButton = button;
        overlay = overlayObj;
        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
        if (rebindButton != null)
            rebindButton.onClick.AddListener(StartRebinding);
    }

    private void OnDisable()
    {
        if (rebindButton != null)
            rebindButton.onClick.RemoveListener(StartRebinding);
        _rebindOperation?.Dispose();
    }

    private void UpdateUI()
    {
        if (actionReference == null || actionReference.action == null) return;
        if (actionLabel == null || bindingText == null) return;

        actionLabel.text = actionReference.action.name;

        var binding = actionReference.action.bindings[bindingIndex];
        if (binding.isComposite)
        {
             actionLabel.text += " (Composite)";
        }
        else if (binding.isPartOfComposite)
        {
            actionLabel.text += " - " + binding.name;
        }

        bindingText.text = actionReference.action.GetBindingDisplayString(bindingIndex);
    }

    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    private void StartRebinding()
    {
        actionReference.action.Disable();

        overlay.SetActive(true);
        bindingText.text = "Waiting for input...";

        _rebindOperation = actionReference.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Pointer>/delta")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishRebinding())
            .OnCancel(operation => FinishRebinding())
            .Start();
    }

    private void FinishRebinding()
    {
        _rebindOperation.Dispose();
        _rebindOperation = null;

        overlay.SetActive(false);
        UpdateUI();
        actionReference.action.Enable();

        if (InputSettingsManager.Instance != null)
            InputSettingsManager.Instance.SaveOverrides();
    }
}