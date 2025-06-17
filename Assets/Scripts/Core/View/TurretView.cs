using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Core.View
{
    public class TurretView
    {
        private float minYaw = 90f;
        private float maxYaw = 270f;

        private float _currentYaw;
        private Transform _selfTransform;
        
        public TurretView(Transform selfTransform)
        {
            _selfTransform = selfTransform;
        }

        public void SetFromScreenPosition(float screenX)
        {
            float normalizedX = screenX / Screen.width;
            _currentYaw = Mathf.Lerp(minYaw, maxYaw, normalizedX);
            ApplyRotation();
        }

        private void ApplyRotation()
        {
            _selfTransform.localRotation = Quaternion.Euler(-90, _currentYaw, 0f);
        }
    }
}