using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController
{    
    private static SelectionController _instance;
    private Client _selectedClient;

    public static SelectionController Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SelectionController();
            return _instance;
        }
    }


    private SelectionController() { }


    public void SelectClient(Client client) 
    {
        _selectedClient = client;
    }

    public void SelectTaxi(Taxi taxi) 
    {
        if (_selectedClient) 
        {
            taxi.PickupClient(_selectedClient);
            _selectedClient = null;
        }
    }

}
