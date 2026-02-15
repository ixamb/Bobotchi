using System.Collections;
using System.Collections.Generic;
using Core.Runtime.Services.Delayer;
using Core.Runtime.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Core.Tests
{
    public class DelayerTests
    {
        private static readonly float[] FloatDelays = { 0.5f, 1f, 2f };
        private const float Tolerance = 0.05f;
        
        private GameObject _testGameObject;
        private IActionDelayerService _service;
        
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            ActionDelayerService.ResetInstance();
            yield return null;
            
            var existing = GameObject.Find("TestActionDelayerService");
            if (existing != null)
            {
                Object.DestroyImmediate(existing);
                yield return null;
            }
            
            _testGameObject = new GameObject("TestActionDelayerService");
            _service = _testGameObject.AddComponent<ActionDelayerService>();
            yield return null;
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_testGameObject != null)
            {
                Object.Destroy(_testGameObject);
                _testGameObject = null;
                _service = null;
            }
            
            yield return null;
            ActionDelayerService.ResetInstance();
        }
        
        [UnityTest]
        public IEnumerator CreateDelayer_ShouldSuccess([ValueSource(nameof(FloatDelays))] float delay)
        {
            var delayCode = $"TestDelay_{delay}_{StringUtils.Random(8)}";
            _service.Delay(delay, () => {}, delayCode);
            Assert.IsNotNull(_service.Get(delayCode), $"Delayer '{delayCode}' should be registered");
            yield return null;
        }

        [UnityTest]
        public IEnumerator WaitForDelayerExecution_ShouldSuccess([ValueSource(nameof(FloatDelays))] float delay)
        {
            var startTime = Time.realtimeSinceStartup;
            var endTime = 0f;
            var executed = false;
            var delayCode = $"TestDelay_{delay}_{StringUtils.Random(8)}";
            
            _service.Delay(delay, () =>
            {
                endTime = Time.realtimeSinceStartup - startTime;
                executed = true;
            }, delayCode);
            
            Assume.That(_service.Get(delayCode), Is.Not.Null, "Delayer should be registered");
            
            yield return new WaitForSecondsRealtime(delay + 0.1f);
            Assert.IsTrue(executed, $"Action should execute after {delay}s");
            Assert.That(endTime, Is.EqualTo(delay).Within(Tolerance), $"Expected ~{delay}s, actual {endTime:F3}s");
        }

        [UnityTest]
        public IEnumerator CancelDelayerExecution_ShouldSuccess([ValueSource(nameof(FloatDelays))] float delay)
        {
            var delayCode = $"TestDelay_{delay}_{StringUtils.Random(8)}";
            _service.Delay(delay, () => { }, delayCode);
            
            Assume.That(_service.Get(delayCode), Is.Not.Null, "Delayer should be registered");
            
            yield return new WaitForSecondsRealtime(delay/2);
            Assume.That(_service.Get(delayCode), Is.Not.Null, $"Delayer with {delay}s delay should execute after {delay}s");
            _service.Cancel(delayCode);
            yield return null;
            Assert.IsNull(_service.Get(delayCode), "Delayer should be null");
        }

        [UnityTest]
        [TestCaseSource(nameof(GenerateDelaySetData))]
        public IEnumerator CancelAllDelayerExecution_ShouldSuccess(float[] delays)
        {
            var delayCodes = new List<string>();
            foreach (var delay in delays)
            {
                var delayCode = $"TestDelay_{delay}_{StringUtils.Random(8)}";
                delayCodes.Add(delayCode);
                _service.Delay(delay, () => { }, delayCode);
                Assume.That(_service.Get(delayCode), Is.Not.Null, "Delayer should be registered");
            }

            yield return null;
            _service.CancelAll();
            yield return null;
            foreach (var delayCode in delayCodes)
            {
                Assert.IsNull(_service.Get(delayCode), "Delayer should be null");
            }
        }
        
        #region delay set generators

        private static IEnumerable<TestCaseData> GenerateDelaySetData()
        {
            yield return new TestCaseData(FloatDelays).SetName("Delays_Set");
        }
        
        #endregion delay set generators
    }
}