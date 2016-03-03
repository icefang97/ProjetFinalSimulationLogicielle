using UnityEngine;
using System.Collections;

[RequireComponent (typeof(playerController))]
[RequireComponent(typeof(GunController))]
public class player : LivingEntity {

    public float moveSpeed = 5;
    Camera viewCamera;
    playerController controller;
    GunController gunController;
	protected override void Start () {
        base.Start();
        controller = GetComponent<playerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
	}
	
	void Update () {
        //Mouvement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);
        //Look input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray,out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin,point,Color.red);
            controller.LookAt(point);
        }
        //Weapom Input
        if (Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
	}
}
