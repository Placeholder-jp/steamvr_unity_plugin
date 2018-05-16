﻿//========= Copyright 2016-2018, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.VRModuleManagement;
using System;
using System.Collections;
using Litpla.VR.Util;
using UnityEngine;
using UnityEngine.UI;

namespace HTC.UnityPlugin.Vive.BindingInterface
{
    public class BindingInterfaceRoleSetBindingItem : MonoBehaviour
    {
        [SerializeField]
        private Image m_modelIcon;
        [SerializeField]
        private Text m_deviceSN;
        [SerializeField]
        private Text m_roleName;
        [SerializeField]
        private Button m_editButton;
        [SerializeField]
        private Image m_heighLight;

        private uint m_deviceIndex;

        public string deviceSN { get; set; }
        public bool isHeighLight { get { return m_heighLight.enabled; } set { m_heighLight.enabled = value; } }
        public bool isEditing { get { return m_editButton.interactable; } set { m_editButton.interactable = !value; } }
        public event Action<string> onEditPress;
        public event Action<string> onRemovePress;

        public void RefreshDisplayInfo(ViveRole.IMap roleMap)
        {
            var roleInfo = roleMap.RoleValueInfo;
            var roleValue = roleMap.GetBoundRoleValueByDevice(deviceSN);
            var deviceModel = ViveRoleBindingsHelper.GetDeviceModelHint(deviceSN);

            m_deviceIndex = roleMap.GetMappedDeviceByRoleValue(roleValue);
            m_deviceSN.text = deviceSN;
            m_roleName.text = roleInfo.GetNameByRoleValue(roleValue);

            BindingInterfaceSpriteManager.SetupDeviceIcon(m_modelIcon, deviceModel, VRModule.IsDeviceConnected(deviceSN));
        }

        public void OnEdit()
        {
            if (onEditPress != null) { onEditPress(deviceSN); }
        }

        public void OnRemove()
        {
            if (onRemovePress != null) { onRemovePress(deviceSN); }
        }

        public void OnCalibrate()
        {
            StartCoroutine(CalibrateImpl());
        }

        IEnumerator CalibrateImpl()
        {
            SteamVR_ChaperoneUtil.Reset();

            yield return null;
            var pos = VRModule.GetCurrentDeviceState(m_deviceIndex).position;
            var rot = VRModule.GetCurrentDeviceState(m_deviceIndex).rotation;
            SteamVR_ChaperoneUtil.SetWorkingStandingZeroPoseFrom(pos, rot);
        }
    }
}