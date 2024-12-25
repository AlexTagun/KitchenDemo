using UnityEngine;

namespace KitchenDemo.Gameplay
{
    public class CharacterAnimator
    {
        private static readonly int Run = Animator.StringToHash("Run");

        private readonly Animator _animator;

        public CharacterAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void SetRun(float run)
        {
            _animator.SetFloat(Run, run);
        }
    }
}