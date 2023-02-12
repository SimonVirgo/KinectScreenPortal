using UnityEngine;

public class PlaneToCamProjection : MonoBehaviour
{
	public Transform Plane;
	public Transform POV;

	public Transform Parent;

    public bool running = true;

	void Update()
	{
        if (running)
        {
            Transform prevParent = transform.parent;
            if (Parent != null)
            {
                transform.parent = Parent;
                transform.position = Parent.position;
            }
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            SetLossyScale(transform, 1, 1, 1);


            PointOfView = POV.transform.localPosition;
            ViewportPosition = Plane.transform.localPosition;
            ViewportRotation = Plane.transform.localEulerAngles;
            UseFixedAspectRatio = true;
            UseAspectAsViewportSize = true;
            AspectWidth = Plane.transform.lossyScale.x;
            AspectHeight = Plane.transform.lossyScale.y;


            transform.parent = prevParent;
        }
	}



	public const float MinimumNearClipPlane = 0.01f;

	public const float MinimumAspectSize = 0.01f;

	private const float DefaultNearClipPlane = 0.01f;

	private const int LowerLeftCornerIndex = 0;

	private const int UpperLeftCornerIndex = 1;

	private const int UpperRightCornerIndex = 2;

	private const int LowerRightCornerIndex = 3;

	private const float DefaultAspectWidth = 1.0f;

	private const float DefaultAspectHeight = 1.0f;

	private static readonly Vector3 DefaultPointOfView = Vector3.zero;

	private static readonly Vector3 DefaultViewportPosition = Vector3.zero;

	private static readonly Vector3 DefaultViewportRotation = Vector3.zero;

	private readonly Vector3[] localFrustumCorners = new Vector3[4];

	[HideInInspector]
	[SerializeField]
	[Tooltip("Local position of the view plane. The component Z must be greater than or equal to 0.01")]
	private Vector3 viewportPosition = DefaultViewportPosition;

	[HideInInspector]
	[SerializeField]
	[Tooltip("Local rotation of the view plane, expressed in euler angles")]
	private Vector3 viewportRotation = DefaultViewportRotation;

	[Space]

	[HideInInspector]
	[SerializeField]
	[Tooltip("The near clipping plane distance")]
	private float nearClipPlane = DefaultNearClipPlane;

	[HideInInspector]
	[SerializeField]
	[Tooltip("If true, the near clipping plane distance is set equal to the distance between the point of view and the of view plane")]
	private bool setNearClipPlane;

	[Space]

	[HideInInspector]
	[SerializeField]
	[Tooltip("Local position of the point of view")]
	private Vector3 pointOfView = DefaultPointOfView;

	[Space]

	[HideInInspector]
	[SerializeField]
	[Tooltip("If true, a fixed aspect ratio is set (AspectWidth divided by AspectHeight)")]
	private bool useFixedAspectRatio;

	[HideInInspector]
	[SerializeField]
	[Tooltip("Width of the view plane")]
	private float aspectWidth = DefaultAspectWidth;

	[HideInInspector]
	[SerializeField]
	[Tooltip("Height of the view plane")]
	private float aspectHeight = DefaultAspectHeight;

	[HideInInspector]
	[SerializeField]
	[Tooltip("If true, the fixed aspect ratio is set as the size of the view plane")]
	private bool useAspectAsViewportSize;

	private Camera cameraComponent;

	private Vector2 viewportSize;

	/// <summary>
	/// Gets the camera used
	/// </summary>
	public Camera Camera
	{
		get
		{
			return this.cameraComponent;
		}
	}

	/// <summary>
	/// Gets the local position of lower left corner of view plane
	/// </summary>
	public Vector3 LocalLowerLeftCorner
	{
		get
		{
			return this.localFrustumCorners[LowerLeftCornerIndex];
		}
	}

	/// <summary>
	/// Gets the local position of lower right corner of view plane
	/// </summary>
	public Vector3 LocalLowerRightCorner
	{
		get
		{
			return this.localFrustumCorners[LowerRightCornerIndex];
		}
	}

	/// <summary>
	/// Gets the local position of upper left corner of view plane
	/// </summary>
	public Vector3 LocalUpperLeftCorner
	{
		get
		{
			return this.localFrustumCorners[UpperLeftCornerIndex];
		}
	}

	/// <summary>
	/// Gets the local position of upper right corner of view plane
	/// </summary>
	public Vector3 LocalUpperRightCorner
	{
		get
		{
			return this.localFrustumCorners[UpperRightCornerIndex];
		}
	}

	/// <summary>
	/// Gets the world position of lower left corner of view plane
	/// </summary>
	public Vector3 LowerLeftCorner
	{
		get
		{
			return this.transform.TransformPoint(this.localFrustumCorners[LowerLeftCornerIndex]);
		}
	}

	/// <summary>
	/// Gets the world position of lower right corner of view plane
	/// </summary>
	public Vector3 LowerRightCorner
	{
		get
		{
			return this.transform.TransformPoint(this.localFrustumCorners[LowerRightCornerIndex]);
		}
	}

	/// <summary>
	/// Gets the world position of upper left corner of view plane
	/// </summary>
	public Vector3 UpperLeftCorner
	{
		get
		{
			return this.transform.TransformPoint(this.localFrustumCorners[UpperLeftCornerIndex]);
		}
	}

	/// <summary>
	/// Gets the world position of upper right corner of view plane
	/// </summary>
	public Vector3 UpperRightCorner
	{
		get
		{
			return this.transform.TransformPoint(this.localFrustumCorners[UpperRightCornerIndex]);
		}
	}

	/// <summary>
	/// Gets the size of the view plane
	/// </summary>
	public Vector2 ViewportSize
	{
		get
		{
			return this.viewportSize;
		}
	}

	/// <summary>
	/// Gets or sets the local position of the view plane. The component Z must be greater than or equal to 0.01
	/// </summary>
	public Vector3 ViewportPosition
	{
		get
		{
			return this.viewportPosition;
		}

		set
		{
			this.viewportPosition = value;
//			this.viewportPosition.z = Mathf.Max(this.viewportPosition.z, -1);
		}
	}

	/// <summary>
	/// Gets or sets the local rotation of the view plane, expressed in euler angles
	/// </summary>
	public Vector3 ViewportRotation
	{
		get
		{
			return this.viewportRotation;
		}

		set
		{
			this.viewportRotation = value;
		}
	}

	/// <summary>
	/// The near clipping plane distance
	/// </summary>
	public float NearClipPlane
	{
		get
		{
			return this.nearClipPlane;
		}

		set
		{
			this.nearClipPlane = Mathf.Max(MinimumNearClipPlane, value);
		}
	}

	/// <summary>
	/// Gets or sets a value that indicates whether the near clipping plane distance is set equal to the distance between the point of view and the of view plane
	/// </summary>
	public bool SetNearClipPlane
	{
		get
		{
			return this.setNearClipPlane;
		}

		set
		{
			this.setNearClipPlane = value;
		}
	}

	/// <summary>
	/// Local position of the point of view
	/// </summary>
	public Vector3 PointOfView
	{
		get
		{
			return this.pointOfView;
		}

		set
		{
			this.pointOfView = value;
		}
	}

	/// <summary>
	/// Global position of the point of view
	/// </summary>
	public Vector3 WorldPointOfView
	{
		get
		{
			return this.transform.TransformPoint(this.pointOfView);
		}

		set
		{
			this.pointOfView = this.transform.InverseTransformPoint(value);
		}
	}

	/// <summary>
	///  Gets or sets a value that indicates whether a fixed aspect ratio is set (AspectWidth divided by AspectHeight)
	/// </summary>
	public bool UseFixedAspectRatio
	{
		get
		{
			return this.useFixedAspectRatio;
		}

		set
		{
			this.useFixedAspectRatio = value;
		}
	}

	/// <summary>
	/// Width of the view plane
	/// </summary>
	public float AspectWidth
	{
		get
		{
			return this.aspectWidth;
		}

		set
		{
			this.aspectWidth = Mathf.Max(MinimumAspectSize, value);
		}
	}

	/// <summary>
	/// Height of the view plane
	/// </summary>
	public float AspectHeight
	{
		get
		{
			return this.aspectHeight;
		}

		set
		{
			this.aspectHeight = Mathf.Max(MinimumAspectSize, value);
		}
	}

	/// <summary>
	/// Gets or sets a value that indicates whether the fixed aspect ratio is set as the size of the view plane
	/// </summary>
	public bool UseAspectAsViewportSize
	{
		get
		{
			return this.useAspectAsViewportSize;
		}

		set
		{
			this.useAspectAsViewportSize = value;
		}
	}

	public void Reset()
	{
		this.useFixedAspectRatio = false;
		this.aspectWidth = DefaultAspectWidth;
		this.aspectHeight = DefaultAspectHeight;
		this.useAspectAsViewportSize = false;

		this.viewportPosition = DefaultViewportPosition;
		this.viewportRotation = DefaultViewportRotation;

		this.nearClipPlane = DefaultNearClipPlane;
		this.setNearClipPlane = false;

		this.pointOfView = DefaultPointOfView;

		this.cameraComponent.nearClipPlane = DefaultNearClipPlane;
		this.cameraComponent.orthographic = false;
		this.cameraComponent.ResetAspect();
		this.cameraComponent.ResetProjectionMatrix();
		this.cameraComponent.ResetWorldToCameraMatrix();
	}

	protected void Awake()
	{
		this.cameraComponent = this.GetComponent<Camera>();
	}

	protected void Start()
	{
		this.ConfigureCamera();
	}

	protected void LateUpdate()
	{
		this.ConfigureCamera();
	}

	protected void OnDrawGizmosSelected()
	{
		var originalMatrix = Gizmos.matrix;
		Gizmos.matrix = this.transform.localToWorldMatrix;

		Gizmos.color = Color.white;
		Gizmos.DrawLine(this.localFrustumCorners[LowerLeftCornerIndex], this.localFrustumCorners[UpperLeftCornerIndex]);
		Gizmos.DrawLine(this.localFrustumCorners[UpperLeftCornerIndex], this.localFrustumCorners[UpperRightCornerIndex]);
		Gizmos.DrawLine(this.localFrustumCorners[UpperRightCornerIndex], this.localFrustumCorners[LowerRightCornerIndex]);
		Gizmos.DrawLine(this.localFrustumCorners[LowerLeftCornerIndex], this.localFrustumCorners[LowerRightCornerIndex]);

		Gizmos.matrix = originalMatrix;
	}

	int lateUpdate;
	private void ConfigureCamera()
	{
        lateUpdate++;
		if (!running || lateUpdate <= 1)
		{
			return;
		}
		
		this.nearClipPlane = Mathf.Max(this.nearClipPlane, MinimumNearClipPlane);
		//this.viewportPosition.z = Mathf.Max(this.viewportPosition.z, MinimumNearClipPlane);
		this.aspectWidth = Mathf.Max(this.aspectWidth, MinimumAspectSize);
		this.aspectHeight = Mathf.Max(this.aspectHeight, MinimumAspectSize);

		this.cameraComponent.orthographic = false;
		this.cameraComponent.ResetAspect();
		this.cameraComponent.ResetProjectionMatrix();
		this.cameraComponent.ResetWorldToCameraMatrix();

		var z = viewportPosition.z;

		if (this.useFixedAspectRatio)
		{
			this.cameraComponent.aspect = this.aspectWidth / this.aspectHeight;

			if (this.useAspectAsViewportSize)
			{
				var leftCorner = -0.5f * this.aspectWidth;
				var rightCorner = 0.5f * this.aspectWidth;
				var upperCorner = 0.5f * this.aspectHeight;
				var lowerCorner = -0.5f * this.aspectHeight;

				this.localFrustumCorners[LowerLeftCornerIndex].x = leftCorner;
				this.localFrustumCorners[LowerLeftCornerIndex].y = lowerCorner;
				this.localFrustumCorners[LowerLeftCornerIndex].z = z;

				this.localFrustumCorners[LowerRightCornerIndex].x = rightCorner;
				this.localFrustumCorners[LowerRightCornerIndex].y = lowerCorner;
				this.localFrustumCorners[LowerRightCornerIndex].z = z;

				this.localFrustumCorners[UpperLeftCornerIndex].x = leftCorner;
				this.localFrustumCorners[UpperLeftCornerIndex].y = upperCorner;
				this.localFrustumCorners[UpperLeftCornerIndex].z = z;

				this.localFrustumCorners[UpperRightCornerIndex].x = rightCorner;
				this.localFrustumCorners[UpperRightCornerIndex].y = upperCorner;
				this.localFrustumCorners[UpperRightCornerIndex].z = z;
			}
			else
			{
				this.cameraComponent.CalculateFrustumCorners(
				new Rect(0, 0, 1, 1),
				z,
				Camera.MonoOrStereoscopicEye.Mono,
				this.localFrustumCorners);
			}
		}
		else
		{
			this.cameraComponent.CalculateFrustumCorners(
			  new Rect(0, 0, 1, 1),
			  z,
			  Camera.MonoOrStereoscopicEye.Mono,
			  this.localFrustumCorners);
		}

		this.viewportSize.x = Vector3.Distance(this.localFrustumCorners[LowerLeftCornerIndex], this.localFrustumCorners[LowerRightCornerIndex]);
		this.viewportSize.y = Vector3.Distance(this.localFrustumCorners[LowerLeftCornerIndex], this.localFrustumCorners[UpperLeftCornerIndex]);

		var viewportQuaternion = Quaternion.Euler(this.viewportRotation);

		for (var i = 0; i < this.localFrustumCorners.Length; i++)
		{
			this.localFrustumCorners[i].x += this.viewportPosition.x;
			this.localFrustumCorners[i].y += this.viewportPosition.y;
			this.localFrustumCorners[i] = viewportQuaternion * this.localFrustumCorners[i];
		}

		var lowerLeftCorner = this.LowerLeftCorner;
		var lowerRightCorner = this.LowerRightCorner;
		var upperLeftCorner = this.UpperLeftCorner;
		var worldEyePosition = this.WorldPointOfView;

		var screenRightAxis = lowerRightCorner - lowerLeftCorner;
		var screenUpAxis = upperLeftCorner - lowerLeftCorner;

		var lowerLeftDirection = lowerLeftCorner - worldEyePosition;
		var lowerRightDirection = lowerRightCorner - worldEyePosition;
		var upperLeftDirection = upperLeftCorner - worldEyePosition;

		if (Vector3.Dot(-Vector3.Cross(lowerLeftDirection, upperLeftDirection), lowerRightDirection) < 0.0)
		{
			screenRightAxis = -screenRightAxis;

			lowerLeftCorner = lowerRightCorner;
			lowerRightCorner = lowerLeftCorner + screenRightAxis;
			upperLeftCorner = lowerLeftCorner + screenUpAxis;

			lowerLeftDirection = lowerLeftCorner - worldEyePosition;
			lowerRightDirection = lowerRightCorner - worldEyePosition;
			upperLeftDirection = upperLeftCorner - worldEyePosition;
		}

		screenRightAxis.Normalize();
		screenUpAxis.Normalize();

		var normal = -Vector3.Cross(screenRightAxis, screenUpAxis).normalized;

		var distanceFromEyeToScreen = Mathf.Max(MinimumNearClipPlane, -Vector3.Dot(lowerLeftDirection, normal));
		this.cameraComponent.nearClipPlane = this.setNearClipPlane ? distanceFromEyeToScreen : this.nearClipPlane;

		var left = Vector3.Dot(screenRightAxis, lowerLeftDirection) * this.cameraComponent.nearClipPlane / distanceFromEyeToScreen;
		var right = Vector3.Dot(screenRightAxis, lowerRightDirection) * this.cameraComponent.nearClipPlane / distanceFromEyeToScreen;
		var bottom = Vector3.Dot(screenUpAxis, lowerLeftDirection) * this.cameraComponent.nearClipPlane / distanceFromEyeToScreen;
		var top = Vector3.Dot(screenUpAxis, upperLeftDirection) * this.cameraComponent.nearClipPlane / distanceFromEyeToScreen;

		this.cameraComponent.projectionMatrix = Perspective(left, right, bottom, top, this.cameraComponent.nearClipPlane, this.cameraComponent.farClipPlane);
		this.cameraComponent.worldToCameraMatrix = Rotation(screenRightAxis, screenUpAxis, normal) * Translation(worldEyePosition);

		if (this.useFixedAspectRatio && this.useAspectAsViewportSize)
		{
			this.cameraComponent.fieldOfView = Mathf.Atan(1f / this.cameraComponent.projectionMatrix[1, 1]) * 2f * Mathf.Rad2Deg;
		}
	}

	public static Transform SetLossyScale(Transform transform, float? x = null, float? y = null, float? z = null)
	{

		var lossyScale = Change3 (transform.lossyScale,x, y, z);

		transform.localScale = Vector3.one;
		transform.localScale = new Vector3(lossyScale.x / transform.lossyScale.x,
										   lossyScale.y / transform.lossyScale.y,
										   lossyScale.z / transform.lossyScale.z);

		return transform;
	}

	public static Vector3 Change3(Vector3 vector, float? x = null, float? y = null, float? z = null)
	{
		if (x.HasValue) vector.x = x.Value;
		if (y.HasValue) vector.y = y.Value;
		if (z.HasValue) vector.z = z.Value;
		return vector;
	}

	public static Matrix4x4 Perspective(float left, float right, float bottom, float top, float near, float far)
	{
		var perspective = new Matrix4x4();
		perspective[0, 0] = 2.0f * near / (right - left);
		perspective[0, 1] = 0.0f;
		perspective[0, 2] = (right + left) / (right - left);
		perspective[0, 3] = 0.0f;

		perspective[1, 0] = 0.0f;
		perspective[1, 1] = 2.0f * near / (top - bottom);
		perspective[1, 2] = (top + bottom) / (top - bottom);
		perspective[1, 3] = 0.0f;

		perspective[2, 0] = 0.0f;
		perspective[2, 1] = 0.0f;
		perspective[2, 2] = (far + near) / (near - far);
		perspective[2, 3] = 2.0f * far * near / (near - far);

		perspective[3, 0] = 0.0f;
		perspective[3, 1] = 0.0f;
		perspective[3, 2] = -1.0f;
		perspective[3, 3] = 0.0f;

		return perspective;
	}

	public static Matrix4x4 Rotation(Vector3 right, Vector3 up, Vector3 forward)
	{
		var rotationMatrix = new Matrix4x4();
		rotationMatrix[0, 0] = right.x;
		rotationMatrix[0, 1] = right.y;
		rotationMatrix[0, 2] = right.z;
		rotationMatrix[0, 3] = 0.0f;

		rotationMatrix[1, 0] = up.x;
		rotationMatrix[1, 1] = up.y;
		rotationMatrix[1, 2] = up.z;
		rotationMatrix[1, 3] = 0.0f;

		rotationMatrix[2, 0] = forward.x;
		rotationMatrix[2, 1] = forward.y;
		rotationMatrix[2, 2] = forward.z;
		rotationMatrix[2, 3] = 0.0f;

		rotationMatrix[3, 0] = 0.0f;
		rotationMatrix[3, 1] = 0.0f;
		rotationMatrix[3, 2] = 0.0f;
		rotationMatrix[3, 3] = 1.0f;

		return rotationMatrix;
	}
	public static Matrix4x4 Translation(Vector3 translation)
	{
		var translationMatrix = new Matrix4x4();
		translationMatrix[0, 0] = 1.0f;
		translationMatrix[0, 1] = 0.0f;
		translationMatrix[0, 2] = 0.0f;
		translationMatrix[0, 3] = -translation.x;

		translationMatrix[1, 0] = 0.0f;
		translationMatrix[1, 1] = 1.0f;
		translationMatrix[1, 2] = 0.0f;
		translationMatrix[1, 3] = -translation.y;

		translationMatrix[2, 0] = 0.0f;
		translationMatrix[2, 1] = 0.0f;
		translationMatrix[2, 2] = 1.0f;
		translationMatrix[2, 3] = -translation.z;

		translationMatrix[3, 0] = 0.0f;
		translationMatrix[3, 1] = 0.0f;
		translationMatrix[3, 2] = 0.0f;
		translationMatrix[3, 3] = 1.0f;

		return translationMatrix;
	}
}