using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Chimera
{
    public class Afterimage : MonoBehaviour
    {
        [Space]

        [SerializeField]
        int _maxGhosts = 6;

        [SerializeField]
        float _interval = 1;

        [Space]

        [SerializeField]
        SkinnedMeshRenderer _master;

        [SerializeField]
        Material _material;

        [Space]

        [SerializeField]
        bool _emit = true;

        public bool emit {
            get { return _emit; }
            set { _emit = value; }
        }

        Queue<Ghost> _ghostPool = new Queue<Ghost>();

        IEnumerator Start()
        {
            for (var i = 0; i < _maxGhosts; i++)
                _ghostPool.Enqueue(new Ghost(transform, _material));

            while (true)
            {
                yield return new WaitForSeconds(_interval);

                if (_emit)
                {
                    var ghost = _ghostPool.Dequeue();
                    ghost.Reset(_master, _interval * _maxGhosts);
                    _ghostPool.Enqueue(ghost);
                }
            }
        }

        void Update()
        {
            foreach (var ghost in _ghostPool) ghost.Update();
        }
    }
}
