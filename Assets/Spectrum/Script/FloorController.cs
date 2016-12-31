using UnityEngine;

namespace Spectrum
{
    public class FloorController : MonoBehaviour
    {
        [SerializeField] Transform _target;

        MaterialPropertyBlock _block;

        void Start()
        {
            _block = new MaterialPropertyBlock();
        }

        void Update()
        {
            var p = _target.position;
            p.y = 0;

            _block.SetVector("_Origin", p);

            GetComponent<Renderer>().SetPropertyBlock(_block);
        }
    }
}
