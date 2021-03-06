﻿using System.Collections.Generic;
using UnityEngine;

namespace managers
{
    public abstract class AIManager<T> : MonoBehaviour
    {
        [SerializeField]
        protected List<T> controllers;
        protected SystemManager systemManager;
        protected GameRoundManager gameRoundManager;

        protected void Start()
        {
            controllers = new List<T>(GetComponentsInChildren<T>());
            systemManager = GetComponentInParent<SystemManager>();

            gameRoundManager = GetComponentInParent<SystemManager>().GetGameRoundManager();
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


        public int GetControllerCount()
        {
            return controllers.Count;
        }
        
        protected abstract void HandlePhaseChange(GameRoundPhase phase);
    }
}