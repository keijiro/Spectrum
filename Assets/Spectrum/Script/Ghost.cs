using UnityEngine;
using UnityEngine.Rendering;

namespace Chimera
{
    public class Ghost
    {
        GameObject _gameObject;
        Transform _transform;
        MeshFilter _meshFilter;
        MeshRenderer _meshRenderer;

        MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();
        Vector3 _velocity;
        float _lifetime;
        float _time;

        public Ghost(Transform parent, Material material)
        {
            var instance = new GameObject("Ghost");

            _transform = instance.GetComponent<Transform>();
            _meshFilter = instance.AddComponent<MeshFilter>();
            _meshRenderer = instance.AddComponent<MeshRenderer>();

            _transform.parent = parent;

            _meshFilter.sharedMesh = new Mesh();
            _meshRenderer.sharedMaterial = material;
            _meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

            _meshRenderer.enabled = false;
        }

        public void Reset(SkinnedMeshRenderer master, float lifetime)
        {
            master.BakeMesh(_meshFilter.sharedMesh);

            _transform.position = master.transform.position;
            _transform.rotation = master.transform.rotation;

            _velocity = Random.insideUnitSphere * 0.2f;
            _lifetime = lifetime;
            _time = 0;

            _propertyBlock.SetFloat("_RandomSeed", Random.value);
            _propertyBlock.SetFloat("_BlastTime", 0);
            _meshRenderer.SetPropertyBlock(_propertyBlock);

            _meshRenderer.enabled = true;
        }

        public void Update()
        {
            if (_time < _lifetime)
            {
                _velocity += Vector3.forward * 2 * Time.deltaTime;
                _transform.position += _velocity * Time.deltaTime;

                _time += Time.deltaTime;
                _propertyBlock.SetFloat("_BlastTime", _time / _lifetime);
                _meshRenderer.SetPropertyBlock(_propertyBlock);
            }
            else
            {
                _meshRenderer.enabled = false;
            }
        }
    }
}
