using CreativeCoders.Core.IO;
using Newtonsoft.Json;

namespace CreativeCoders.Daemon.Base
{
    public class DaemonInfoFile
    {
        private readonly string _fileName;

        public DaemonInfoFile(string fileName)
        {
            _fileName = fileName;
        }

        public DaemonInfo LoadInfo()
        {
            return JsonConvert.DeserializeObject<DaemonInfo>(FileSys.File.ReadAllText(_fileName),
                new JsonSerializerSettings {MissingMemberHandling = MissingMemberHandling.Ignore});
        }

        public void SaveInfo(DaemonInfo daemonInfo)
        {
            FileSys.File.WriteAllText(_fileName, JsonConvert.SerializeObject(daemonInfo, Formatting.Indented));
        }
    }
}