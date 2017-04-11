using Xunit;

namespace Gkill.API.Test.Utils{

    public class ProcessManagerTest{

        [Fact]
        public void AtLeastOneProcessIsRunning(){
            var runningProcesses = Gkill.API.Utils.ProccessManager.All();
            Assert.NotEmpty(runningProcesses);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(1000)]
        public void ReturnsProcessNotRunningForNonExistentId(int processId){
            int result = Gkill.API.Utils.ProccessManager.KillProcess(processId);
            Assert.Equal(Gkill.API.Utils.ProccessManager.PROCESS_NOT_RUNNING, result);
        } 
    }
}