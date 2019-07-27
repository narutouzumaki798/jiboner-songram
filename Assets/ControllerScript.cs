using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ControllerScript : MonoBehaviour {

	public bool over;
	Sprite[] food_imgs;
	GameObject Bar,player;
	Vector3 food_vec;

	// Use this for initialization
	void Start () {
		food_imgs = Resources.LoadAll<Sprite>("food");
		Bar = GameObject.Find("food_bar");
		player = GameObject.Find("Player");
		food_vec = new Vector3(0.005f,0,0);
		over = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Bar.transform.localScale.x > 0.2f && player.GetComponent<PlayerController>().home)
       		Bar.transform.localScale -= food_vec;


       	if(Input.GetKey("r"))
       	{
       		Time.timeScale = 1;
       		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       	}
	}
}
