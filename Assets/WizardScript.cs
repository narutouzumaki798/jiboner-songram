using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardScript : MonoBehaviour {

	Sprite[] wizard_imgs;
	SpriteRenderer spriteR;
	int count1,count2,wizard_index;
	GameObject player,control;
	Rigidbody2D rb;
	Vector3 init;
	float hover;



	public float speed,centre,width;

	// Use this for initialization
	void Start () {
		spriteR = gameObject.GetComponent<SpriteRenderer>();
		wizard_imgs = Resources.LoadAll<Sprite>("wizard");
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.Find("Player");
		control = GameObject.Find("Controller");
		wizard_index = 0;
		init = transform.position;
		centre = -2.2f;
		width = 6;
		hover = 1;

		count1 = 0;
		count2 = 0;
	}
	
	// Update is called once per frame
	void Update () {

		

		
		if(control.GetComponent<ControllerScript>().over)
		{
			this.enabled = false;
		}
		Vector3 dest;

		if(player.GetComponent<PlayerController>().home)
		{
			dest = init;
		}
		else dest = player.transform.position;

		Vector3 dir3 = dest - transform.position;
		dir3 = Vector3.Normalize(dir3);
		Vector2 dir = new Vector2(dir3.x, dir3.y);

		rb.velocity = speed*dir3;

		if(rb.velocity.x > 0)
		{
			spriteR.flipX = true;
		}
		else
		{
			spriteR.flipX = false;
		}

		if(count1 == 0)
        {
			spriteR.sprite = wizard_imgs[wizard_index];
            wizard_index = (wizard_index+1)%6;
	    }

	    if(transform.position.x >= centre - width && transform.position.x <= centre + width
			&& !player.GetComponent<PlayerController>().home)
       	{
       		rb.velocity = new Vector2(0,hover); 
       		if(count2 == 0) hover *= -1;
       	}

		count1 = (count1 + 1)%6;
		count2 = (count2 + 1)%30;
	}
}
