using System;
using GKill.Models;
using Xunit;

namespace simba.test
{
    public class GKillTest
    {
        [Fact]
        public void ShouldList()
        {
            var runningProccesses = ProcessModel.GetProcesses();
            Assert.NotEmpty(runningProccesses);
        }
    }
}
