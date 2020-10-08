using System;
using controller;
using UnityEngine;

namespace character
{
    public enum InnocentState
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

        public void SetMode(InnocentState state)
        {
            if (state == InnocentState.Neutral)
            {
                neutralSprite.gameObject.SetActive(true);
                runningSprite.gameObject.SetActive(false);
            }
            else if (state == InnocentState.Running)
            {
                runningSprite.gameObject.SetActive(true);
                neutralSprite.gameObject.SetActive(false);
            }
        }
    }
}