using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> waypoints;
        [SerializeField]
        private int currentIndex;
        
        private Action _onDeathReturn;

        public float speed = 2f;

        public void Init(List<Vector3> waypoints, Action returnAction)
        {
            this.waypoints = waypoints;
            currentIndex = 0;
            _onDeathReturn = returnAction;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (waypoints == null || waypoints.Count == 0) return;

            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndex], speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, waypoints[currentIndex]) < 0.1f)
            {
                currentIndex++;

                if (currentIndex >= waypoints.Count)
                {
                    _onDeathReturn?.Invoke(); 
                }
            }
        }
    }
}