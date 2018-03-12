using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {

	public GameObject linePrefab;

	private Line activeLine;
	private List<List<Vector2>> lines = new List<List<Vector2>> ();

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GameObject newLine = Instantiate (linePrefab);
			activeLine = newLine.GetComponent<Line> ();
		} else if (Input.GetMouseButtonUp (0)) {
			if (activeLine != null) {
				lines.Add (activeLine.GetPoints());
			}
			activeLine = null;
		}

		if (activeLine != null) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			activeLine.UpdateLine (mousePos);
		}
	}

	public List<List<Vector2>> GetLines () {
		return lines;
	}

	public void Reset() {
		lines = new List<List<Vector2>> ();
	}
}
