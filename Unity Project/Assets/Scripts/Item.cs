﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
	public string name;
	public int cost;
	public string type;
	public GameObject model;
	public MeshRenderer costText;
}