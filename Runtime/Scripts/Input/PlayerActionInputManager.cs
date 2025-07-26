using UnityEngine;

namespace Chonker.Core.Scripts.Input
{
    public abstract class PlayerActionInputManager : MonoBehaviour
    {
        
        public abstract Vector2 ReadMovementInput();

        public Vector3 ReadMovementInputAsVector3() {
            Vector2 RawInput = ReadMovementInput();
            return new Vector3(RawInput.x, 0, RawInput.y);
        }
        public abstract Vector2 ReadLookInputDelta();
        
    }
}
