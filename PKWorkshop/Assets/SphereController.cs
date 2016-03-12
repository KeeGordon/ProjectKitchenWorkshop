using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {

    public float rotationAngle = 5.0f;
    public float acceleration = 1.0f;
    public float maxSpeed = 10.0f;
    public float jumpStrength = 20.0f;

    bool onFloor = false;
    Vector3 direction;
    Matrix4x4 rotationMatrix;

    Matrix4x4 SetIdentityMatrix()
    {
        Matrix4x4 mat = new Matrix4x4();
        mat.SetRow(0, Vector4.zero);
        mat.SetRow(1, Vector4.zero);
        mat.SetRow(2, Vector4.zero);
        mat.SetRow(3, Vector4.zero);
        return mat;
    }

    Matrix4x4 ComputeRotationMatrix(float angleX, float angleY, float angleZ)
    {
        Matrix4x4 mat = new Matrix4x4();
        Matrix4x4 Rx = SetIdentityMatrix();
        Matrix4x4 Ry = SetIdentityMatrix();
        Matrix4x4 Rz = SetIdentityMatrix();

        Rx.m00 = 1;
        Rx.m11 = Mathf.Cos(angleX);
        Rx.m12 = -Mathf.Sin(angleX);
        Rx.m21 = Mathf.Sin(angleX);
        Rx.m22 = Mathf.Cos(angleX);

        Ry.m00 = Mathf.Cos(angleY);
        Ry.m02 = Mathf.Sin(angleY);
        Ry.m11 = 1;
        Ry.m20 = -Mathf.Sin(angleY);
        Ry.m22 = Mathf.Cos(angleY);

        Rz.m00 = Mathf.Cos(angleZ);
        Rz.m01 = -Mathf.Sin(angleZ);
        Rz.m10 = Mathf.Sin(angleZ);
        Rz.m11 = Mathf.Cos(angleZ);
        Rz.m22 = 1;

        mat = Rz * Ry;
        mat = mat * Rx;

        return mat;
    }

	// Use this for initialization
	void Start () {
        direction = new Vector3(0, 0, 1);
        rotationMatrix = ComputeRotationMatrix(0, rotationAngle, 0);
	}
	
	// Update is called once per frame
	void Update () {
        //ForwardAndRotation();
        MoveAlongAxes();
        GetJump();
	}

    void ForwardAndRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            direction = rotationMatrix * direction;
            //transform.Rotate(Vector3.up, -rotationAngle);
        }
        if(Input.GetKey(KeyCode.S))
        {
            if(transform.forward.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(-direction * acceleration);
            }
        }
        if(Input.GetKey(KeyCode.D))
        {
            direction = rotationMatrix * -direction;
            //transform.Rotate(Vector3.up, rotationAngle);
        }
        if(Input.GetKey(KeyCode.W))
        {
            if (transform.forward.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(direction * acceleration);
            }
        }
    }

    void MoveAlongAxes()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.forward.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(-Vector3.right * acceleration);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (transform.forward.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(-Vector3.forward * acceleration);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.forward.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.right * acceleration);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (transform.forward.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.forward * acceleration);
            }
        }
    }

    void GetJump()
    {
        if (onFloor && Input.GetKey(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpStrength);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag.Equals("Floor"));
        if(col.gameObject.tag.Equals("Floor"))
        {
            onFloor = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        onFloor = false;
    }
}
