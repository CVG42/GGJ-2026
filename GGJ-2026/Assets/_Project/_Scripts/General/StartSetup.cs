using UnityEngine;

namespace GGJ
{
    public class StartSetup : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Source.ResumeGame();
        }
    }
}
