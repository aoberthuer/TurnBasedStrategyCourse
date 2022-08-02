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

        public void SetBaseAction(BaseAction baseAction)
        {
            _textMeshPro.text = baseAction.GetActionName().ToUpper();
            
            _button.onClick.AddListener(() => UnitActionSystem.Instance.SetSelectedAction(baseAction) );
        }
        
    }
}