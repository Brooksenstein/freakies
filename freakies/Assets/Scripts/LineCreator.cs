using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour {

	public GameObject linePrefab;

	private GameObject undoButton;
	private Line activeLine;
	private List<List<Vector3>> lineVectors;
	private Stack<Line> lines;


	void Start () {
		print ("Yo I should be dead");
		lineVectors = new List<List<Vector3>> ();
		lines = new Stack<Line> ();
	}

	// Update is called once per frame
	void Update () {
		/*
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
		*/
	}

	public void DrawLine(bool mouseDown) {
		if (mouseDown) {
			GameObject newLine = Instantiate (linePrefab);
			activeLine = newLine.GetComponent<Line> ();
		} else if (!mouseDown) {
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

	public void CreateLine () {
		GameObject newLine = Instantiate (linePrefab);
		activeLine = newLine.GetComponent<Line> ();
	}

	public void UpdateLine () {
		if (activeLine != null) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			activeLine.UpdateLine (mousePos);
		}
	}

	public void EndLine () {
		if (activeLine != null) {
			lineVectors.Add (activeLine.GetPoints());
			lines.Push (activeLine);
		}
		activeLine = null;
	}

	public bool hasActiveLine () {
		return activeLine != null;
	}


	public List<List<Vector3>> GetLineVectors () {
		return lineVectors;
	}

	public void Reset() {
		lineVectors = new List<List<Vector3>> ();
	}

	public void UndoLine () {
		Destroy (lines.Pop ().gameObject);
	}
		
}
