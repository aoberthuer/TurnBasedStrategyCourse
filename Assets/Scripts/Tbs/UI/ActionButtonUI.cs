using tbs.actions;
using tbs.units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace tbs.ui
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Button _button;
        
        [SerializeField] private Image _selectionImage;

        private BaseAction _baseAction;

        public void SetBaseAction(BaseAction baseAction)
        {
            _baseAction = baseAction;
            
            _textMeshPro.text = baseAction.GetActionName().ToUpper();
            _button.onClick.AddListener(() => UnitActionSystem.Instance.SetSelectedAction(baseAction) );
        }
        
        public void UpdateSelectedVisual()
        {
            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            _selectionImage.gameObject.SetActive(selectedAction == _baseAction);
        }
        
    }
}