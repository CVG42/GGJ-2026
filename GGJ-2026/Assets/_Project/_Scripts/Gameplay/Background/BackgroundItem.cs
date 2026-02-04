using UnityEngine;

namespace GGJ
{
    public class BackgroundItem : MonoBehaviour
    {
        [SerializeField] private Sprite _daySprite;
        [SerializeField] private Sprite _nightSprite;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            LevelVisualsManager.Source.RegisterSpriteRenderer(_spriteRenderer, _daySprite, _nightSprite);
        }

        private void OnDestroy()
        {
            LevelVisualsManager.Source.UnregisterSpriteRenderer(_spriteRenderer);
        }
    }
}
