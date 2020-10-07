using System.Collections.Generic;
using controller;
using UnityEngine;

namespace managers
{
    public class AIManager<T> : MonoBehaviour 
    {
        protected GameManager gameManager = null;
        protected List<T> controllers = null;
        protected GameRoundManager grm = null;


        protected void AddController(T controller)
        {
            controllers.Add(controller);

            grm = FindObjectOfType<GameRoundManager>();
            grm.OnPhaseChange.AddListener(HandlePhaseChange);
        }

        protected void RemoveController(T controller)
        {
            controllers.Remove(controller);
        }

        protected void Start()
        {
            controllers = new List<T>(GetComponentsInChildren<T>());
            gameManager = GetComponentInParent<GameManager>();
        }

        protected virtual void HandlePhaseChange(GameRoundPhase phase)
        {
        }
    }
}