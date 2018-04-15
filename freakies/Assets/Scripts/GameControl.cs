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
	private List<List<Vector2>> headLineVectors = new List<List<Vector2>> ();
	private List<Color> headLineColors = new List<Color> ();
	private List<List<Vector2>> bodyLineVectors = new List<List<Vector2>> ();
	private List<Color> bodyLineColors = new List<Color> ();
	private List<List<Vector2>> activeVectorList;
	private List<Color> activeColorList;
	private Stack<Line> lineHistory;
	private int lineCount;

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
		lineCount = GameObject.FindGameObjectsWithTag ("Line").Length;
	}

	void Update () {
		DrawLine ();
	}

	// Public methods

	public void Undo () {
		activeVectorList.RemoveAt (activeVectorList.Count - 1);
		activeColorList.RemoveAt (activeColorList.Count - 1);
		Destroy (lineHistory.Pop ().gameObject);
		lineCount--;
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
				headLineVectors = new List<List<Vector2>> ();
				headLineColors = new List<Color> ();
				bodyLineVectors = new List<List<Vector2>> ();
				bodyLineColors = new List<Color> ();
			}
			lineHistory = new Stack<Line> ();
			lineCount = GameObject.FindGameObjectsWithTag ("Line").Length;
			activeVectorList = freakyScene.Equals (FreakyScene.Head) ? headLineVectors : bodyLineVectors;
			activeColorList = freakyScene.Equals (FreakyScene.Head) ? headLineColors : bodyLineColors;
			ColorPicker.SelectedColor = Color.black;
		} else if (freakyScene == FreakyScene.Reveal) {
			lineCount = GameObject.FindGameObjectsWithTag ("Line").Length;
			DrawFreakie ();
		}
	}

	// Private methods

	void DrawLine () {
		Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		bool isInCanvas = hit != null && hit.collider != null && hit.collider.CompareTag("Canvas");
		bool isLineActive = activeLine != null;

		if (isLineActive && (Input.GetMouseButtonUp (0) || !isInCanvas)) {
			activeVectorList.Add (activeLine.GetPoints ());
			activeColorList.Add (activeLine.GetComponent<Renderer> ().material.color);
			lineHistory.Push (activeLine);
			activeLine = null;
		} else if (isLineActive) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			activeLine.UpdateLine (mousePos);
		} else if (Input.GetMouseButtonDown (0) && isInCanvas) {
			GameObject newLine = Instantiate (linePrefab);
			lineCount++;
			newLine.GetComponent<Renderer> ().material.color = ColorPicker.SelectedColor;
			newLine.GetComponent<Renderer> ().sortingOrder += lineCount * 10;
			activeLine = newLine.GetComponent<Line> ();
		}
	}

	void DrawFreakie () {
		for (int i = 0; i < headLineVectors.Count; i++) {
			List<Vector2> vectors = headLineVectors [i];
			GameObject newLineObj = Instantiate (linePrefab);
			Line newLine = newLineObj.GetComponent<Line> ();
			newLine.GetComponent<Renderer> ().material.color = headLineColors [i];
			vectors.ForEach (delegate(Vector2 vector) {
				newLine.UpdateLine (new Vector2(vector.x * shrinkFactor, vector.y * shrinkFactor + (canvasSize / 2)));
			});
		}
		for (int i = 0; i < bodyLineVectors.Count; i++) {
			List<Vector2> vectors = bodyLineVectors [i];
			GameObject newLineObj = Instantiate (linePrefab);
			Line newLine = newLineObj.GetComponent<Line> ();
			newLine.GetComponent<Renderer> ().material.color = bodyLineColors [i];
			vectors.ForEach (delegate(Vector2 vector) {
				newLine.UpdateLine (new Vector2(vector.x * shrinkFactor, vector.y * shrinkFactor - (canvasSize / 2)));
			});
		}
	}
}
