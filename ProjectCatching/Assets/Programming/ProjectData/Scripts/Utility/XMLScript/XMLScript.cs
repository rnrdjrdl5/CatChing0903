using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLScript : MonoBehaviour {

    public List<string> Names;

	// Use this for initialization
	void Start () {
        XMLManager xml = new XMLManager();

        xml.XmlWrite(Names);

        Debug.Log("완료");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
