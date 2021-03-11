using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Khite.AutomationTest
{
    public abstract class TestScript : MonoBehaviour
    {
        public bool IsCompleted { get; protected set; } = false;
        public bool IsSuccess { get; protected set; } = true;
        public string ErrorMessage { get; private set; }
        public string LogToFix { get; private set; }

        //public void Run()
        //{
        //    StartCoroutine(EnumratorRun());
        //}

        public IEnumerator EnumratorRun()
        {
            yield return OnRun();
            IsCompleted = true;
        }

        protected abstract IEnumerator OnRun();
        protected void Check(bool value, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                Assert.IsTrue(value);
                IsSuccess = true;
                LogToFix = sourceFilePath + Config.SPLIT + Config.SUCCESS;
            }
            catch(Exception e)
            {
                string fileError = Path.GetFileName(sourceFilePath);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Failed test at <a>{fileError}:{sourceLineNumber}</a>");
                LogToFix = $"{sourceFilePath}{Config.SPLIT}{sourceLineNumber}";
                ErrorMessage = sb.ToString();

                IsSuccess = false;
            }
        }
    }
}
