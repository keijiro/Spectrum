using UnityEngine;
using Klak.Math;

namespace Chimera
{
    public class Puppet2 : MonoBehaviour
    {
        #region Neck joint

        [SerializeField] float _neckBias = 3;
        [SerializeField] float _neckFrequency = 0.16f;
        [SerializeField] float _neckRotation = 15;

        Quaternion CalculateNeckRotation()
        {
            var r = _neckNoise.Vector(2) * _neckRotation;
            r.x += _neckBias;
            return Quaternion.Euler(r);
        }

        #endregion

        #region Spine joints

        [Space]
        [SerializeField] float _spineBias = 3;
        [SerializeField] float _spineFrequency = 0.15f;
        [SerializeField] float _spineRotation = 8;

        Quaternion CalculateSpineRotation()
        {
            var r = _spineNoise.Vector(3) * _spineRotation;
            r.x += _spineBias;
            return Quaternion.Euler(r);
        }

        #endregion

        #region Hand IK target

        [Space]
        [SerializeField] Vector3 _handTarget = new Vector3(0.23f, -0.08f, 0.2f);
        [SerializeField] float _handFrequency = 0.13f;
        [SerializeField] float _handTranslation = 0.1f;
        [SerializeField] float _handRotation = 15;

        Vector3 CalculateHandPosition(bool right)
        {
            var p = _handTarget;
            if (!right) p.x *= -1;
            p += _handNoise.Vector(right ? 4 : 5) * _handTranslation;
            var neck = _animator.GetBoneTransform(HumanBodyBones.Chest);
            return neck.TransformPoint(p);
        }

        Quaternion CalculateHandRotation(bool right)
        {
            return _handNoise.Rotation(right ? 6 : 7, _handRotation);
        }

        #endregion

        #region MonoBehaviour functions

        Animator _animator;

        NoiseGenerator _neckNoise;
        NoiseGenerator _spineNoise;
        NoiseGenerator _handNoise;

        Vector3 _leftHandPosition;
        Vector3 _rightHandPosition;

        void Start()
        {
            _animator = GetComponent<Animator>();

            _neckNoise  = new NoiseGenerator(_neckFrequency );
            _spineNoise = new NoiseGenerator(_spineFrequency);
            _handNoise  = new NoiseGenerator(_handFrequency );
        }

        void Update()
        {
            _neckNoise .Frequency = _neckFrequency;
            _spineNoise.Frequency = _spineFrequency;
            _handNoise .Frequency = _handFrequency;

            _neckNoise .Step();
            _spineNoise.Step();
            _handNoise .Step();

            _leftHandPosition  = CalculateHandPosition(false);
            _rightHandPosition = CalculateHandPosition(true);
        }


        void OnAnimatorIK(int layerIndex)
        {
            var spine = CalculateSpineRotation();
            _animator.SetBoneLocalRotation(HumanBodyBones.Spine, spine);
            _animator.SetBoneLocalRotation(HumanBodyBones.Chest, spine);

            var neck = CalculateNeckRotation();
            _animator.SetBoneLocalRotation(HumanBodyBones.Neck, neck);
            _animator.SetBoneLocalRotation(HumanBodyBones.Head, neck);

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandPosition);
            _animator.SetBoneLocalRotation(HumanBodyBones.LeftHand, CalculateHandRotation(false));

            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandPosition);
            _animator.SetBoneLocalRotation(HumanBodyBones.RightHand, CalculateHandRotation(true));
        }

        #endregion
    }
}
