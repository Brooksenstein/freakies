using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {

	public GameObject linePrefab;

	private Line activeLine;
	private List<List<Vector2>> lineVectors;
	private Stack<Line> lines;

	void Start () {
		lineVectors = new List<List<Vector2>> ();
		lines = new Stack<Line> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GameObject newLine = Instantiate (linePrefab);
			activeLine = newLine.GetComponent<Line> ();
		} else if (Input.GetMouseButtonUp (0)) {
			if (activeLine != null) {
				lineVectors.Add (activeLine.GetPoints());
				lines.Push (activeLine);
			}
			activeLine = null;
		}

		if (activeLine != null) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			activeLine.UpdateLine (mousePos);
		}
	}

	public List<List<Vector2>> GetLineVectors () {
		return lineVectors;
	}

	public void Reset() {
		lineVectors = new List<List<Vector2>> ();
	}

	public void UndoLine () {
		Destroy (lines.Pop ().gameObject);
	}
		
}
