using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	public LineRenderer lineRenderer;

	private List<Vector2> points;

	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	public void UpdateLine (Vector2 point) {
		if (points == null) {
			points = new List<Vector2> ();
			SetPoint (point);
		}

		if (Vector2.Distance (points.Last (), point) > .1f) {
			SetPoint (point);
		}
	}

	public List<Vector2> GetPoints () {
		return points;
	}

	private void SetPoint(Vector2 point) {
		points.Add (point);
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPosition (points.Count - 1, point);
	}
}
