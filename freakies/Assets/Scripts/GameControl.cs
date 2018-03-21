using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum FreakyScene { Head=0, Body=1, Reveal=2 }

public class GameControl : MonoBehaviour {
	public static GameControl Control;

	public GameObject linePrefab;

	private FreakyScene currentScene;
	private bool undoButtonDepressed = false;
	private Line activeLine;
	private List<List<Vector3>> headLineVectors;
	private List<List<Vector3>> bodyLineVectors;
	private List<List<Vector3>> activeVectorList;
	private Stack<Line> lineHistory;

	void Awake () {
		if (Control == null) {
			DontDestroyOnLoad (gameObject);
			Control = this;
		} else if (Control != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		headLineVectors = headLineVectors == null ? new List<List<Vector3>> () : headLineVectors;
		bodyLineVectors = bodyLineVectors == null ? new List<List<Vector3>> () : bodyLineVectors;
		currentScene = (FreakyScene)SceneManager.GetActiveScene ().buildIndex;
		StartScene (SceneManager.GetActiveScene (), LoadSceneMode.Single);
		SceneManager.sceneLoaded += StartScene;
	}

	// Update is called once per frame
	void Update () {
		HandleUndoButton ();
		if (!undoButtonDepressed) {
			HandleLine ();
		}

	}

	void StartScene (Scene scene, LoadSceneMode mode) {
		Debug.Log (scene.buildIndex);
		FreakyScene freakyScene = (FreakyScene)scene.buildIndex;
		if (freakyScene == FreakyScene.Head || freakyScene == FreakyScene.Body) {
			lineHistory = new Stack<Line> ();
			activeVectorList = scene.Equals (FreakyScene.Head) ? headLineVectors : bodyLineVectors;
		} else if (freakyScene == FreakyScene.Reveal) {
			Debug.Log ("Making lines!");
			Debug.Log ("Head lines are: " + headLineVectors.Count);
			headLineVectors.ForEach (delegate(List<Vector3> vectors) {
				GameObject newLineObj = Instantiate (linePrefab);
				Line newLine = newLineObj.GetComponent<Line> ();
				vectors.ForEach (delegate(Vector3 vector) {
					newLine.UpdateLine ((Vector2)vector);
				});
			});
		}
	}

	public void Undo () {
		undoButtonDepressed = true;
		activeVectorList.RemoveAt (activeVectorList.Count - 1);
		Destroy (lineHistory.Pop ().gameObject);
	}

	public void Next () {
		currentScene = (FreakyScene)((int)currentScene + 1);
		SceneManager.LoadScene((int)currentScene);
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
