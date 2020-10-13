using System;
using UnityEngine;

namespace character
{
    [Serializable]
    public enum ZombieState
    {
        Shamble,
        Chase,
        Combat
    }

    /// <summary>
    /// Move toward the closest Ally or Player.  When they are contacted, they will be converted into a Zombie.
    /// </summary>
    public class Zombie : GameCharacter
    {
        [SerializeField]
        private SpriteRenderer shambleSprite;

        [SerializeField]
        private SpriteRenderer chaseSprite;

        private void Start()
        {
            base.Start();
        }

        public void SetMode(ZombieState state)
        {
            if (shambleSprite == null || chaseSprite == null) return;

            if (state == ZombieState.Shamble)
            {
                shambleSprite.gameObject.SetActive(true);
                chaseSprite.gameObject.SetActive(false);
            }
            else if (state == ZombieState.Chase || state == ZombieState.Combat)
            {
                chaseSprite.gameObject.SetActive(true);
                shambleSprite.gameObject.SetActive(false);
            }
        }
    }
}