using UnityEngine;

namespace UI
{
    public class FollowMousePosition : MonoBehaviour
    {
        private void Update()
            => transform.position = Input.mousePosition;
    }
}
