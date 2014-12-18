using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System;


public class toolCharacters : EditorWindow{
    string className = "class name";
    string hptext ;
    string mptext;
    string strenghtext;
    string armortext;
    string attackRangetext;
    string mobilityRangetext;
    string moveSpeedtext;
    int hp;
    int mp;
    int strengh;
    int armor;
    int attackRange;
    int mobilityRange;
    int moveSpeed;
    public static UnityEngine.Object emptyObj;
    string [] specifType = { "Tenacité", "Détection des piéges", "Téléportation" };
    int [] indexScriptTab = {0,0,0,0,0,0,0,0};
    string [] scriptTab;
    string [] spriteTab;
    string [] previewSprite;
    int specifIndex = 0;
    int spriteIndex = 0;
    public CharactersAttributes attributes;
    Editor gameObjectEditor;
    GameObject newClass;
    Sprite subSprite;
    Sprite mySprite;
    BoxCollider collider;
    Rigidbody rigidb;
    Animator anim;
    
    [MenuItem ("Window/tool characters")]

    public static void showWindow()
    {
        EditorWindow.GetWindow(typeof(toolCharacters));

    }

    void OnGUI()
    {
        /*** add scripts in path to the tab and get only scripts names***/
        /* can choose any scripts */
        string [] Tab = Directory.GetFiles("Assets/Scripts", "*.cs");
        scriptTab = new string[Tab.Length + 1];
        scriptTab[0] = "aucun";
        for (int j = 0; j < Tab.Length; j++ )
        {
                Tab[j] = Tab[j].Split('\\')[1].Split('.')[0];
                scriptTab[j + 1] = Tab[j];
        }


        /*** add Sprite and Sprite's images in Tab and get only Sprites names***/
        spriteTab = Directory.GetFiles("Assets/Resources/Sprites", "*.png");
        previewSprite = Directory.GetFiles("Assets/Resources/Images", "*.png");
        for (int j = 0; j < spriteTab.Length; j++)
        {
            spriteTab[j] = spriteTab[j].Split('\\')[1].Split('.')[0];
            previewSprite[j] = previewSprite[j].Split('\\')[1].Split('.')[0];      
        }
       
          
        /*** Choose class Name ***/
        EditorGUILayout.LabelField("Characters'Editor", EditorStyles.boldLabel);
        className = EditorGUILayout.TextField("Enter name class", className);
        EditorGUILayout.BeginHorizontal();
        //int i = index;

        /*** choose Sprite and Preview it ***/
        spriteIndex = EditorGUILayout.Popup(spriteIndex, spriteTab);
        Sprite mypreviewSprite = Resources.Load<Sprite>("Images/" + previewSprite[spriteIndex]);
        

        /*** Preview ***/
        Rect area = GUILayoutUtility.GetRect(60, 80);
        area.width = 100;
        EditorGUI.DrawPreviewTexture(new Rect(area), mypreviewSprite.texture);
        EditorGUILayout.EndHorizontal();

        /*** character 's parameter ***/
        hptext = EditorGUILayout.TextField("hp",hptext);
        mptext = EditorGUILayout.TextField("mp", mptext);
        strenghtext = EditorGUILayout.TextField("Strengh", strenghtext);
        armortext = EditorGUILayout.TextField("Armor", armortext);
        attackRangetext = EditorGUILayout.TextField("Attack Range", attackRangetext);
        mobilityRangetext = EditorGUILayout.TextField("Mobility Range", mobilityRangetext);
        moveSpeedtext = EditorGUILayout.TextField("Attaque Range", moveSpeedtext);
        specifIndex = EditorGUILayout.Popup(specifIndex, specifType);

        /*** can choose script in path to add at the gameobject or any script ***/
        indexScriptTab[0] = EditorGUILayout.Popup(indexScriptTab[0], scriptTab);
        for (int s = 1; s < indexScriptTab.Length; s++)
        {

            if (indexScriptTab[s-1] != 0 )
            {
                indexScriptTab[s] = EditorGUILayout.Popup(indexScriptTab[s], scriptTab);
            }
        }

        /*** CREATE BUTTON ***/
        if (GUILayout.Button("Create")) {

            /***  class Name  ***/
            string fileName = className;
            string fileLocation = "Assets/Resources/Prefabs/" + fileName + ".prefab";

            /*** create prefab  ***/
            emptyObj = PrefabUtility.CreateEmptyPrefab(fileLocation);
            newClass = new GameObject();
            newClass.AddComponent<SpriteRenderer>();
            mySprite = Resources.Load<Sprite>("Sprites/" + spriteTab[spriteIndex]);
            newClass.GetComponent<SpriteRenderer>().sprite = mySprite;


            /*** to assign Script CharactersAttributes to newClass***/
            newClass.AddComponent<Animator>();
            anim = newClass.GetComponent<Animator>();
            RuntimeAnimatorController mycontroller = Resources.Load<RuntimeAnimatorController>("Sprites/Animation/ControllerWar");
            anim.runtimeAnimatorController = mycontroller;

            /*** to assign Script CharactersAttributes to newClass***/
            newClass.AddComponent<CharactersAttributes>();
            attributes = newClass.GetComponent<CharactersAttributes>();

            /*** to assign Script SimulateScript to newClass***/
            newClass.AddComponent<SimulateScript>();

            /*** to assign Script SimulateScript to newClass***/
            newClass.AddComponent<AnimationCharacters>();

            /*** to assign Component NetworkView to newClass***/
            newClass.AddComponent<NetworkView>();
         
            /*** assign choice script ***/
            for (int s = 0 ; s < indexScriptTab.Length; s++)
            {
                if (indexScriptTab[s] != 0)
                {
                    newClass.AddComponent(scriptTab[indexScriptTab[s]]);
                }
            }
            
            /***  assign hp  ***/
            hp = int.Parse(hptext);
            attributes.hp = hp;

            /***  assign mp  ***/
            mp = int.Parse(mptext);
            attributes.mp = mp;

            /*** assign Strengh ***/
            strengh = int.Parse(strenghtext);
            attributes.strengh = strengh;

            /*** assign Armor ***/
            armor = int.Parse(armortext);
            attributes.armor = armor;

            /*** assign Attaque Range ***/
            attackRange = int.Parse(attackRangetext);
            attributes.attackRange = attackRange;

            /*** assign Mobility Range ***/
            mobilityRange = int.Parse(mobilityRangetext);
            attributes.mobilityRange = mobilityRange;

            /*** assign Move Speed ***/
            moveSpeed = int.Parse(moveSpeedtext);
            attributes.moveSpeed = moveSpeed;

            /*** assign specification ***/
            attributes.specifications = specifType[specifIndex];

            /*** add tag to gameobject ***/
            newClass.tag = "Player";

            /*** add BoxCollider to gameobject ***/
            collider = newClass.AddComponent("BoxCollider") as BoxCollider;

            /*** add rigidbody to gameobject ***/
            rigidb = newClass.AddComponent("Rigidbody") as Rigidbody;

            /*** assign gameobject to prefab  ***/
            PrefabUtility.ReplacePrefab(newClass, emptyObj, ReplacePrefabOptions.ConnectToPrefab);
            DestroyImmediate(newClass);

         }
       
        }
       

    
        

    /*        void WeaponTypeScript(GameObject go) {
                switch (weapIndex) {
				    case 0:
                        go.AddComponent<CorpsaCorps>();
                        return;
				    case 1:
					    go.AddComponent<CharactersAttributes>();
                        return;
                
				    case 2:
					    go.AddComponent<CharactersAttributes>();
                        return;

				    default:

					    return;
			    }  
        }*/
        
    }
	
     

