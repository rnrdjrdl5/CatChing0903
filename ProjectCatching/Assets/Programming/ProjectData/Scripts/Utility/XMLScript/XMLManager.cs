using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

public class XMLManager {


    public void XmlWrite(List<string> playerNames)
    {
        // Xml 문서 생성
        XmlDocument xmlDocument = new XmlDocument();

        // xml 속성 생성
        XmlElement playerDataElement = xmlDocument.CreateElement("PlayerData");

        // 문서의 자식화
        xmlDocument.AppendChild(playerDataElement);


        //인스펙터 list를 기준으로.
        foreach (string name in playerNames)
        {

            // 속성 생성
            XmlElement nameElenemt = xmlDocument.CreateElement("NameElement");
            
            // 속성에 데이터 붙임
            nameElenemt.SetAttribute("Name", name);

            // 속성을 playerdata에 붙임
            playerDataElement.AppendChild(nameElenemt);

            xmlDocument.Save("./Assets/Resources/XML/PlayerName.xml");
        }

    }
    
    public List<string> XmlRead()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/PlayerName");

        XmlDocument xmlDocument = new XmlDocument();

        xmlDocument.LoadXml(textAsset.text);

        XmlElement PlayerDataElement = xmlDocument["PlayerData"];

        List<string> Names = new List<string>();

        foreach (XmlElement playerName in PlayerDataElement)
        {
            string name;
            name = playerName.GetAttribute("Name");

            Names.Add(name);
        }

        return Names;
    }

}
