using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pageManagingScript : MonoBehaviour {
	
	public int pageNum;
	Text txt;

	// Use this for initialization
	void Start () {
		pageNum=0;
	}
	
	// Update is called once per frame
	void Update () {
		//We update the page number on the UI
		Transform pageObj = transform.Find("Page");
		txt = pageObj.gameObject.GetComponent<Text>();
		txt.text = (pageNum+1).ToString();
	}
	
	public void nextPage()
	{
		if(pageNum < 9)
		{
			pageNum++;
			
			//We update the buttons
			foreach(Transform child in transform)
			{
				if(!child.gameObject.active)
					child.gameObject.active = true;
			}
		}
	}
	
	public void previousPage()
	{
		if(pageNum >= 1)
		{
			pageNum--;
			
			//We update the buttons
			foreach(Transform child in transform)
			{
				if(!child.gameObject.active)
					child.gameObject.active = true;
			}
		}
	}
}
