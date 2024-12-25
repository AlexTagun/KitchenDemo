using UnityEngine;

namespace KitchenDemo.Gameplay
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private CharacterAnimator _characterAnimator;
        
        private void Awake()
        {
            _characterAnimator = new CharacterAnimator(_animator);
        }

        public void SetAnimatorParameters(float movementSpeed)
        {
            _characterAnimator.SetRun(movementSpeed);
        }
    }
}