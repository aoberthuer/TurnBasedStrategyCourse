using UnityEngine;

namespace tbs.actions
{
    public class SpinAction : BaseAction
    {
        private float _totalSpinAmount;

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
        
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

            _totalSpinAmount += spinAddAmount;
            if (_totalSpinAmount >= 360f)
            {
                IsActive = false;
            }
        }

        public void Spin()
        {
            IsActive = true;
            _totalSpinAmount = 0f;
        }

    }
}