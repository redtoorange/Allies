using System;
using UnityEngine;

namespace ui.health
{
    [Serializable]
    public enum HealthSlotStatus
    {
        FULL,
        HALF,
        EMPTY
    }

    public class HealthSlot : MonoBehaviour
    {
        [SerializeField]
        private GameObject fullHeart;

        [SerializeField]
        private GameObject halfHeart;

        [SerializeField]
        private GameObject emptyHeart;

        private HealthSlotStatus currentStatus = HealthSlotStatus.FULL;

        public void SetStatus(HealthSlotStatus newStatus)
        {
            currentStatus = newStatus;
            
            if (currentStatus == HealthSlotStatus.FULL)
            {
                fullHeart.SetActive(true);
                halfHeart.SetActive(false);
                emptyHeart.SetActive(false);
            }
            else if (currentStatus == HealthSlotStatus.HALF)
            {
                fullHeart.SetActive(false);
                halfHeart.SetActive(true);
                emptyHeart.SetActive(false);
            }
            else if (currentStatus == HealthSlotStatus.EMPTY)
            {
                fullHeart.SetActive(false);
                halfHeart.SetActive(false);
                emptyHeart.SetActive(true);
            }
        }
    }
}