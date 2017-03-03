using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointRegistrationSubmod {
    
    public class TextureEventReceiver : MonoBehaviour {
        public string textureName = "_MainTex";
        public Material mat;

        public void SetTexture(Texture tex) {
            mat.SetTexture (textureName, tex);
        }
    }

    [System.Serializable]
    public class TextureEvent : UnityEngine.Events.UnityEvent<Texture> {}
}
