using System;
using System.Collections.Generic;
using character;
using UnityEngine;

namespace util
{
    [Serializable]
    public class TargetManager<T> where T : GameCharacter
    {
        [SerializeField]
        private List<T> targets = new List<T>();

        [SerializeField]
        private bool dirty;

        public void AddTarget(T target)
        {
            if (!targets.Contains(target))
            {
                targets.Add(target);
            }
        }

        public void RemoveTarget(T target)
        {
            if (targets.Contains(target))
            {
                dirty = true;
                targets.Remove(target);
            }
        }

        public int TargetCount()
        {
            SanitizeList();
            return targets.Count;
        }

        public List<T> GetTargets()
        {
            SanitizeList();
            return targets;
        }

        private void SanitizeList()
        {
            if (dirty)
            {
                List<T> newTargets = new List<T>();
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] != null)
                    {
                        newTargets.Add(targets[i]);
                    }
                }

                dirty = false;
                targets = newTargets;
            }
        }

        public T GetTarget()
        {
            SanitizeList();

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    return targets[i];
                }
            }

            return null;
        }

        public T GetClosestTarget(Vector2 position)
        {
            SanitizeList();

            T target = null;
            float distance = float.MaxValue;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] != null)
                {
                    float dist = Vector2.Distance(position, targets[i].GetPosition());
                    if (dist < distance)
                    {
                        target = targets[i];
                    }
                }
            }

            return target;
        }
    }
}