using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(Question))]
//public class QuestionaireEditor : PropertyDrawer
//{
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        float height = base.GetPropertyHeight(property, label) * 9;
//        bool b = property.FindPropertyRelative("onlyText").boolValue;
//        if (!b)
//            height += 115;
//        return height;
//    }
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);

//        EditorGUIUtility.LookLikeControls();
//        int indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;
//        // Draw label
//        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//        //// Don't make child fields be indented
//        //var indent = EditorGUI.indentLevel;
//        //EditorGUI.indentLevel = 0;

//        // Calculate rects
//        var rect0 = new Rect(position.x, position.y, position.width, 100);
//        var rect1 = new Rect(position.x, position.y + 140, 10, 20);
//        var rect1_1 = new Rect(position.x, position.y + 100, 100, 40);


//        //var pos = position;
//        EditorGUI.PropertyField(rect0, property.FindPropertyRelative("question"), new GUIContent("Text"));
//        EditorGUI.PropertyField(rect1_1, property.FindPropertyRelative("effect"), new GUIContent("effect"));
//        //pos.y += 30;
//        EditorGUI.PropertyField(rect1, property.FindPropertyRelative("onlyText"), new GUIContent("Show only text_____________________"));
//        bool b = property.FindPropertyRelative("onlyText").boolValue;
//        if (!b)
//        {
//            int plus = 40;
//            var rect2 = new Rect(position.x + 50, position.y + 130 + plus, position.width - 50, 20);
//            var rect3 = new Rect(position.x + 50, position.y + 150 + plus, position.width - 50, 20);
//            var rect4 = new Rect(position.x + 50, position.y + 170 + plus, position.width - 50, 20);
//            var rect5 = new Rect(position.x + 50, position.y + 190 + plus, position.width - 50, 20);
//            var rect6 = new Rect(position.x + 268, position.y + 190 + plus, position.width - 268, 20);

//            EditorGUI.PropertyField(rect2, property.FindPropertyRelative("showNumber"), new GUIContent("Show UI number:"));

//            EditorGUI.PropertyField(rect3, property.FindPropertyRelative("startText"), new GUIContent("MIN value text:"));
//            EditorGUI.PropertyField(rect4, property.FindPropertyRelative("endText"), new GUIContent("MAX value text:"));



//            var i = property.FindPropertyRelative("numberOfAnswers");
//            EditorGUI.LabelField(rect5, new GUIContent("Number of answers"));
//            i.intValue = EditorGUI.IntSlider(rect6, i.intValue,2, 12);

//        }
   
//        EditorGUI.EndProperty();

//        //property.serializedObject.ApplyModifiedProperties();
//    }
    
      
    
//    //Question myScript;
//    //SerializedProperty onlyText;

//    //// Start is called before the first frame update
//    //private void OnEnable()
//    //{
//    //    myScript = target as Question;
//    //    onlyText = serializedObject.FindProperty("test");

//    //    //style = EditorStyles.boldLabel;
//    //    //style.fontSize = 15; 
//    //}


//    //override public void OnInspectorGUI()
//    //{
//    //    GUILayout.Label("ID:", EditorStyles.miniLabel);
//    //    //myScript.ID =  GUILayout.TextField(myScript.ID);// EditorGUILayout.PropertyField(Q.test, new GUIContent("Condition array"), true);

//    //    EditorGUILayout.PropertyField(onlyText, new GUIContent("Has only text -->"));
//    //    //if (GUI.changed)
//    //    {
//    //        //EditorUtility.SetDirty(myScript);
//    //        serializedObject.ApplyModifiedProperties();
//    //    }
//    //}
//}
