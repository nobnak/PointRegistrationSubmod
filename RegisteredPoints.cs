using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointRegistrationSubmod {

    public class RegisteredPoints : System.IDisposable {

        public readonly List<Point> Latest;
        public readonly List<Point> All;
        public readonly SortedDictionary<int, Point> SortedIDToPoint;

        public RegisteredPoints() {
            this.Latest = new List<Point>();
            this.All = new List<Point>();
            this.SortedIDToPoint = new SortedDictionary<int, Point> ();
        }

        public RegisteredPoints Add(Point p) {
            All.Add (p);
            return this;
        }
        public RegisteredPoints Metabolize(float lifetime) {
            RemoveOldPoints (lifetime);
            BuildLatestPoints ();
            return this;
        }

        public bool HasID(int id) { 
            return SortedIDToPoint.ContainsKey (id);
        }
        public Point LatestPoint(int id) {
            return SortedIDToPoint [id];
        }
        public Vector2 LatestPosition(int id) {
            return LatestPoint (id).rect.center;
        }

        void RemoveOldPoints (float lifetime) {
            var now = Time.timeSinceLevelLoad;
            while (All.Count > 0 && (now - All [0].time) > lifetime)
                All.RemoveAt (0);
        }

        void BuildLatestPoints () {
            Latest.Clear ();
            SortedIDToPoint.Clear ();
            for (var i = All.Count - 1; i >= 0; i--) {
                var pa = All [i];
                if (!SortedIDToPoint.ContainsKey (pa.id)) {
                    Latest.Add (pa);
                    SortedIDToPoint.Add (pa.id, pa);
                }
            }
        }
        
        #region IDisposable implementation
        public void Dispose () {
        }
        #endregion

    }
}
