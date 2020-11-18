using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ui.levelSelect
{
    public class LevelRow : MonoBehaviour
    {
        [Header("Linked Dependencies")]
        [SerializeField]
        private Image levelImage;
        [SerializeField]
        private TextMeshProUGUI levelLabel;
        [SerializeField]
        private Button loadButton;

        [Header("Locked Level")]
        [SerializeField]
        private Sprite lockedImage;
        [SerializeField]
        private string lockedTitle;

        private LevelData data;

        public void SetData(LevelData data)
        {
            this.data = data;
        }

        public void SetLevelUnlocked(bool isUnlocked)
        {
            if (isUnlocked)
            {
                levelImage.sprite = data.levelImage;
                levelLabel.text = data.levelLabel;
                loadButton.interactable = true;
            }
            else
            {
                levelImage.sprite = lockedImage;
                levelLabel.text = lockedTitle;
                loadButton.interactable = false;
            }
        }

        public void OnLoadClicked()
        {
            SceneManager.LoadScene(data.levelIndex);
        }
    }
}