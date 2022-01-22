using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    private Event _event;
    private Map _targetComponent;
    private int _selection = -1; // -1 if no selection

    private void OnSceneGUI() 
    {
        _targetComponent = (Map)target;
        _event = Event.current;

        Handles.color = Color.red;

        int clickedID;

        switch (_targetComponent.editType)
        {
            case EditType.AddingPoints:

                _selection = -1;

                ProcessButtons();

                if (AddButtonPressed()) 
                {
                    _targetComponent.AddPoint(GetMousePosition());

                    EditorUtility.SetDirty(_targetComponent);
                }

                break;

            case EditType.DeletingPoints:

                _selection = -1;

                clickedID = ProcessButtons();

                if (clickedID != -1) 
                {
                    _targetComponent.RemovePoint(_targetComponent.points[clickedID]);

                    EditorUtility.SetDirty(_targetComponent);
                }

                break;

            case EditType.MovingPoints:

                _selection = -1;

                for (int i = 0; i < _targetComponent.points.Count; i++)
                {
                    _targetComponent.points[i].position = Handles.PositionHandle(_targetComponent.points[i].position, Quaternion.identity);
                }

                break;

            case EditType.AddingEdges:

                clickedID = ProcessButtons();

                if (clickedID != -1) 
                {
                    if (_selection == -1)
                    {
                        _selection = clickedID;
                    }
                    else 
                    {
                        _targetComponent.AddEdge(_targetComponent.points[_selection], _targetComponent.points[clickedID]);
                        _selection = -1;
                    }

                    EditorUtility.SetDirty(_targetComponent);
                }

                break;

            case EditType.DeletingEdges:

                clickedID = ProcessButtons();

                if (clickedID != -1)
                {
                    if (_selection == -1)
                    {
                        _selection = clickedID;
                    }
                    else
                    {
                        _targetComponent.RemoveEdge(_targetComponent.points[_selection], _targetComponent.points[clickedID]);
                        _selection = -1;

                        EditorUtility.SetDirty(_targetComponent);
                    }
                }

                break;

            case EditType.ExtrudingPoints:

                ProcessButtons();

                if (AddButtonPressed())
                {
                    _targetComponent.AddPoint(GetMousePosition());

                    if (_selection != -1)
                    {
                        _targetComponent.AddEdge(_targetComponent.points[_selection], _targetComponent.points[_targetComponent.points.Count - 1]);
                    }
                    
                    _selection = _targetComponent.points.Count - 1;

                    EditorUtility.SetDirty(_targetComponent);
                }

                break;
        }

        DrawEdges();

        if (GUI.changed) 
        {
            EditorUtility.SetDirty(_targetComponent);
            EditorSceneManager.MarkSceneDirty(_targetComponent.gameObject.scene);
        }
    }

    private Vector3 GetMousePosition() 
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else 
        {
            throw new Exception("Can`t get mouse position. Make sure if there are colliders on scene!");
        }
    }

    private bool AddButtonPressed()
    {
        switch (_event.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.A))
                    {
                        return true;
                    }
                    break;
                }
        }
        return false;
    }

    private int ProcessButtons() 
    {
        for (int i = 0; i < _targetComponent.points.Count; i++)
        {
            float size = 0.05f;

            if (i == _selection)
                size *= 2;

            if (Handles.Button(_targetComponent.points[i].position, SceneView.lastActiveSceneView.rotation, size, size, Handles.CircleHandleCap))
            {
                if (i == _selection)
                {
                    _selection = -1;
                    return -1;
                }
                else
                {
                    return i;
                }

            }
        }
        return -1;
    }

    private void DrawEdges()
    {
        foreach (Edge edge in _targetComponent.edges)
        {
            Debug.DrawLine(edge.point1.position, edge.point2.position, Color.black);
        }
    }
}
