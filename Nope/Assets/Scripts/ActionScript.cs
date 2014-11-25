﻿using UnityEngine;
using System.Collections;

public class ActionScript : MonoBehaviour {

	private int time;
	private int duration;
	private Vector3 destination;
	private AbstractActionScript action;
	
	public ActionScript(int time, Vector3 destination, AbstractActionScript action, int duration)
	{
		this.time = time;
		this.duration = duration;
		this.destination = destination;
		this.action = action;
	}

	public int getTime() 
	{
		return this.time;
	}

	public int getDuration() 
	{
		return this.duration;
	}

	public Vector3 getDestination() 
	{
		return this.destination;
	}

	public AbstractActionScript getAction() 
	{
		return this.action;
	}
	
}
