using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum FreakyScene { Menu=0, Head=1, Body=2, Reveal=3, HowToPlay=4  }

public class GameControl : MonoBehaviour {
	public static GameControl Control;

	const float canvasSize = 3.5f;
	public float shrinkFactor;

	public GameObject linePrefab;

	private FreakyScene currentScene;
	private Line activeLine;
	private List<List<Vector3>> headLineVectors = new List<List<Vector3>> ();
	private List<List<Vector3>> bodyLineVectors = new List<List<Vector3>> ();
	private List<List<Vector3>> activeVectorList;
	private Stack<Line> lineHistory;

	// Lifecycle

	void Awake () {
		if (Control == null) {
			DontDestroyOnLoad (gameObject);
			Control = this;
		} else if (Control != this) {
			Destroy (gameObject);
		}
	}

	void Start () {
		currentScene = (FreakyScene)SceneManager.GetActiveScene ().buildIndex;
		StartScene (SceneManager.GetActiveScene (), LoadSceneMode.Single);
		SceneManager.sceneLoaded += StartScene;
	}

	void Update () {
		DrawLine ();
	}

	// Public methods

	public void Undo () {
		activeVectorList.RemoveAt (activeVectorList.Count - 1);
		Destroy (lineHistory.Pop ().gameObject);
	}

	public void SetScene (FreakyScene scene) {
		currentScene = scene;
		SceneManager.LoadScene ((int)currentScene);
	}

	// Event handlers

	void StartScene (Scene scene, LoadSceneMode mode) {
		FreakyScene freakyScene = (FreakyScene)scene.buildIndex;
		if (freakyScene == FreakyScene.Head || freakyScene == FreakyScene.Body) {
			if (freakyScene == FreakyScene.Head) {
				// Reset Game
				headLineVectors = new List<List<Vector3>> ();
				bodyLineVectors = new List<List<Vector3>> ();
			}
			lineHistory = new Stack<Line> ();
			activeVectorList = freakyScene.Equals (FreakyScene.Head) ? headLineVectors : bodyLineVectors;
		} else if (freakyScene == FreakyScene.Reveal) {
			DrawFreakie ();
		}
	}

	// Private methods

	void DrawLine () {
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

	void DrawFreakie () {
		headLineVectors.ForEach (delegate(List<Vector3> vectors) {
			GameObject newLineObj = Instantiate (linePrefab);
			Line newLine = newLineObj.GetComponent<Line> ();
			vectors.ForEach (delegate(Vector3 vector) {
				newLine.UpdateLine (new Vector2(vector.x * shrinkFactor, vector.y * shrinkFactor + (canvasSize / 2)));
			});
		});
		bodyLineVectors.ForEach (delegate(List<Vector3> vectors) {
			GameObject newLineObj = Instantiate (linePrefab);
			Line newLine = newLineObj.GetComponent<Line> ();
			vectors.ForEach (delegate(Vector3 vector) {
				newLine.UpdateLine (new Vector2(vector.x * shrinkFactor, vector.y * shrinkFactor - (canvasSize / 2)));
			});
		});
	}
}
