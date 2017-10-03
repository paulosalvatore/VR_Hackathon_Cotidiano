using UnityEngine;
using System.Collections;
using System;

public class FollowPlayer : MonoBehaviour
{
	public Transform target;
	private Transform model;

	public float velocidadeMovimento = 5;
	public float velocidadeRotacao = 100;

	public bool modelUpDown = false;

	internal bool moverDesabilitado = false;
	internal bool rotacionarDesabilitado = false;
	internal bool upDownWhenRotating = false;

	private void Awake()
	{
		model = transform.GetChild(0);
	}
	
    void FixedUpdate()
    {
		Mover();

		Rotacionar();

		ModelUpDown();
    }

	private bool ValidarDistancia()
	{
		return Vector3.Distance(target.position, transform.position) < 0.5f;
	}

	void Mover()
	{
		if (ValidarDistancia() || moverDesabilitado)
			return;

		rotacionarDesabilitado = false;

		modelUpDown = false;

		transform.localEulerAngles = Vector3.zero;

		Vector3 direcao = (target.position - transform.position).normalized * velocidadeMovimento * Time.smoothDeltaTime;

		transform.Translate(direcao);
	}

	private void Rotacionar()
	{
		if (!ValidarDistancia() || rotacionarDesabilitado)
			return;

		if (upDownWhenRotating)
		{
			modelUpDown = true;
			upDownWhenRotating = false;
		}

		transform.Rotate(new Vector3(0, 360, 0) * velocidadeRotacao * Time.smoothDeltaTime);
	}

	public float minimum = -1.0F;
	public float maximum = 1.0F;
	public float velocidadeUpDown;

	static float t = 0.0f;

	private void ModelUpDown()
	{
		if (!modelUpDown)
			return;
		
		model.localPosition =
			new Vector3(
				model.localPosition.x,
				Mathf.Lerp(minimum, maximum, t),
				model.localPosition.z
			);
		
		t += 0.5f * Time.smoothDeltaTime * velocidadeUpDown;
		
		if (t > 1.0f)
		{
			float temp = maximum;
			maximum = minimum;
			minimum = temp;
			t = 0.0f;
		}
	}

	/*

	private bool isTurning;

	Vector3 rotacaoAnterior;

	void Turn()
    {
		
		Vector3 pos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(pos);
		if (pos.magnitude>1)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationlDamp * Time.deltaTime);

			Vector3 rotacaoAtual = transform.rotation.eulerAngles;

			float diferenca = Vector3.Distance(rotacaoAtual, rotacaoAnterior);

			if (diferenca < 0.1f)
				isTurning = false;
			else
				isTurning = true;

			rotacaoAnterior = rotacaoAtual;
		}
        
    }

    void Move()
    {
		float useMoveSpeed = isTurning ? movementSpeed : movementSpeedNoTurnning;

		transform.position += transform.forward * useMoveSpeed * Time.smoothDeltaTime;
    }

    void Pathfinding()
    {
        RaycastHit hit;
        Vector3 raycatOffset = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;
        
        Debug.DrawLine(left, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawLine(right, transform.forward * detectionDistance, Color.blue);
        Debug.DrawLine(up, transform.forward * detectionDistance, Color.yellow);
        Debug.DrawLine(down, transform.forward * detectionDistance, Color.red);
        if (Physics.Raycast(left, transform.forward, out hit, detectionDistance))
        {
            raycatOffset += Vector3.right;
        }
        else if (Physics.Raycast(right, transform.forward, out hit, detectionDistance))
        {
            raycatOffset -= Vector3.right;
        }

        if (Physics.Raycast(up, transform.forward, out hit, detectionDistance))
        {
            raycatOffset += Vector3.right;
        }
        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance))
        {
            raycatOffset -= Vector3.right;
        }

        if (raycatOffset != Vector3.zero)
		{
			transform.Rotate(raycatOffset * 5f * Time.deltaTime);
		}
        else
            Turn();
    }

    void Death()
    {
        Destroy(this.gameObject);
    }

    public Transform getPositionOrigin()
    {
        return origin;
    }

    public void setPositionOrigin(string nameofPlayer)
    {
        origin = player;
    }

    public Transform getTargetOrigin()
    {
        return target;
    }
	*/
}