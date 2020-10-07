using System;
using controller;
using UnityEngine;

namespace character
{
    public enum InnocentMode
    {
        Neutral,
        Running
    }

    /// <summary>
    /// Neutral character, will move to avoid the closest zombie.
    /// </summary>
    public class Innocent : GameCharacter
    {
        [SerializeField]
        private SpriteRenderer neutralSprite;

        [SerializeField]
        private SpriteRenderer runningSprite;
        
        private void Start()
        {
            base.Start();
        }

        public void SetMode(InnocentMode mode)
        {
            if (mode == InnocentMode.Neutral)
            {
                neutralSprite.gameObject.SetActive(true);
                runningSprite.gameObject.SetActive(false);
            }
            else if (mode == InnocentMode.Running)
            {
                runningSprite.gameObject.SetActive(true);
                neutralSprite.gameObject.SetActive(false);
            }
        }
    }
}