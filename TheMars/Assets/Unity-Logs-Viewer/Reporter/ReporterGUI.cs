﻿using UnityEngine;
using System.Collections;

public class ReporterGUI : MonoBehaviour
{
	private Reporter reporter;
	private void Awake()
	{
		reporter = gameObject.GetComponent<Reporter>();
	}

	private void OnGUI()
	{
		reporter.OnGUIDraw();
	}
}
