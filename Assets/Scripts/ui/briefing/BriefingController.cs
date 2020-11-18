using System.Collections.Generic;
using controller;
using preferences.unlock;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ui.briefing
{
    public class BriefingController : MonoBehaviour
    {
        private LevelDataContainer levelDataContainer;

        [SerializeField]
        private TextMeshProUGUI text;
        [Header("NavButtons")]
        [SerializeField]
        private Button nextButton;
        [SerializeField]
        private Button doneButton;
        [SerializeField]
        private Button skipButton;


        private int currentLine = 0;
        private List<string> briefingLines;
        private CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            levelDataContainer = FindObjectOfType<LevelDataContainer>();
            briefingLines = levelDataContainer.GetBriefingData();

            if (briefingLines == null || briefingLines.Count == 0)
            {
                text.text = "No briefing, get it done";

                nextButton.gameObject.SetActive(false);
                doneButton.gameObject.SetActive(true);
            }
            else
            {
                text.text = briefingLines[currentLine];
            }
        }

        public void DisplayNextLine()
        {
            currentLine += 1;
            text.text = briefingLines[currentLine];

            if (currentLine + 1 == briefingLines.Count)
            {
                nextButton.gameObject.SetActive(false);
                doneButton.gameObject.SetActive(true);
            }
        }

        public void NextClicked()
        {
            DisplayNextLine();
        }

        public void DoneClicked()
        {
            LeanTween.alphaCanvas(canvasGroup, 0, 0.5f)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() =>
                {
                    GameController.S.SetGamePaused(false);
                    gameObject.SetActive(false);
                });
        }

        public void SkipClicked()
        {
            DoneClicked();
        }
    }
}