using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Scene { Head, Body }

public class GameControl : MonoBehaviour {
	public List<Line> headLines;
	public List<Line> bodyLines;
	public Button undoButton;
	public Button doneButton;
	public GameObject linePrefab;

	private bool undoButtonDepressed = false;
	private Line activeLine;
	private List<List<Vector3>> headLineVectors = new List<List<Vector3>> ();
	private List<List<Vector3>> bodyLineVectors = new List<List<Vector3>> ();
	private List<List<Vector3>> activeVectorList;
	private Stack<Line> lineHistory;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		undoButton.onClick.AddListener (Undo);
		doneButton.onClick.AddListener (Next);
		StartScene (Scene.Head);
	}

	// Update is called once per frame
	void Update () {
		HandleUndoButton ();
		if (!undoButtonDepressed) {
			HandleLine ();
		}

	}

	void StartScene (Scene scene) {
		lineHistory = new Stack<Line> ();
		activeVectorList = scene.Equals (Scene.Head) ? headLineVectors : bodyLineVectors;
	}

	void Undo () {
		undoButtonDepressed = true;
		activeVectorList.RemoveAt (activeVectorList.Count - 1);
		Destroy (lineHistory.Pop ().gameObject);
	}

	void Next () {
		Application.LoadLevel(1);
		StartScene (Scene.Body);
	}

	void HandleUndoButton () {
		if (Input.GetMouseButtonUp (0)) {
			undoButtonDepressed = false;
		}
	}

	void HandleLine () {
		Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		bool isInCanvas = hit != null && hit.collider != null;
		bool isLineActive = activeLine != null;

		if (isLineActive && (Input.GetMouseButtonUp (0) || !isInCanvas)) {
			activeVectorList.Add (activeLine.GetPoints ());
			lineHistory.Push (activeLine);
			activeLine = null;
		} else if (isLineActive) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			activeLine.UpdateLine (mousePos);
		} else if (Input.GetMouseButtonDown (0) && isInCanvas) {
			GameObject newLine = Instantiate (linePrefab);
			activeLine = newLine.GetComponent<Line> ();
		}
	}
}
