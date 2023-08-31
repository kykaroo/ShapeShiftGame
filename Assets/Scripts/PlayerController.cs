using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Button humanButton;
    [SerializeField] private Button carButton;
    [SerializeField] private Button helicopterButton;
    [SerializeField] private Button shipButton;

    public float playerSpeed;

    public GameObject humanFormGameObject;
    public GameObject carFormGameObject;
    public GameObject helicopterFormGameObject;
    public GameObject shipFormGameObject;

    private BoxCollider _playerCollider;
    private BoxCollider _humanCollider;
    private BoxCollider _carCollider;
    private BoxCollider _helicopterCollider;
    private BoxCollider _shipCollider;

    private GameObject _previousFormGameObject;
    private void Start()
    {
        AssignButtons();
        _previousFormGameObject = humanFormGameObject;
        HumanForm();
    }

    private void AssignButtons()
    {
        humanButton.onClick.AddListener(HumanForm);
        carButton.onClick.AddListener(CarForm);
        helicopterButton.onClick.AddListener(HelicopterForm);
        shipButton.onClick.AddListener(ShipForm);
    }

    private void ShipForm()
    {
        _previousFormGameObject.SetActive(false);
        shipFormGameObject.SetActive(true);
        _previousFormGameObject = shipFormGameObject;
    }

    private void HelicopterForm()
    {
        _previousFormGameObject.SetActive(false);
        helicopterFormGameObject.SetActive(true);
        _previousFormGameObject = helicopterFormGameObject;
    }

    private void CarForm()
    {
        _previousFormGameObject.SetActive(false);
        carFormGameObject.SetActive(true);
        _previousFormGameObject = carFormGameObject;
    }

    private void HumanForm()
    {
        _previousFormGameObject.SetActive(false);
        humanFormGameObject.SetActive(true);
        _previousFormGameObject = humanFormGameObject;
    }

    private void Update()
    {
        gameObject.transform.Translate(Vector3.forward * (playerSpeed * Time.deltaTime));
    }
}
