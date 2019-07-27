using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Rigidbody2D rb;
	public float speed, centre, width;
	public Vector2 right_vel,grav;
	public GameObject health,over_text,food1,food2;

	Sprite[] idle_imgs,run_imgs,jump_imgs,death_imgs;
	Sprite jump1,jump2,slide;
	bool running,idle,jumping,dying,sliding,face_right;
	public bool home;
	SpriteRenderer spriteR;
	int count1,jump_count,idx;
	int run_index, idle_index, death_count, slide_count, touch_count;
	GameObject flame,control,Bar;
	Vector3 health_vec,down_dir3d,point_A;
	Vector2 down_dir;
	Collider2D[] coll;

	

	// Use this for initialization
	void Start () {
		idle_imgs = Resources.LoadAll<Sprite>("idle");
		run_imgs = Resources.LoadAll<Sprite>("run");
		death_imgs = Resources.LoadAll<Sprite>("death");
		jump1 = Resources.Load<Sprite>("jump/1");
		jump2 = Resources.Load<Sprite>("jump/2");
		slide = Resources.Load<Sprite>("slide/0");
		flame = GameObject.Find("safe");
		Bar = GameObject.Find("food_bar");
		spriteR = gameObject.GetComponent<SpriteRenderer>();
		coll = GetComponents<Collider2D>();
		control = GameObject.Find("Controller");
		running = false;
		idle = false;
		jumping = false;
		dying = false;
		count1 = 0;
		run_index = 0;
		idle_index = 0;
		slide_count = 0;
		health_vec = new Vector3(0.008f,0,0);
		down_dir = new Vector2(0,-1);
		down_dir3d = new Vector3(0,-1, 0);
		point_A = transform.position + down_dir3d;
		rb = GetComponent<Rigidbody2D>();
		idx = 0;

		speed = 12;
		centre = -2.2f;
		width = 4;
		right_vel = new Vector2(1,0);
		grav = new Vector2(0,0.5f);
		health = GameObject.Find("Bar");
		over_text = GameObject.Find("over");
		food1 = GameObject.Find("food1");
		food2 = GameObject.Find("food2");
		home= false;
		over_text.SetActive(false);

	}

	bool nearground()
	{
		if(Physics2D.Raycast(transform.position + 2*down_dir3d, down_dir, 5))
			return true;
		else return false;
	}

	void OnCollisionEnter2D(Collision2D collider_info)
	{
		if(collider_info.collider.name == "Wizard")
		{
			touch_count = 0;
		}

		RaycastHit2D hit = Physics2D.Raycast(transform.position + 2*down_dir3d, down_dir, 5);
    	if(hit != null && hit.collider.gameObject.name == "Ground" && rb.velocity.y <= 0
    		)
    	{
    		Debug.Log(hit.collider.name + idx.ToString());
    		jumping = false;
    		jump_count = 2;
    	}
	}

	void OnCollisionStay2D(Collision2D collider_info)
    {
    	//Debug.Log("aaaa");
    	RaycastHit2D hit = Physics2D.Raycast(transform.position + 2*down_dir3d, down_dir, 5);
    	if(hit != null //&& hit.collider.gameObject.name == "Ground" 
    		&& rb.velocity.y <= 0
    		)
    	{
    		Debug.Log(hit.collider.name + idx.ToString());
    		jumping = false;
    		jump_count = 2;
    	}

    	if(collider_info.collider.name == "Wizard")
		{
			touch_count++;
			if(touch_count > 60)
				over("touch");
		}
    }

    void OnCollisionExit2D(Collision2D collider_info)
    {
    	RaycastHit2D hit = Physics2D.Raycast(transform.position + 2*down_dir3d, down_dir, 5);
    	//if(hit == null)
    	if(!nearground())
    	jumping = true;

    }

    void over(string s)
    {
    	over_text.SetActive(true);
    	control.GetComponent<ControllerScript>().over = true;
    	Time.timeScale = 0;
    	Debug.Log(s);
    	this.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
		float tempx = rb.velocity.x;
        float tempy = rb.velocity.y;
        Vector2 down_dir = new Vector2(0,-1);

        rb.velocity = new Vector2(0.89f*rb.velocity.x, rb.velocity.y);

        Vector3 point_B = transform.position + 2*down_dir3d + down_dir3d;
        Debug.DrawLine(transform.position + 2*down_dir3d, point_B);

        rb.velocity -= grav;

        if(Input.GetKey("right") && !dying && !sliding)
        {
        	rb.velocity += right_vel;
        	running = true;
        	idle = false;
        	spriteR.flipX = false;
        	face_right = true;
        }
        else if(Input.GetKey("left") && !dying && !sliding)
        {
        	rb.velocity -= right_vel;
        	running = true;
        	idle = false;
        	spriteR.flipX = true;
        	face_right = false;
        }

        // if(rb.velocity.x > 0) spriteR.flipX = false;
        // else if(rb.velocity.y < 0)spriteR.flipX = true;

      	if(Input.GetKeyDown("down") && !jumping && !dying && !sliding)
        {
        	sliding = true;
        	running = false;
        	idle = false;
        	slide_count = 0;
        }
        if(Input.GetKeyDown("space") && jump_count > 0 && rb.velocity.y <= -0.5f && !dying)
        {
        	rb.velocity = new Vector2(rb.velocity.x, speed);
        	spriteR.sprite = jump1;
        	jumping = true;
        	//Debug.Log("bbb");
        	running = false;
        	idle = false;
        	sliding = false;
        	jump_count--;
        }

        if(sliding)
        {
			 if(face_right)
        	{
        		spriteR.sprite = slide;
        		rb.velocity = new Vector2(15,rb.velocity.y);
        		coll[0].enabled = false;
        	}
        	else 
        	{
        		spriteR.sprite = slide;
        		rb.velocity = new Vector2(-15,rb.velocity.y);
        		coll[0].enabled = false;
        	}
        	

        	slide_count++;
        	if(slide_count > 60) 
        	{ 
        		sliding = false;
        		coll[0].enabled = true;
        	}
        }

        if(jumping)
        {
        	if(rb.velocity.y < 0)
        	{
        		spriteR.sprite = jump2;
        	}
        }

        if(Mathf.Abs(rb.velocity.x) <= 1f && !jumping && !sliding)
       	{
       		running = false;
       		idle = true;
       		sliding = false;
       	}

       	if(transform.position.x >= centre - width && transform.position.x <= centre + width)
       	{
       		flame.SetActive(true);
       		home = true;
       	}
       	else 
       	{ 
       		flame.SetActive(false);
       		home = false;
       	}


       	if(home)
       	{
       		food1.SetActive(true);
       		food2.SetActive(true);
       	}


        if(count1 == 0)
        {
	        if(running && !jumping)
	        {
	        	spriteR.sprite = run_imgs[run_index];
            	run_index = (run_index+1)%8;
	        }

	        if(idle && !jumping)
	        {
	        	spriteR.sprite = idle_imgs[idle_index];
            	idle_index = (idle_index+1)%12;
	        }

	        
	    }

	    if(dying)
	    {
	    	if(death_count < 60) spriteR.sprite = death_imgs[0];
	    	else spriteR.sprite = death_imgs[1];
	    	if(death_count > 60) 
	    	over("health");
	    	death_count++;
	    }

	    if(!home || Bar.transform.localScale.x <= 0.2f)
       	{
       		if(health.transform.localScale.x > 0.2f)
       		health.transform.localScale -= health_vec;
       	}
       	else
       	{
       		if(health.transform.localScale.x < 5.6f && !dying)
       		health.transform.localScale += 2.0f*health_vec;
       	}

	    if(health.transform.localScale.x <= 0.2 && !dying && !jumping)
	    {
	    	death_count = 0;
	    	dying = true;
	    	Debug.Log("dead");
	    }

	    if(transform.position.y <= -60 )
	    {
	    	health.transform.localScale = new Vector3(0.2f, health.transform.localScale.y, 1);
	    	over("fall");
	    }

	    count1 = (count1 + 1)%4;
	    idx = (idx+1)%100000;

	}
}
