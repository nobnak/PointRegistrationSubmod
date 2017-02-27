using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointRegistrationSubmod {

    public class RegisteredPoints : System.IDisposable {

        public readonly List<Point> Latest;
        public readonly List<Point> All;

        public RegisteredPoints() {
            this.Latest = new List<Point>();
            this.All = new List<Point>();
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

        void RemoveOldPoints (float lifetime) {
            var now = Time.timeSinceLevelLoad;
            while (All.Count > 0 && (now - All [0].time) > lifetime)
                All.RemoveAt (0);
        }

        void BuildLatestPoints () {
            Latest.Clear ();
            for (var i = All.Count - 1; i >= 0; i--) {
                var pa = All [i];
                if (Latest.FindIndex (pl => pl.id == pa.id) < 0)
                    Latest.Add (pa);
            }
        }
        
        #region IDisposable implementation
        public void Dispose () {
        }
        #endregion

    }
}
