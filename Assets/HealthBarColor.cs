using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarColor : MonoBehaviour {


	void Start()
	{

	}

	// Update is called once per frame
	void Update () {



		if (-gameObject.GetComponent<RectTransform> ().offsetMax.x >= 145) {
			gameObject.GetComponent<Image> ().color = Color.red;
		} else if (-gameObject.GetComponent<RectTransform> ().offsetMax.x >= 73) {
			gameObject.GetComponent<Image> ().color = Color.yellow;
		} else {
			gameObject.GetComponent<Image> ().color = Color.green;
		}
	}
}
