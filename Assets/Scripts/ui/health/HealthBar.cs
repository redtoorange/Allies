using System;
using UnityEngine;

namespace ui.health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private HealthSlot healthSlot1;

        [SerializeField]
        private HealthSlot healthSlot2;

        [SerializeField]
        private HealthSlot healthSlot3;

        public void SetHealth(int amount)
        {
            switch (amount)
            {
                case 6:
                    healthSlot1.SetStatus(HealthSlotStatus.FULL);
                    healthSlot2.SetStatus(HealthSlotStatus.FULL);
                    healthSlot3.SetStatus(HealthSlotStatus.FULL);
                    break;
                case 5:
                    healthSlot1.SetStatus(HealthSlotStatus.FULL);
                    healthSlot2.SetStatus(HealthSlotStatus.FULL);
                    healthSlot3.SetStatus(HealthSlotStatus.HALF);
                    break;
                case 4:
                    healthSlot1.SetStatus(HealthSlotStatus.FULL);
                    healthSlot2.SetStatus(HealthSlotStatus.FULL);
                    healthSlot3.SetStatus(HealthSlotStatus.EMPTY);
                    break;
                case 3:
                    healthSlot1.SetStatus(HealthSlotStatus.FULL);
                    healthSlot2.SetStatus(HealthSlotStatus.HALF);
                    healthSlot3.SetStatus(HealthSlotStatus.EMPTY);
                    break;
                case 2:
                    healthSlot1.SetStatus(HealthSlotStatus.FULL);
                    healthSlot2.SetStatus(HealthSlotStatus.EMPTY);
                    healthSlot3.SetStatus(HealthSlotStatus.EMPTY);
                    break;
                case 1:
                    healthSlot1.SetStatus(HealthSlotStatus.HALF);
                    healthSlot2.SetStatus(HealthSlotStatus.EMPTY);
                    healthSlot3.SetStatus(HealthSlotStatus.EMPTY);
                    break;
                case 0:
                    healthSlot1.SetStatus(HealthSlotStatus.EMPTY);
                    healthSlot2.SetStatus(HealthSlotStatus.EMPTY);
                    healthSlot3.SetStatus(HealthSlotStatus.EMPTY);
                    break;
                default:
                    Debug.LogError("Unsupported Health count: " + amount);
                    break;
            }
        }
    }
}