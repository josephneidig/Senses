using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener: MonoBehaviour {

	public GameObject helpScreen;
	public GameObject startScreen;
	
	public void OpenPanel()
	{
		if(helpScreen != null)
		{
			helpScreen.SetActive(true);
			startScreen.SetActive(false);
		}
	}
}