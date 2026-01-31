using UnityEngine;
using UnityEngine.UI;

namespace GGJ
{
    public class MasksHUDController : MonoBehaviour
    {
        [Header("Skill Icons")]
        [SerializeField] private Image horizontalSkillIcon;
        [SerializeField] private Image verticalSkillIcon;

        [Header("Direction Indicators")]
        [SerializeField] private RectTransform horizontalIndicator;
        [SerializeField] private RectTransform verticalIndicator;

        [Header("Colors")]
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color inactiveColor = Color.gray;

        private enum ActiveSkill { Horizontal, Vertical }
        private ActiveSkill _activeSkill = ActiveSkill.Horizontal;

        private void Start()
        {
            InputManager.Source.OnPlatformPower1 += ActivateHorizontal;
            InputManager.Source.OnPlatformPower2 += ActivateVertical;

            PlatformControllerEvents.OnHorizontalDirectionChanged += OnHorizontalDirectionChanged;
            PlatformControllerEvents.OnVerticalDirectionChanged += OnVerticalDirectionChanged;
        }

        private void OnDestroy()
        {
            InputManager.Source.OnPlatformPower1 -= ActivateHorizontal;
            InputManager.Source.OnPlatformPower2 -= ActivateVertical;

            PlatformControllerEvents.OnHorizontalDirectionChanged -= OnHorizontalDirectionChanged;
            PlatformControllerEvents.OnVerticalDirectionChanged -= OnVerticalDirectionChanged;
        }

        private void ActivateHorizontal()
        {
            if (_activeSkill == ActiveSkill.Horizontal)
                return;

            _activeSkill = ActiveSkill.Horizontal;
            UpdateSkillColors();
        }

        private void ActivateVertical()
        {
            if (_activeSkill == ActiveSkill.Vertical)
                return;

            _activeSkill = ActiveSkill.Vertical;
            UpdateSkillColors();
        }

        private void UpdateSkillColors()
        {
            horizontalSkillIcon.color =
                _activeSkill == ActiveSkill.Horizontal ? activeColor : inactiveColor;

            verticalSkillIcon.color =
                _activeSkill == ActiveSkill.Vertical ? activeColor : inactiveColor;
        }

        private void OnHorizontalDirectionChanged(int direction)
        {
            horizontalIndicator.localScale = direction > 0 ? Vector3.one : Vector3.one * -1f;

            if (_activeSkill == ActiveSkill.Horizontal)
            {
                verticalSkillIcon.color = inactiveColor;
            }
        }

        private void OnVerticalDirectionChanged(int direction)
        {
            verticalIndicator.localScale = direction > 0 ? Vector3.one : Vector3.one * -1f;
        }
    }
}
