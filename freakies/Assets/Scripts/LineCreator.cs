using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {

	public GameObject linePrefab;

	private Line activeLine;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GameObject newLine = Instantiate (linePrefab);
			activeLine = newLine.GetComponent<Line> ();
		} else if (Input.GetMouseButtonUp (0)) {
			activeLine = null;
		}

		if (activeLine != null) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			activeLine.UpdateLine (mousePos);
		}
	}
}
