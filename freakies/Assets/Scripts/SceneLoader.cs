using UnityEngine;
using System.Collections.Generic;

public class SceneLoader: MonoBehaviour {

	public void LoadScene(int level)
	{ 
		Application.LoadLevel(level);
	}
}