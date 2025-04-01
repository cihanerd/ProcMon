using ProcessMonitor.Domain;
using ProcessMonitor.Infrastructure;

namespace ProcessMonitor.Tests
{
    [TestFixture]
    public class ProcessTrackerTests
    {
        private ProcessTracker _processTracker;

        [SetUp]
        public void Setup()
        {
            _processTracker = new ProcessTracker();
        }

        [TearDown]
        public void TearDown()
        {
            _processTracker.Dispose();
        }

        [Test]
        public void GetProcessInfo_ShouldReturnListOfProcessInfo()
        {
            var processInfoList = _processTracker.GetProcessInfo();
            Assert.That(processInfoList, Is.Not.Null);
            Assert.That(processInfoList, Is.InstanceOf<List<ProcessInfo>>());
        }

        [Test]
        public void GetProcessInfo_ShouldContainValidProcessData()
        {
            var processInfoList = _processTracker.GetProcessInfo();

            if (processInfoList.Any())
            {
                var processInfo = processInfoList.First();
                Assert.That(processInfo.Id, Is.GreaterThan(0));
                Assert.That(processInfo.Name, Is.Not.Empty);
                Assert.That(processInfo.MemoryUsage, Is.GreaterThanOrEqualTo(0));
                Assert.That(processInfo.ThreadCount, Is.GreaterThan(0));
            }
            else
            {
                Assert.Pass("No processes are running for validation, skipping test.");
            }
        }

        [Test]
        public void ProcessTracker_ShouldNotThrowException_OnDispose()
        {
            Assert.DoesNotThrow(() => _processTracker.Dispose());
        }
    }
}
