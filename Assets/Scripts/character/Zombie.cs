using System;
using UnityEngine;

namespace character
{
    [Serializable]
    public enum ZombieMode
    {
        SHAMBLE,
        CHASE,
        COMBAT
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

        public void SetMode(ZombieMode mode)
        {
            if (mode == ZombieMode.SHAMBLE)
            {
                shambleSprite.gameObject.SetActive(true);
                chaseSprite.gameObject.SetActive(false);
            }
            else if (mode == ZombieMode.CHASE || mode == ZombieMode.COMBAT)
            {
                chaseSprite.gameObject.SetActive(true);
                shambleSprite.gameObject.SetActive(false);
            }
        }
    }
}