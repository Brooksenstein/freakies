using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNavigator : MonoBehaviour {

	public void toMainMenuScene () {
		GameControl.Control.SetScene (FreakyScene.Menu);
	}

	public void toHowToPlayScene () {
		GameControl.Control.SetScene (FreakyScene.HowToPlay);
	}

	public void toHeadScene() {
		GameControl.Control.SetScene (FreakyScene.Head);
	}

	public void toBodyScene () {
		GameControl.Control.SetScene (FreakyScene.Body);
	}

	public void toRevealScene () {
		GameControl.Control.SetScene (FreakyScene.Reveal);
	}
}
