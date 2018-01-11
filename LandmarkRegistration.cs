using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nobnak.Gist;
using UnityEngine.Assertions;
using nobnak.Gist.Extensions.ComponentExt;

namespace PointRegistrationSubmod {
    
    [ExecuteInEditMode]
    public class LandmarkRegistration : MonoBehaviour {
        public const int TYPE_HILL = 0;
        public const int TYPE_POND = 1;
        public enum LandmarkTypeEnum { Hill = TYPE_HILL, Pond = TYPE_POND }
        public static readonly Color[] POINT_MARKER_TYPE_COLOR = new Color[] {
            Color.red, Color.green, Color.blue 
        };

        public Camera targetCam;
        public float lifetime = 3f;

        [Header("Debug")]
        public bool isDebugMode;
        public float pointMarkerSize = 1f;
        public float pointMarkerDepth = 10f;

        public RegisteredPoints Registered { get; private set; }

        #region Unity
        void OnEnable() {
            Registered = new RegisteredPoints ();
        }
        void Update() {
            Registered.Metabolize (lifetime);
        }
        void OnRenderObject() {
            if (!this.IsVisibleLayer())
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
        void OnDisable() {
            Registered.Dispose ();
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
