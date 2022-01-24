using System;
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

        switch (_targetComponent.EditType)
        {
            case EditTypes.AddingPoints:

                _selection = -1;

                ProcessButtons();

                if (AddButtonPressed()) 
                {
                    _targetComponent.AddPoint(GetMousePosition());

                    EditorUtility.SetDirty(_targetComponent);
                }

                break;

            case EditTypes.DeletingPoints:

                _selection = -1;

                clickedID = ProcessButtons();

                if (clickedID != -1) 
                {
                    _targetComponent.RemovePoint(_targetComponent.Points[clickedID]);

                    EditorUtility.SetDirty(_targetComponent);
                }

                break;

            case EditTypes.MovingPoints:

                _selection = -1;

                for (int i = 0; i < _targetComponent.Points.Count; i++)
                {
                    _targetComponent.Points[i].Position = Handles.PositionHandle(_targetComponent.Points[i].Position, Quaternion.identity);
                }

                break;

            case EditTypes.AddingEdges:

                clickedID = ProcessButtons();

                if (clickedID != -1) 
                {
                    if (_selection == -1)
                    {
                        _selection = clickedID;
                    }
                    else 
                    {
                        _targetComponent.AddEdge(_targetComponent.Points[_selection], _targetComponent.Points[clickedID]);
                        _selection = -1;
                    }

                    EditorUtility.SetDirty(_targetComponent);
                }

                break;

            case EditTypes.DeletingEdges:

                clickedID = ProcessButtons();

                if (clickedID != -1)
                {
                    if (_selection == -1)
                    {
                        _selection = clickedID;
                    }
                    else
                    {
                        _targetComponent.RemoveEdge(_targetComponent.Points[_selection], _targetComponent.Points[clickedID]);
                        _selection = -1;

                        EditorUtility.SetDirty(_targetComponent);
                    }
                }

                break;

            case EditTypes.ExtrudingPoints:

                ProcessButtons();

                if (AddButtonPressed())
                {
                    _targetComponent.AddPoint(GetMousePosition());

                    if (_selection != -1)
                    {
                        _targetComponent.AddEdge(_targetComponent.Points[_selection], _targetComponent.Points[_targetComponent.Points.Count - 1]);
                    }
                    
                    _selection = _targetComponent.Points.Count - 1;

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
        for (int i = 0; i < _targetComponent.Points.Count; i++)
        {
            float size = 0.05f;

            if (i == _selection)
                size *= 2;

            if (Handles.Button(_targetComponent.Points[i].Position, SceneView.lastActiveSceneView.rotation, size, size, Handles.CircleHandleCap))
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
        foreach (Edge edge in _targetComponent.Edges)
        {
            Debug.DrawLine(edge.Point1.Position, edge.Point2.Position, Color.black);
        }
    }
}
