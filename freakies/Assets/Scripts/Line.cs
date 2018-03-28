using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	public LineRenderer lineRenderer;

	//TODO: These can be made Vector2 points again probably, I made them Vector3 when messing with render ordering.
	private List<Vector3> points;

	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.sortingLayerName = "Foreground";
	}

	public void UpdateLine (Vector2 pointIn) {
		Vector3 point = new Vector3 (pointIn.x, pointIn.y, 10);
		if (points == null) {
			points = new List<Vector3> ();
			SetPoint (point);
		}

		if (Vector3.Distance (points.Last (), point) > .05f) {
			SetPoint (point);
		}
	}

	public List<Vector3> GetPoints () {
		return points;
	}

	private void SetPoint(Vector3 point) {
		points.Add (point);
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPosition (points.Count - 1, point);
	}
}
