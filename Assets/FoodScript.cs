using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour {

	SpriteRenderer spriteR;
	Sprite[] food_imgs;
	GameObject Bar;
	Vector3 food_vec;

	// Use this for initialization
	void Start () {
		food_imgs = Resources.LoadAll<Sprite>("food");
		Bar = GameObject.Find("food_bar");
		food_vec = new Vector3(0.01f,0,0);
		spriteR = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void OnTriggerEnter2D(Collider2D col)
    {
    	Bar.transform.localScale += food_vec*500;
    	if(Bar.transform.localScale.x > 5.6) Bar.transform.localScale = new Vector3(5.6f,1,1);
    	gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
