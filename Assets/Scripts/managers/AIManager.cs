using System.Collections.Generic;
using UnityEngine;

namespace managers
{
    public abstract class AIManager<T> : MonoBehaviour
    {
        protected List<T> controllers;
        protected GameManager gameManager;
        protected GameRoundManager gameRoundManager;

        protected void Start()
        {
            controllers = new List<T>(GetComponentsInChildren<T>());
            gameManager = GetComponentInParent<GameManager>();

            gameRoundManager = GetComponentInParent<GameManager>().GetGameRoundManager();
            gameRoundManager.OnPhaseChange += HandlePhaseChange;
        }

        protected void AddController(T controller)
        {
            controllers.Add(controller);
        }

        protected void RemoveController(T controller)
        {
            controllers.Remove(controller);
        }


        protected abstract void HandlePhaseChange(GameRoundPhase phase);
    }
}