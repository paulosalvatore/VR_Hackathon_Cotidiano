using UnityEngine;
using System.Collections;

namespace NewtonVR.Example
{
    public class NVRExampleLaserPointer : MonoBehaviour
    {
        public Color LineColor;
		public Color LineNpcColor;
		public float LineWidth = 0.02f;
        public bool ForceLineVisible = true;

		public Transform origem;

        public bool OnlyVisibleOnTrigger = true;

        private LineRenderer Line;

        private NVRHand Hand;

        private void Awake()
        {
			Line = this.GetComponent<LineRenderer>();
            Hand = this.GetComponent<NVRHand>();

            if (Line == null)
            {
                Line = this.gameObject.AddComponent<LineRenderer>();
            }

            if (Line.sharedMaterial == null)
            {
                Line.material = new Material(Shader.Find("Unlit/Color"));
                Line.material.SetColor("_Color", LineColor);
                NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
            }

            Line.useWorldSpace = true;
        }

		public OVRInput.Controller controller;

		//Public Buttons
		public bool buttonOnePress = false;
		public bool buttonTwoPress = false;
		public bool buttonStartPress = false;
		public bool buttonStickPress = false;

		//Public Capacitive Touch
		public bool thumbRest = false;
		public bool buttonOneTouch = false;
		public bool buttonTwoTouch = false;
		public bool buttonThreeTouch = false;
		public bool buttonFourTouch = false;
		public bool buttonTrigger = false;
		public bool buttonStick = false;

		//Public Near Touch
		public bool nearTouchIndexTrigger = false;
		public bool nearTouchThumbButtons = false;

		//Public Trigger & Grip
		public float trigger = 0.0f;
		public float grip = 0.0f;

		//Public Stick Axis
		Vector2 stickXYPos;
		public float stickXPos = 0.0f;
		public float stickYPos = 0.0f;

		void Update()
		{

			//Controller Position & Rotation
			transform.localPosition = OVRInput.GetLocalControllerPosition(controller);
			transform.localRotation = OVRInput.GetLocalControllerRotation(controller);

			//Controller Button State
			buttonOnePress = OVRInput.Get(OVRInput.Button.One, controller);
			buttonTwoPress = OVRInput.Get(OVRInput.Button.Two, controller);
			buttonStartPress = OVRInput.Get(OVRInput.Button.Start, controller);
			buttonStickPress = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, controller);

			//Controller Capacitive Sensors State
			thumbRest = OVRInput.Get(OVRInput.Touch.PrimaryThumbRest, controller);
			buttonOneTouch = OVRInput.Get(OVRInput.Touch.One, controller);
			buttonTwoTouch = OVRInput.Get(OVRInput.Touch.Two, controller);
			buttonThreeTouch = OVRInput.Get(OVRInput.Touch.Three, controller);
			buttonFourTouch = OVRInput.Get(OVRInput.Touch.Four, controller);
			buttonTrigger = OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, controller);
			buttonStick = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, controller);

			//Controller NearTouch State
			nearTouchIndexTrigger = OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, controller);
			nearTouchThumbButtons = OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, controller);

			//Controller Trigger State
			grip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
			trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);

			//Controller Analogue Stick State
			stickXYPos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
			stickXPos = stickXYPos.x;
			stickYPos = stickXYPos.y;


			//Output Logs
			//  

		}

		private NPC npcMarcado;

		private void LateUpdate()
        {
            if (trigger > 0f)
			{
				Line.enabled = true;

				Line.material.SetColor("_Color", LineColor);
                NVRHelpers.LineRendererSetColor(Line, LineColor, LineColor);
                NVRHelpers.LineRendererSetWidth(Line, LineWidth, LineWidth);

                RaycastHit hitInfo;
                bool hit = Physics.Raycast(origem.position, origem.forward, out hitInfo, 1000);
                Vector3 endPoint;

                if (hit == true && hitInfo.transform.gameObject.CompareTag("NPC"))
				{
					Line.material.SetColor("_Color", Color.green);
					endPoint = hitInfo.point;
					
					npcMarcado = hitInfo.transform.GetComponent<NPC>();
				}
                else
                {
					npcMarcado = null;
					endPoint = origem.position + (origem.forward * 1000f);
                }

                Line.SetPositions(new Vector3[] { origem.position, endPoint });
            }
			else
			{
				if (npcMarcado != null)
				{
					Jogo.instancia.SelecionarNpc(npcMarcado);

					npcMarcado = null;
				}

				Line.enabled = false;
			}
        }
    }
}