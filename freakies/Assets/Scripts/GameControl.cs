using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
	public List<Line> headLines;
	public List<Line> bodyLines;
	public Button undoButton;
	private bool clicklingButton;

	public GameObject linePrefab;

	private Line activeLine;
	private List<List<Vector3>> lineVectors;
	private Stack<Line> lines;
	private BoxCollider2D undoCollider;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		clicklingButton = false;
		undoButton.onClick.AddListener (ButtonClicked);
		lineVectors = new List<List<Vector3>> ();
		lines = new Stack<Line> ();
		print ("X: " + undoButton.transform.position.x + " Y: " + undoButton.transform.position.y + " Z: " + undoButton.transform.position.z);
		undoCollider = undoButton.GetComponent<BoxCollider2D> ();
		print(undoCollider.size);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			clicklingButton = false;
			print ("Mouse button up " + clicklingButton);
		}
		if (!clicklingButton) {
			DrawLine ();
		}

	}

	void ButtonClicked () {
		clicklingButton = true;
		Destroy (lines.Pop ().gameObject);
		print ("I HAVE BEEN CLICKED! " + clicklingButton);
	}

	void DrawLine () {
		Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (Input.GetMouseButtonDown (0)) {
			if (hit != null && hit.collider != null) {
				GameObject newLine = Instantiate (linePrefab);
				activeLine = newLine.GetComponent<Line> ();
			}

		} else if (Input.GetMouseButtonUp (0) || (hit == null || hit.collider == null)) {
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
}
