using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointRegistrationSubmod {
        
    public class PointsMonoBehaviour : MonoBehaviour {
        public Camera targetCam;
        public float lifetime = 3f;
        public Texture pointMarkerTexture;

        RegisteredPoints registered;

        public List<Point> Points { get { return registered.Latest; } }

        #region Unity
        void OnEnable() {
            registered = new RegisteredPoints ();
        }
        void Update() {
            registered.Metabolize (lifetime);
        }
        void OnRenderObject() {
            if ((targetCam.cullingMask & (1 << gameObject.layer)) == 0 || !isActiveAndEnabled)
                return;

            for (var i = 0; i < registered.Latest.Count; i++) {
                var p = registered.Latest [i];
                
            }
        }
        #endregion

        #region Event
        public void Receive(Osc.OscPort.Capsule c) {
            Point p;
            if (Point.TryParse (c, out p))
                registered.Add (p);
        }
        public void Error(System.Exception e) {
            Debug.LogErrorFormat ("Error {0}", e);
        }
        #endregion
    
    }
}
