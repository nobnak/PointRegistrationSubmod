using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gist;

namespace PointRegistrationSubmod {
        
    public class LandmarkRegistration : MonoBehaviour {
        public Camera targetCam;
        public float lifetime = 3f;

        [Header("Debug")]
        public bool isDebugMode;
        public static readonly Color[] POINT_MARKER_TYPE_COLOR = new Color[] {
            Color.red, Color.green, Color.blue 
        };
        public float pointMarkerSize = 1f;
        public float pointMarkerDepth = 10f;

        public RegisteredPoints Registered { get; private set; }

        public Vector3 NormalizedToLocalPosition(Vector3 normalizedPosition) {
            var p = targetCam.ViewportToWorldPoint (normalizedPosition);
            return targetCam.transform.InverseTransformPoint (p);
        }

        #region Unity
        void OnEnable() {
            Registered = new RegisteredPoints ();
        }
        void Update() {
            Registered.Metabolize (lifetime);
        }
        void OnRenderObject() {
            if ((targetCam.cullingMask & (1 << gameObject.layer)) == 0 || !isActiveAndEnabled)
                return;
            if (!isDebugMode)
                return;

            var fig = GLFigure.Instance;
            for (var i = 0; i < Registered.Latest.Count; i++) {
                var p = Registered.Latest [i];
                var viewportPos = (Vector3)p.rect.center;
                viewportPos.z = pointMarkerDepth;
                var worldPos = targetCam.ViewportToWorldPoint (viewportPos);
                fig.FillCircle (worldPos, targetCam.transform.rotation, pointMarkerSize * Vector3.one,
                    POINT_MARKER_TYPE_COLOR [p.type]);
            }
        }
        #endregion

        #region Event
        public void Receive(Osc.Message m) {
            Point p;
            if (Point.TryParse (m, out p))
                Registered.Add (p);
        }
        public void Error(System.Exception e) {
            Debug.LogErrorFormat ("Error {0}", e);
        }
        #endregion

    }
}
