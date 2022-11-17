using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCloser: MonoBehaviour {

	public GameObject helpScreen;
	public GameObject startScreen;
	
	public void OpenPanel()
	{
		if(helpScreen != null)
		{
			helpScreen.SetActive(false);
			startScreen.SetActive(true);
		}
	}
}