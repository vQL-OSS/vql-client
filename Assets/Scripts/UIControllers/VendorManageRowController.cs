﻿//------------------------------------------------------------------------------
// <copyright file="VendorManageRowController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Manage Row Controller
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    [RequireComponent(typeof(LayoutElement))]
    public class VendorManageRowController : MonoBehaviour
    {
        [SerializeField]
        GameObject content;
        [SerializeField]
        Text KeyCodePrefix;
        [SerializeField]
        string KeyCodeSuffix;
        LayoutElement layout;

        public IEnumerator Initialize()
        {
            layout = GetComponent<LayoutElement>();
            yield return ClearRow();
        }

        public void SetRow(params string []data)
        {
            KeyCodePrefix.text = data[0];
            KeyCodeSuffix = data[1];
            StartCoroutine(Show());
        }

        public void CallClearRow()
        {
            StartCoroutine(ClearRow());
        }

        IEnumerator ClearRow()
        {
            KeyCodePrefix.text = "";
            KeyCodeSuffix = "";
            yield return StartCoroutine(Dismiss());
        }

        IEnumerator Show()
        {
            layout.enabled = true;
            content.SetActive(true);
            yield return null;
        }

        IEnumerator Dismiss()
        {
            content.SetActive(false);
            layout.enabled = false;
            yield return null;
        }

        public void CallPass()
        {
            StartCoroutine(Pass());
        }

        IEnumerator Pass()
        {
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();

            var force = true;
            yield return StartCoroutine(Instance.WebAPI.VendorDequeue(KeyCodePrefix.text, KeyCodeSuffix, force, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.VendorDequeue>();
                if (data.Updated) Dismiss();
                // error dialog
            }
            else
            {
                // error dialog
            }
            yield return null;
            content.SetActive(true);
        }

        public void CallNotify()
        {
            StartCoroutine(Notify());
        }

        IEnumerator Notify()
        {
            //KeyCodePrefix
            //KeyCodeSuffix
            yield return null;
        }
    }
}