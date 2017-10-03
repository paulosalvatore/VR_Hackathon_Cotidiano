#define FREE_VERSION
using UnityEngine;
using System.Collections.Generic;

namespace CF.CameraBot
{
    /// <summary>CF Camera for Unity3D - a spherical coordinates camera handler.</summary>
    /// <see cref="http://www.clonefactor.com"/>
    public partial class CameraBot : MonoBehaviour
    {
        #region variable
        /// <summary>Target gameobject want to chase</summary>
        public Transform ChaseTarget;
        /// <summary>Override chase target forward direction by this transform forward.</summary>
		public Transform TargetForward;

        /// <summary>Translate object to chase target's relative position.</summary>
        public Transform ControlPosition;
        /// <summary>Rotate gameObject to facing chase target.</summary>
        public Transform ControlRotation;
        /// <summary>Input handler parameter.</summary>
        public InputSetting InputSetting;
        /// <summary>Advance Setting</summary>
        public AdvanceSetting AdvanceSetting;
		/// <summary>Currect Selected handler for camera.</summary>
		/// <remarks>Assign and change this at run time can also affect SwitchCamera method.</remarks>
		/// <see cref="SwitchCamera()"/>
		public int Selected = 0;
        /// <summary>A list for developer to setup the camera detail.</summary>
        public List<Preset> PresetList = new List<Preset>();
		public Preset activePreset { get { return PresetList[Selected]; } }
		#endregion

        #region API
        /// <summary>In order to control camera you need to giving the following values.</summary>
        /// <param name="horizontal">mouse horizontal value, rotate orbit yaw angle.</param>
        /// <param name="vertical">mouse vertical value, rotate orbit pitch angle.</param>
        /// <param name="zoom">mouse wheel value, for camera zooming usage.</param>
        public void UpdatePosition(float horizontal, float vertical, float zoom)
        {
			horizontal = InputSetting.FlipMouseX ? -horizontal : horizontal;
            vertical = InputSetting.FlipMouseY ? -vertical : vertical;
            zoom = this.InputSetting.FlipMouseWheel ? -zoom : zoom;

#if !FREE_VERSION
#else
			float amount = InputSetting.Sensitive * Time.fixedDeltaTime;
#endif

#if FREE_VERSION
			bool
				updateAngle = (horizontal * horizontal > InputSetting.Threshold || vertical * vertical > InputSetting.Threshold);
			if(updateAngle)
			{
				UpdateOrbitCoordinateData(vertical, horizontal, amount, activePreset);
			}
#endif

			// update Spherical Coordinates in memory.
			for (int i = 0; i < PresetList.Count; i++)
            {
				if (i == Selected)
					continue; // skip, handled before.

				if (updateAngle)
					UpdateOrbitCoordinateData(vertical, horizontal, amount, PresetList[i]);
            }
        }
        /// <summary>Select Camera preset by giving name.</summary>
        /// <param name="cameraLabel">the camera name on preset list.</param>
        public void SwitchCamera(string cameraLabel)
        {
            for (int i = 0; i < PresetList.Count; i++)
            {
                if (PresetList[i].name.Trim().ToLower().Equals(cameraLabel.Trim().ToLower()))
                {
                    SwitchCamera(i);
                    return;
                }
            }
        }
        /// <summary>Select Camera preset by giving index.</summary>
        /// <param name="id">the camera index on the list.</param>
        public void SwitchCamera(int id)
        {
            if (id == Selected)
                return;
            Selected = Mathf.Clamp(id, 0, PresetList.Count);
			HandlePresetActiveState();

			if (activePreset.m_Method.m_UpdateAngle == UpdateAngleMethod.ResetWhenActive)
            {
                ResetToInitStage(Selected);
            }
        }

        /// <summary>Gets current chase target.</summary>
        /// <param name="targetReference">The target reference.</param>
        /// <param name="forwardReference">The forward reference.</param>
        public void GetChaseTarget(out Transform targetReference, out Transform forwardReference)
        {
            forwardReference = (TargetForward == null) ? ChaseTarget : TargetForward;
            targetReference = ChaseTarget;
        }

		/// <summary>Sets chase target and it's forward reference.</summary>
		/// <param name="helper"></param>
		public void SetChaseTarget(ChaseTargetHelper helper)
		{
			SetChaseTarget(helper.m_Target, helper.m_TargetForward);
		}

		/// <summary>Sets chase target and it's forward reference.</summary>
		/// <param name="newTarget">The new target.</param>
		/// <param name="newForward">The new forward reference, default is newTarget itself.</param>
		public void SetChaseTarget(Transform newTarget, Transform newForward = null)
        {
            TargetForward = (newForward == null) ? newTarget : newForward;
            ChaseTarget = newTarget;
        }
        /// <summary>Gets camera position & rotation transform reference.</summary>
        /// <param name="positionReference">The position reference.</param>
        /// <param name="rotationReference">The rotation reference.</param>
        public void GetCamera(out Transform positionReference, out Transform rotationReference)
        {
            positionReference = ControlPosition;
            rotationReference = ControlRotation;
        }
        /// <summary>Sets the camera transform position pivot point and rotation transform object.</summary>
        /// <param name="positionReference">The position reference.</param>
        /// <param name="rotationReference">The rotation reference.</param>
        public void SetCamera(Transform positionReference, Transform rotationReference = null)
        {
            ControlPosition = positionReference;
            ControlRotation = (rotationReference == null) ? positionReference : rotationReference;
        }
#endregion
	}
}